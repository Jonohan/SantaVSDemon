using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drawline : MonoBehaviour
{
    public GameObject sphere;
    LineRenderer lineRenderer;
    private Vector3 prevPosittion;
    private float minDist = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        prevPosittion = sphere.transform.position;
        prevPosittion.y = 0.5f;
    }

    // Update is called once per frame
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
    }
}
