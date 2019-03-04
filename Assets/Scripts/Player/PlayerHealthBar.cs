using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerHealthBar : MonoBehaviour {

	[HideInInspector] public Player player;
    public float positionHealthBar = 30;

	// Use this for initialization
	void Start () {
		player = GetComponent<Player>();
		player.playerHealth.playerTakingDamage.AddListener(OnTakingDamage);
	}
	
	// Update is called once per frame
	void Update () {
		if (GameStatesManager.Instance.gameState != GameStatesManager.AvailableGameStates.Menu) {
			if (player.playerId.panelHealthBar) {
				Vector3 positionScreen = Camera.main.WorldToScreenPoint(player.transform.position);
				positionScreen.y += positionHealthBar;
				player.playerId.panelHealthBar.transform.position = positionScreen;
			}
		}
    }

	public void OnTakingDamage(PlayerId playerId, float healthRatio) {
		playerId.greenHealthBar.fillAmount = Mathf.Clamp(healthRatio, 0.0f, 1.0f);
		Canvas.ForceUpdateCanvases();
	}
}
