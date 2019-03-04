using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Player))]
public class PlayerActions : MonoBehaviour {

	[HideInInspector] public Player player;
	private Beam beam;

	// Use this for initialization
	void Start () {
		player = GetComponent<Player>();
		beam = player.beam.GetComponent<Beam>();
	}

	// Update is called once per frame
	void Update () {
		if (GameStatesManager.Instance.gameState == GameStatesManager.AvailableGameStates.Playing) {
			if (player.playerId.controls.GetRBumper() || player.playerId.controls.GetLBumper()) {
				if (player.playerId.controls.GetRBumper()) {    //PUSH
					beam.attackType = Beam.AttackType.Push;
				} else {    //PULL
					beam.attackType = Beam.AttackType.Pull;
				}
				player.beam.SetActive(true);
			} else {
				player.beam.SetActive(false);
			}
		}
	}
}
