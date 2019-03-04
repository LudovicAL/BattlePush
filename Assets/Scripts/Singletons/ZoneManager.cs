using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Collider2D))]
public class ZoneManager : MonoBehaviour {
    private Collider2D zoneCollider;
    public static ZoneManager Instance { get; private set; }
    public float timeToMinScale = 100;
    public float xMinScale = 2;
    public float yMinScale = 1;

    public bool IsInTheZone (Collider2D other) {
        return zoneCollider.IsTouching(other);
    }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else if (Instance != this) {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start() {
		zoneCollider = GetComponent<Collider2D>();
		GameStatesManager.Instance.GameStateChanged.AddListener(OnGameStateChange);
        OnGameStateChange();
    }

    // Update is called once per frame
    void Update() {
    }

    //Called when the GameState changes
    private void OnGameStateChange() {
        if (GameStatesManager.Instance.gameState == GameStatesManager.AvailableGameStates.Playing) {
            float xFinalPosition = Random.Range(-9.5f, 9.5f);
            float yFinalPosition = Random.Range(-4.5f, 4.5f);
            LerpManager.Instance.StartLerp(IdGetter, LocalScaleXGetter, LocalScaleXSetter, xMinScale, timeToMinScale, LerpManager.LerpMode.SmoothLerp, null);
            LerpManager.Instance.StartLerp(IdGetter, LocalScaleYGetter, LocalScaleYSetter, yMinScale, timeToMinScale, LerpManager.LerpMode.SmoothLerp, null);
            LerpManager.Instance.StartLerp(IdGetter, LocalPositionXGetter, LocalPositionXSetter, xFinalPosition, timeToMinScale, LerpManager.LerpMode.SmoothLerp, null);
            LerpManager.Instance.StartLerp(IdGetter, LocalPositionYGetter, LocalPositionYSetter, yFinalPosition, timeToMinScale, LerpManager.LerpMode.SmoothLerp, null);
        }
    }

    public int IdGetter() {
        return 1;
    }

    public float LocalScaleXGetter() {
        return transform.localScale.x;
    }

    public float LocalScaleYGetter() {
        return transform.localScale.y;
    }

    public void LocalScaleXSetter(float x) {
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }

    public void LocalScaleYSetter(float y) {
        transform.localScale = new Vector3(transform.localScale.x, y, transform.localScale.z);
    }

    public float LocalPositionXGetter() {
        return transform.localPosition.x;
    }

    public float LocalPositionYGetter() {
        return transform.localPosition.y;
    }

    public void LocalPositionXSetter(float x) {
        transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
    }

    public void LocalPositionYSetter(float y) {
        transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
    }
}
