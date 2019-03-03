using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerActions))]
[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerHealthBar))]
[RequireComponent(typeof(PlayerMovements))]
[RequireComponent(typeof(PlayerAttack))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Player : MonoBehaviour {
	public PlayerId playerId;
	[HideInInspector] public PlayerActions playerActions;
	[HideInInspector] public PlayerHealth playerHealth;
	[HideInInspector] public PlayerHealthBar playerHealthBar;
	[HideInInspector] public PlayerMovements playerMovements;
	[HideInInspector] public PlayerAttack playerAttack;
	[HideInInspector] public Rigidbody2D rigidBody2D;
    [HideInInspector] public Collider2D playerCollider2D;
    [HideInInspector] public GameObject pusher;
	[HideInInspector] public GameObject puller; 

    // Use this for initialization
    void Start () {
		playerActions = GetComponent<PlayerActions>();
		playerHealth = GetComponent<PlayerHealth>();
		playerHealthBar = GetComponent<PlayerHealthBar>();
		playerMovements = GetComponent<PlayerMovements>();
		playerAttack = GetComponent<PlayerAttack>();
		rigidBody2D = GetComponent<Rigidbody2D>();
        playerCollider2D = GetComponent<Collider2D>();
        pusher = transform.Find("Pusher").gameObject;
		puller = transform.Find("Puller").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	}
}
