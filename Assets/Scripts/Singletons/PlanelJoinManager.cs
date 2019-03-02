using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanelJoinManager : MonoBehaviour {

    public static string REDTEAM = "Red";
    public static string BLUETEAM = "Blue";
    public GameObject panelPlayerJoinedPrefab;
	public static PlanelJoinManager Instance {get; private set;}
	public GameObject panelJoinInstruction;
	public Transform panelPlayerJoinTransform;

    public GameObject panelPlayerJoinedRedPrefab;
    public GameObject panelJoinRedInstruction;
    public Transform panelPlayerJoinRedTransform;

    public GameObject panelPlayerJoinedBluePrefab;
    public GameObject panelJoinBlueInstruction;
    public Transform panelPlayerJoinBlueTransform;

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
		PlayerListManager.Instance.playerLeaving.AddListener(OnPlayerLeaving);
    }

    private void OnPlayerJoining(PlayerId playerId, bool gameFull) {
        playerId.panelJoin = Instantiate(panelPlayerJoinedPrefab, panelPlayerJoinTransform);
        playerId.panelJoin.GetComponent<RectTransform>().localScale = Vector3.one;
        playerId.panelJoin.transform.Find("Text").GetComponent<Text>().text = playerId.playerName + " joined the game!";
        if (gameFull) {
            panelJoinInstruction.SetActive(false);
        }
		Canvas.ForceUpdateCanvases();
	}

    private void joinTeam(PlayerId playerId, string teamName, GameObject panelJoinedPrefab, Transform panelJoinTransform) {
        playerId.panelJoin = Instantiate(panelJoinedPrefab, panelJoinTransform);
        playerId.panelJoin.GetComponent<RectTransform>().localScale = Vector3.one;
        playerId.panelJoin.transform.Find("Text").GetComponent<Text>().text = playerId.playerName + " joined the "+ teamName+" Team!";
        Canvas.ForceUpdateCanvases();
    }

    public void SwitchTeamUI(PlayerId playerId, char team){
        switch (team) {
            case 'r':
                Destroy(playerId.panelJoin);
                joinTeam(playerId, REDTEAM, panelPlayerJoinedRedPrefab, panelPlayerJoinRedTransform);
                break;
            case 'b':
                Destroy(playerId.panelJoin);
                joinTeam(playerId, BLUETEAM, panelPlayerJoinedBluePrefab, panelPlayerJoinBlueTransform);
                break;
            default:
                Debug.Log("This is not normal");
                break;
        };
    }

    private void OnPlayerLeaving(PlayerId playerId) {
        if (playerId.panelJoin != null) {
            Destroy(playerId.panelJoin);

        }
        panelJoinInstruction.SetActive(true);
        panelJoinRedInstruction.SetActive(true);
        panelJoinBlueInstruction.SetActive(true);
        Canvas.ForceUpdateCanvases();
	}
}
