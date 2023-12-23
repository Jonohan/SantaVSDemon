using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drawline : MonoBehaviour
{
    public GameObject sphere;
    LineRenderer lineRenderer;
    private Vector3 prevPosittion;
    private float minDist = 0.1f;

    [Header("Snowball Explosion")]
    public GameObject player;
    public float initialWidth = 1.0f;
    public float changedWidth = 2.5f; // Explosion width (hardcod test)
    public bool isBallCollide = false;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        prevPosittion = sphere.transform.position;
        prevPosittion.y = 0.5f;

        lineRenderer.startWidth = initialWidth;
        lineRenderer.endWidth = initialWidth;
    }

    void Update()
    {
        Vector3 currentPosition = sphere.transform.position;
        currentPosition.y = 0.5f;
        if (Vector3.Distance(currentPosition, prevPosittion) > minDist)
        {
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, currentPosition);
            prevPosittion = currentPosition;
        }

        SnowballCollide();
    }



    void SnowballCollide()
    {
        if (isBallCollide)
        {
            lineRenderer.startWidth = changedWidth;
            lineRenderer.endWidth = changedWidth;
            StartCoroutine(ResetLineWidth());
        }
    }

    IEnumerator ResetLineWidth()
    {
        yield return new WaitForSeconds(1);
        lineRenderer.startWidth = initialWidth;
        lineRenderer.endWidth = initialWidth;
    }

}
