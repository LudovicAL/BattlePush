using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartingManager : MonoBehaviour {
    public Text countDownText;
	public int countDownStart;

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
		}
	}
}
