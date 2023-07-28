using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuBiao : MonoBehaviour
{
    public GameObject Player;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        
    }
    private void Update()
    {
        lineRenderer.SetPosition(0, Player.transform.position);
    }
    private void OnEnable()
    {
        lineRenderer.SetPosition(1, this.transform.position);
        lineRenderer.SetPosition(0, Player.transform.position);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player)
            gameObject.SetActive(false);
    }
}
