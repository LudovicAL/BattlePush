using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour {

	public Transform blueTeamTransform;
	public Transform redTeamTransform;
	public GameObject avatarPrefab;
	public GameObject spawnPointPrefab;
	public List<Transform> availableRedSpawnPoints;
	public List<Transform> availableBlueSpawnPoints;
	private List<Transform> usedRedSpawnPoints = new List<Transform>();
	private List<Transform> usedBlueSpawnPoints = new List<Transform>();
	public static PlayerSpawnManager Instance {get; private set;}

    private void Awake() {
		if (Instance == null) {
			Instance = this;
		} else if (Instance != this) {
			Destroy(gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		PlayerListManager.Instance.playerJoining.AddListener(OnPlayerJoining);
		PlayerListManager.Instance.playerJoiningTeam.AddListener(OnPlayerJoiningTeam);
		PlayerListManager.Instance.playerLeavingTeam.AddListener(OnPlayerLeavingTeam);
		PlayerListManager.Instance.playerLeaving.AddListener(OnPlayerLeaving);
	}

	private void OnPlayerJoining(PlayerId playerId, bool isGameFull) {
		playerId.avatar = Instantiate(avatarPrefab, Vector3.zero, Quaternion.identity);
		playerId.player = playerId.avatar.GetComponent<Player>();
		playerId.player.playerId = playerId;
    }

	private void OnPlayerJoiningTeam(PlayerId playerId) {
		if (playerId.team == PlanelJoinManager.REDTEAM && availableRedSpawnPoints.Count() > 0) {
            int index = Random.Range(0, availableRedSpawnPoints.Count());
			playerId.spawnTransform = availableRedSpawnPoints[index];
			usedRedSpawnPoints.Add(availableRedSpawnPoints[index]);
			availableRedSpawnPoints.RemoveAt(index);
		} else if (playerId.team == PlanelJoinManager.BLUETEAM && availableBlueSpawnPoints.Count() > 0) {
            int index = Random.Range(0, availableBlueSpawnPoints.Count());
			playerId.spawnTransform = availableBlueSpawnPoints[index];
			usedBlueSpawnPoints.Add(availableBlueSpawnPoints[index]);
			availableBlueSpawnPoints.RemoveAt(index);
		}
		playerId.player.transform.position = playerId.spawnTransform.position;
	}

	public void OnPlayerLeavingTeam(PlayerId playerId) {
		MakeSpawnTransformAvailable(playerId);
	}

	private void OnPlayerLeaving(PlayerId playerId) {
		if (playerId.avatar != null) {
			Destroy(playerId.avatar);
		}
		MakeSpawnTransformAvailable(playerId);
	}

	private void MakeSpawnTransformAvailable(PlayerId playerId) {
		if (playerId.team == PlanelJoinManager.REDTEAM) {
			if (usedRedSpawnPoints.Contains(playerId.spawnTransform)) {
				usedRedSpawnPoints.Remove(playerId.spawnTransform);
			}
			if (!availableRedSpawnPoints.Contains(playerId.spawnTransform)) {
				availableRedSpawnPoints.Add(playerId.spawnTransform);
			}
		} else if (playerId.team == PlanelJoinManager.BLUETEAM) {
			if (usedBlueSpawnPoints.Contains(playerId.spawnTransform)) {
				usedBlueSpawnPoints.Remove(playerId.spawnTransform);
			}
			if (!availableBlueSpawnPoints.Contains(playerId.spawnTransform)) {
				availableBlueSpawnPoints.Add(playerId.spawnTransform);
			}
		}
	}
}
