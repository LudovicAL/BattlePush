using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PlayerJoiningEvent : UnityEvent<PlayerId, bool> {}

[System.Serializable]
public class PlayerLeavingEvent : UnityEvent<PlayerId> {}

public class PlayerListManager : MonoBehaviour {
	public int maxNumPlayers;
	public List<PlayerId> listOfPlayers {get; private set;}
    public List<PlayerId> listOfPlayersNull { get; private set; }
    public List<PlayerId> listOfPlayersRed { get; private set; }
    public List<PlayerId> listOfPlayersBlue { get; private set; }
    public List<PlayerId> listOfAvailablePlayers;
    public PlayerJoiningEvent playerJoining = new PlayerJoiningEvent();
    public PlayerLeavingEvent playerLeaving = new PlayerLeavingEvent();
	public int currentPlayerCount;

	public static PlayerListManager Instance {get; private set;}

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else if (Instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start () {
		currentPlayerCount = 0;
		maxNumPlayers = Mathf.Min(maxNumPlayers, listOfAvailablePlayers.Count);
        listOfPlayers = new List<PlayerId> ();
        listOfPlayersRed = new List<PlayerId>();
        listOfPlayersBlue = new List<PlayerId>();
        listOfPlayersNull = new List<PlayerId>();
    }

	void Update () {
        for (int i = listOfPlayers.Count - 1; i >= 0; i--) {
            if (listOfPlayers[i].controls.GetButtonBDown()) {
                RemovePlayer(listOfPlayers[i]);
            }
            if (listOfPlayers[i].controls.GetLHorizontal()>=0.5){
                SwitchPlayerList(listOfPlayers[i], listOfPlayersBlue, listOfPlayersRed);
            }
            if (listOfPlayers[i].controls.GetLHorizontal()<=-0.5){
                SwitchPlayerList(listOfPlayers[i], listOfPlayersRed, listOfPlayersBlue);
            }
        }
        for (int i = listOfAvailablePlayers.Count - 1; i >= 0; i--) {
            if (listOfAvailablePlayers[i].controls.GetButtonADown()) {
                AddPlayer(listOfAvailablePlayers[i]);
            }
        }
    }

	//Adds a player to the game
	private void AddPlayer(PlayerId playerId) {
		if (listOfPlayers.Count < maxNumPlayers) {
            listOfPlayers.Add(playerId);
            listOfPlayersNull.Add(playerId);
            listOfAvailablePlayers.Remove(playerId);
			currentPlayerCount = listOfPlayers.Count;
			bool gameFull = (currentPlayerCount < maxNumPlayers) ? false : true;
			playerJoining.Invoke(playerId, gameFull);
		}
	}

    private void SwitchPlayerList(PlayerId playerId, List<PlayerId> listDest, List<PlayerId> listSource) {
        if (!listDest.Contains(playerId) && (listSource.Contains(playerId)|| listOfPlayersNull.Contains(playerId))) {
            listSource.Remove(playerId);
            listOfPlayersNull.Remove(playerId);
            listDest.Add(playerId);
            updateUIDispach(playerId, listDest);
        }
    }

    private void updateUIDispach(PlayerId playerId, List<PlayerId> listDest) {
        if (listDest.Equals(listOfPlayersRed)) {
            PlanelJoinManager.Instance.SwitchTeamUI(playerId, 'r');
        } else if (listDest.Equals(listOfPlayersBlue)) {
            PlanelJoinManager.Instance.SwitchTeamUI(playerId, 'b');
        }
    }

	//Removes a player from the game
	private void RemovePlayer(PlayerId playerId) {
        listOfAvailablePlayers.Add(playerId);
        listOfPlayers.Remove(playerId);
        listOfPlayersBlue.Remove(playerId);
        listOfPlayersRed.Remove(playerId);
        listOfPlayersNull.Remove(playerId);
        currentPlayerCount = listOfPlayers.Count;
		playerLeaving.Invoke(playerId);
	}
}
