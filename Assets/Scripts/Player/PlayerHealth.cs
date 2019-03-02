using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Player))]
public class PlayerHealth : MonoBehaviour {
    public int DAMAGE_BY_SECOND = 1;

	[HideInInspector] public Player player;
	public PlayerTakingDamage playerTakingDamage = new PlayerTakingDamage();
	public PlayerDying playerDying = new PlayerDying();

	[System.Serializable]
	public class PlayerTakingDamage : UnityEvent<PlayerId, float> {}

	[System.Serializable]
	public class PlayerDying : UnityEvent<PlayerId> {}

	// Use this for initialization
	void Start () {
		player = GetComponent<Player>();
        player.playerId.currentHealth = player.playerId.maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
        float timeSinceLastDamage = Time.realtimeSinceStartup - player.playerId.lastTimeDamageTaken;
        if (timeSinceLastDamage > 1 && true) //Remplacer true par isInTheZone
        {
            TakeDamage(DAMAGE_BY_SECOND);
        }
	}

	//Call this function when dealing damage to this player
	public void TakeDamage(int amount) {
		if (player.playerId.currentHealth > 0) {
			player.playerId.currentHealth -= amount;
            player.playerId.lastTimeDamageTaken = Time.realtimeSinceStartup;
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
        }
    }
}
