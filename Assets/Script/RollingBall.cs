using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class RollingBall : MonoBehaviour
{
    public Transform target;
    public Vector3 initialOffset = new Vector3(0, 0, 1.2f);
    public float initialRotationSpeed = 100.0f;
    public float growthRate = 0.1f;

    private Vector3 initialScale;
    private float initialRadius;

    void Start()
    {
        initialScale = transform.localScale;
        initialRadius = initialScale.x / 2.0f;
    }

    void Update()
    {
        if (target != null)
        {
            // ball is on local offset of player, as it get bigger it get away from player
            float currentRadius = transform.localScale.x / 2.0f;
            float offsetIncrease = currentRadius - initialRadius;
            Vector3 updatedOffset = initialOffset + initialOffset.normalized * offsetIncrease;

            transform.position = target.TransformPoint(updatedOffset);

            // Larger radius, smaller angular vecolity, v=wr
            float rotationSpeed = initialRotationSpeed * (initialRadius / currentRadius);

            // Rotation base on player's direction
            Vector3 relativeXAxis = target.right;
            transform.RotateAround(target.position, relativeXAxis, rotationSpeed * Time.deltaTime);

            // Size change
            // transform.localScale = initialScale + Vector3.one * growthRate * Time.time;
        }
    }
}

