using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Collider2D))]
public class ZoneManager : MonoBehaviour
{
    private Collider2D zoneCollider;
    public static ZoneManager Instance { get; private set; }
    public float timeToMinScale = 100;
    public float xMinScale = 1;
    public float yMinScale = 1;

    public bool IsInTheZone (Collider2D other)
    {
        return zoneCollider.IsTouching(other);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        zoneCollider = GetComponent<Collider2D>();

        LerpManager.Instance.StartLerp(IdGetter, LocalScaleXGetter, LocalScaleXSetter, xMinScale, timeToMinScale, LerpManager.LerpMode.SmoothLerp, null);
        LerpManager.Instance.StartLerp(IdGetter, LocalScaleYGetter, LocalScaleYSetter, yMinScale, timeToMinScale, LerpManager.LerpMode.SmoothLerp, null);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public int IdGetter()
    {
        return 1;
    }

    public float LocalScaleXGetter()
    {
        return transform.localScale.x;
    }

    public float LocalScaleYGetter()
    {
        return transform.localScale.y;
    }

    public void LocalScaleXSetter(float x)
    {
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }

    public void LocalScaleYSetter(float y)
    {
        transform.localScale = new Vector3(transform.localScale.x, y, transform.localScale.z);
    }
}
