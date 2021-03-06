﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarsManager : MonoBehaviour {

	public GameObject panelHealthBarPrefab;
    public Vector3 healthBarScale = new Vector3(0.05f, 0.01f, 1f);

    public static HealthBarsManager Instance {get; private set;}
	

	private void Awake() {
		if (Instance == null) {
			Instance = this;
		} else if (Instance != this) {
			Destroy(gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		PlayerListManager.Instance.playerJoining.AddListener(OnPlayerJoining);
		PlayerListManager.Instance.playerLeaving.AddListener(OnPlayerLeaving);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnPlayerJoining(PlayerId playerId, bool gameFull) {
		playerId.panelHealthBar = Instantiate(panelHealthBarPrefab, CanvasManager.Instance.panelPlaying.transform);
		playerId.panelHealthBar.GetComponent<RectTransform>().localScale = healthBarScale;
		playerId.greenHealthBar = playerId.panelHealthBar.transform.Find("Panel Green").gameObject.GetComponent<Image>();
		Canvas.ForceUpdateCanvases();
	}

	private void OnPlayerLeaving(PlayerId playerId) {
		if (playerId.panelHealthBar != null) {
			Destroy(playerId.panelHealthBar);
			Canvas.ForceUpdateCanvases();
		}
	}
}
