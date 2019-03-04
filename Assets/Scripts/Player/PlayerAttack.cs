﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerAttack : MonoBehaviour {

	[HideInInspector] public Player player;

    // Start is called before the first frame update
    void Start() {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update() {
    }

    public void SetPusherActivation(bool activate) {
    	player.beam.SetActive(activate);
    }

    public void SetPullerActivation(bool activate) {
    	player.beam.SetActive(activate);
    }
}
