using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class RollingBall : MonoBehaviour
{
    public Transform target;
    public int ballColor;
    public Vector3 initialOffset = new Vector3(0, 0, 1.2f);
    public float initialRotationSpeed = 100.0f;
    private float growthRate = 0.01f;
    public GameObject ground;
    public Rigidbody rb;

    private Vector3 initialScale;
    private float initialRadius;

    void Start()
    {
        initialScale = transform.localScale;
        initialRadius = initialScale.x / 2.0f;
        rb = GetComponent<Rigidbody>();
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
            int sizechange = ground.GetComponent<ground>().color((int)(10 * (transform.position.x + 15)), (int)(10 * (transform.position.z + 15)), 5 * (transform.localScale.x), ballColor);
            //Debug.Log("color" + ballColor.ToString() + " " + sizechange.ToString() + " " + (10 * (transform.position.x + 15)).ToString() + " " + (10 * (transform.position.z + 15)).ToString());
            float velo = Mathf.Sqrt(rb.velocity.x * rb.velocity.x + rb.velocity.y * rb.velocity.y);
            Debug.Log("color" + ballColor.ToString() + " " + sizechange.ToString());
            if (sizechange > 0)
            {
                if (transform.localScale.x > 0.3) transform.localScale = transform.localScale - Vector3.one * growthRate * velo;
            } else
            {
                if (transform.localScale.x < 2) transform.localScale = transform.localScale + Vector3.one * growthRate * velo;
            }
        }
    }
}

