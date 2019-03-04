using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Player))]
public class PlayerHealth : MonoBehaviour {
    public int DAMAGE_BY_SECOND = 10;

	[HideInInspector] public Player player;
	public PlayerTakingDamage playerTakingDamage = new PlayerTakingDamage();
	public PlayerDying playerDying = new PlayerDying();

	[System.Serializable]
	public class PlayerTakingDamage : UnityEvent<PlayerId, float> {}

	[System.Serializable]
	public class PlayerDying : UnityEvent<PlayerId> {}

    public float lastTimeDamageTaken = 0;

    // Use this for initialization
    void Start () {
		player = GetComponent<Player>();
        player.playerId.currentHealth = player.playerId.maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
        if (GameStatesManager.Instance.gameState.Equals(GameStatesManager.AvailableGameStates.Playing))
        {
            float timeSinceLastDamage = Time.realtimeSinceStartup - lastTimeDamageTaken;
            if (timeSinceLastDamage > 1 && !ZoneManager.Instance.IsInTheZone(player.playerCollider2D))
            {
                TakeDamage(DAMAGE_BY_SECOND);
            }
        }
	}

	//Call this function when dealing damage to this player
	public void TakeDamage(int amount) {
		if (player.playerId.currentHealth > 0) {
			player.playerId.currentHealth -= amount;
            lastTimeDamageTaken = Time.realtimeSinceStartup;
            playerTakingDamage.Invoke(player.playerId, (float)player.playerId.currentHealth / (float)player.playerId.maxHealth);
            ToDieOrNotToDie();
        }
	}

    public void ToDieOrNotToDie()
    {
        if (player.playerId.currentHealth <= 0)
        {
            player.playerId.currentHealth = 0;
            AudioManager.Instance.PlayClip(AudioManager.Instance.GetRandomClipFromList(AudioManager.Instance.listOfDeath));
            playerDying.Invoke(player.playerId);
            Destroy(player.playerId.panelHealthBar);
            Destroy(player.playerId.avatar);
        }
    }
}
