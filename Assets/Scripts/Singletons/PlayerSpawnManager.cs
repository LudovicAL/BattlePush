using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour {

	public Transform blueTeamTransform;
	public Transform redTeamTransform;
	public GameObject avatarPrefab;
	public GameObject spawnPointPrefab;
	public List<GameObject> redSpawnPointList;
	public List<GameObject> blueSpawnPointList;
	public static PlayerSpawnManager Instance {get; private set;}

	private void Awake() {
		if (Instance == null) {
			Instance = this;
		} else if (Instance != this) {
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}

	// Use this for initialization
	void Start () {
		PlayerListManager.Instance.playerJoining.AddListener(OnPlayerJoining);
		PlayerListManager.Instance.playerJoiningTeam.AddListener(OnPlayerJoiningTeam);
		PlayerListManager.Instance.playerLeavingTeam.AddListener(OnPlayerLeavingTeam);
		PlayerListManager.Instance.playerLeaving.AddListener(OnPlayerLeaving);
	}

	private void OnPlayerJoining(PlayerId playerId, bool isGameFull) {
		playerId.avatar = Instantiate(avatarPrefab, playerId.spawnPosition, Quaternion.identity);
		playerId.avatar.GetComponent<Player>().playerId = playerId;
	}

	private void OnPlayerJoiningTeam(PlayerId playerId) {
		if (playerId.team == PlanelJoinManager.REDTEAM && redSpawnPointList.Count() > 0) {
			int index = Random.Range(0, redSpawnPointList.Count());
			playerId.spawnPosition = redSpawnPointList[index].transform.position;
			Destroy(redSpawnPointList[index]);
			blueSpawnPointList.RemoveAt(index);
		} else if (playerId.team == PlanelJoinManager.BLUETEAM && blueSpawnPointList.Count() > 0) {
			int index = Random.Range(0, blueSpawnPointList.Count());
			playerId.spawnPosition = blueSpawnPointList[index].transform.position;
			Destroy(blueSpawnPointList[index]);
			blueSpawnPointList.RemoveAt(index);
		}
	}

	public void OnPlayerLeavingTeam(PlayerId playerId) {
		if (playerId.team == PlanelJoinManager.REDTEAM) {
			GameObject spawnPoint = Instantiate(spawnPointPrefab, redTeamTransform);
			spawnPoint.transform.position = playerId.spawnPosition;
			redSpawnPointList.Add(spawnPoint);
		} else {
			GameObject spawnPoint = Instantiate(spawnPointPrefab, blueTeamTransform);
			spawnPoint.transform.position = playerId.spawnPosition;
			blueSpawnPointList.Add(spawnPoint);
		}
	}

	private void OnPlayerLeaving(PlayerId playerId) {
		if (playerId.avatar != null) {
			if (playerId.team == PlanelJoinManager.REDTEAM) {
				GameObject spawnPoint = Instantiate(spawnPointPrefab, redTeamTransform);
				spawnPoint.transform.position = playerId.spawnPosition;
				redSpawnPointList.Add(spawnPoint);
			} else if (playerId.team == PlanelJoinManager.BLUETEAM) {
				GameObject spawnPoint = Instantiate(spawnPointPrefab, blueTeamTransform);
				spawnPoint.transform.position = playerId.spawnPosition;
				blueSpawnPointList.Add(spawnPoint);
			}
			Destroy(playerId.avatar);
		}
	}
}
