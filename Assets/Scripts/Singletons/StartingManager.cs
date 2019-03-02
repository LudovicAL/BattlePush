using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartingManager : MonoBehaviour
{
    public Text countDown;

    private IEnumerator startCountDown()
    {
        for (int i = 3; i >= 0; i--)
        {
            countDown.text = (i==0)?"Let's Go!":i.ToString();
            yield return new WaitForSeconds(0.9f);
        }
        nextState();
    }

    // Start is called before the first frame update
    void Start()
    {
        countDown.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStatesManager.Instance.gameState.Equals(GameStatesManager.AvailableGameStates.Starting)) {
            StartCoroutine(startCountDown());
        }
    }
    private void nextState() {
        GameStatesManager.Instance.ChangeGameStateTo(GameStatesManager.AvailableGameStates.Playing);
    } 
}
