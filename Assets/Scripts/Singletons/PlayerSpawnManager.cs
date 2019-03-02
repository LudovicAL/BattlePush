using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour {

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
		PlayerListManager.Instance.PlayerJoiningTeam.AddListener(OnPlayerJoiningTeam);
		PlayerListManager.Instance.playerLeaving.AddListener(OnPlayerLeaving);
	}

	private void OnPlayerJoiningTeam(PlayerId playerId) {
		if (playerId.team == PlanelJoinManager.REDTEAM && redSpawnPointList.Count() > 0) {
			int index = Random.Range(0, redSpawnPointList.Count());
			playerId.spawnPosition = redSpawnPointList[index].transform.position;
			redSpawnPointList.RemoveAt(index);
		} else if (playerId.team == PlanelJoinManager.BLUETEAM && blueSpawnPointList.Count() > 0) {
			int index = Random.Range(0, blueSpawnPointList.Count());
			playerId.spawnPosition = blueSpawnPointList[index].transform.position;
			blueSpawnPointList.RemoveAt(index);
		}
		playerId.avatar = Instantiate(avatarPrefab, playerId.spawnPosition, Quaternion.identity);
		playerId.avatar.GetComponent<Player>().playerId = playerId;
	}

	private void OnPlayerLeaving(PlayerId playerId) {
		if (playerId.avatar != null) {
			GameObject spawnPoint = Instantiate(spawnPointPrefab, playerId.player.transform.position, Quaternion.identity);
			if (playerId.team == PlanelJoinManager.REDTEAM) {
				redSpawnPointList.Add(spawnPoint);
			} else {
				blueSpawnPointList.Add(spawnPoint);
			}
			Destroy(playerId.avatar);
		}
	}
}
