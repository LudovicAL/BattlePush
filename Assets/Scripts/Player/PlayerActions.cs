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
		if (player.playerId.controls.GetRBumper()) {
			player.playerAttack.SetPusherActivation(true);
			player.playerAttack.SetPullerActivation(false);
		} else {
			player.playerAttack.SetPusherActivation(false);
		}
		if (player.playerId.controls.GetLBumper()) {
			player.playerAttack.SetPullerActivation(true);
			player.playerAttack.SetPusherActivation(false);	
		} else {
			player.playerAttack.SetPullerActivation(false);
		}
	}

	public void Attack() {
		// Invoke PlayerAttack
	}
}
