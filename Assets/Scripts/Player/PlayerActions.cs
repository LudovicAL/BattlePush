using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Player))]
public class PlayerActions : MonoBehaviour {

	[HideInInspector] public Player player;

	// Use this for initialization
	void Start () {
		player = GetComponent<Player>();
	}

	// Update is called once per frame
	void Update () {
		if (GameStatesManager.Instance.gameState == GameStatesManager.AvailableGameStates.Playing) {
			player.pushBeam.SetActive(player.playerId.controls.GetRBumper());
			player.pullBeam.SetActive(player.playerId.controls.GetLBumper());
		}
	}
}
