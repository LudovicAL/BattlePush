using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartingManager : MonoBehaviour {
    public Text countDownText;
	public int countDownStart;
	public Text endText;

    private IEnumerator countDown() {
        for (int i = countDownStart; i >= 0; i--) {
            countDownText.text = (i == 0)? "Let's Go!" : i.ToString();
            yield return new WaitForSeconds(0.7f);
        }
		GameStatesManager.Instance.ChangeGameStateTo(GameStatesManager.AvailableGameStates.Playing);
	}

    // Start is called before the first frame update
    void Start() {
		countDownText.text = countDownStart.ToString();
		GameStatesManager.Instance.GameStateChanged.AddListener(OnGameStateChange);
	}

    // Update is called once per frame
    void Update() {
		    
    }


	//Called when the GameState changes
	private void OnGameStateChange() {
		if (GameStatesManager.Instance.gameState == GameStatesManager.AvailableGameStates.Starting) {
			StartCoroutine(countDown());
		} else if (GameStatesManager.Instance.gameState == GameStatesManager.AvailableGameStates.Ending) {
            AudioManager.Instance.StopClip(AudioManager.Instance.AmbianceClip);
            AudioManager.Instance.PlayClip(AudioManager.Instance.VictoryClip);
            if (PlayerListManager.Instance.listOfPlayersRed.Count > 0 && PlayerListManager.Instance.listOfPlayersBlue.Count > 0) {
				endText.text = "Les deux équipes ont gagné... bravo Ludo... c'est toi qui a programmé ce boutte là.";
			} else if (PlayerListManager.Instance.listOfPlayersRed.Count > 0) {
				endText.text = "Les rouges gagnent. Ils ont probablement triché.";
			} else if (PlayerListManager.Instance.listOfPlayersBlue.Count > 0) {
				endText.text = "Les bleus gagnent. Dans les dents les rouges!";
			} else {
				endText.text = "Personne ne gagne... c'est quoi ce jeu?";
			}
		}
	}
}
