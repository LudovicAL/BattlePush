using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class PlayerJoiningEvent : UnityEvent<PlayerId, bool> {}

[System.Serializable]
public class PlayerLeavingEvent : UnityEvent<PlayerId> {}

[System.Serializable]
public class PlayerJoiningTeamEvent : UnityEvent<PlayerId> {}

public class PlayerListManager : MonoBehaviour {
	public int maxNumPlayers;
    public Text errorText;
	public List<PlayerId> listOfPlayers {get; private set;}
    public List<PlayerId> listOfPlayersNull { get; private set; }
    public List<PlayerId> listOfPlayersRed { get; private set; }
    public List<PlayerId> listOfPlayersBlue { get; private set; }
    public List<PlayerId> listOfAvailablePlayers;
    public PlayerJoiningEvent playerJoining = new PlayerJoiningEvent();
    public PlayerLeavingEvent playerLeaving = new PlayerLeavingEvent();
    public PlayerJoiningTeamEvent playerJoiningTeam = new PlayerJoiningTeamEvent();
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
        if (GameStatesManager.Instance.gameState.Equals(GameStatesManager.AvailableGameStates.Menu))
        {
            for (int i = listOfPlayers.Count - 1; i >= 0; i--)
            {
                if (listOfPlayers[i].controls.GetLHorizontal() >= 0.5)
                {
                    SwitchPlayerList(listOfPlayers[i], listOfPlayersBlue, listOfPlayersRed);
                }
                if (listOfPlayers[i].controls.GetLHorizontal() <= -0.5)
                {
                    SwitchPlayerList(listOfPlayers[i], listOfPlayersRed, listOfPlayersBlue);
                }
                if (listOfPlayers[i].controls.GetButtonStartDown()){
                    if (listOfPlayersNull.Count == 0 && (listOfPlayersRed.Count != 0 && listOfPlayersBlue.Count != 0))
                    {
                        GameStatesManager.Instance.ChangeGameStateTo(GameStatesManager.AvailableGameStates.Starting);
                    }
                    else {
                        errorText.text = "You must have one player min. in each team and no one in the middle";
                    }
                }
                if (listOfPlayers[i].controls.GetButtonBDown())
                {
                    RemovePlayer(listOfPlayers[i]);
                }
            }
            for (int i = listOfAvailablePlayers.Count - 1; i >= 0; i--)
            {
                if (listOfAvailablePlayers[i].controls.GetButtonADown())
                {
                    AddPlayer(listOfAvailablePlayers[i]);
                }
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
            if (listSource.Contains(playerId))
                listSource.Remove(playerId);
            if (listOfPlayersNull.Contains(playerId))
                listOfPlayersNull.Remove(playerId);
            listDest.Add(playerId);
            updateUIDispach(playerId, listDest);
        }
    }

    private void updateUIDispach(PlayerId playerId, List<PlayerId> listDest) {
        if (listDest.Equals(listOfPlayersRed)) {
            PlanelJoinManager.Instance.SwitchTeamUI(playerId, 'r');
            playerId.team = PlanelJoinManager.REDTEAM;
            playerJoiningTeam.Invoke(playerId);
        } else if (listDest.Equals(listOfPlayersBlue)) {
            PlanelJoinManager.Instance.SwitchTeamUI(playerId, 'b');
            playerId.team = PlanelJoinManager.BLUETEAM;
            playerJoiningTeam.Invoke(playerId);
        }
    }

	//Removes a player from the game
	private void RemovePlayer(PlayerId playerId) {
        listOfAvailablePlayers.Add(playerId);
        listOfPlayers.Remove(playerId);
        if (listOfPlayersBlue.Contains(playerId))
            listOfPlayersBlue.Remove(playerId);
        if (listOfPlayersRed.Contains(playerId))
            listOfPlayersRed.Remove(playerId);
        if (listOfPlayersNull.Contains(playerId))
            listOfPlayersNull.Remove(playerId);
        currentPlayerCount = listOfPlayers.Count;
		playerLeaving.Invoke(playerId);
	}
}
