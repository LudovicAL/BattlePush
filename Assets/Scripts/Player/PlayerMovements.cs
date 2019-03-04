using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerMovements : MonoBehaviour {

	[HideInInspector] public Player player;

    private float movementSpeed = 5f;
    public float maxSpeed = 25f;
    public float aimingThreshold = 0.75f;
    public float movingThreshold = 0.15f;


    // Start is called before the first frame update
    void Start() {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update() {

        // Rotation
        float xTarget = player.playerId.controls.GetRHorizontal();
        float yTarget = player.playerId.controls.GetRVertical();

        if (Mathf.Abs(xTarget) + Mathf.Abs(yTarget) > aimingThreshold) {
            Vector2 target = new Vector2(xTarget, -yTarget);
            float angle = Mathf.Atan2(target.x, target.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        // Movement
        float xSpeed = movementSpeed * player.playerId.controls.GetLHorizontal();
        float ySpeed = movementSpeed * player.playerId.controls.GetLVertical();
        if (player.rigidBody2D.velocity.magnitude > maxSpeed) {
            player.rigidBody2D.velocity = player.rigidBody2D.velocity.normalized * maxSpeed;
        } else {
            player.rigidBody2D.AddForce(new Vector2(xSpeed, ySpeed) * Time.deltaTime, ForceMode2D.Impulse); 
        }
    }

    public void PlayerAiming() {
    	
    }

    public void PlayerMoving() {

    }
}
