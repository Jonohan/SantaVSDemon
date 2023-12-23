using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class RollingBall : MonoBehaviour
{
    public Transform target;
    private GameObject targetGameObject;// use to track the target

    public int ballColor;
    public Vector3 initialOffset = new Vector3(0, 0, 1.2f);
    public float initialRotationSpeed = 100.0f;
    private float growthRate = 0.01f;
    public GameObject ground;
    public Rigidbody rb;
    public GameObject colormesh;
    public GameObject oppocolor;

    private Vector3 initialScale;
    private float initialRadius;
    private Vector3 prevPosition;
    private float minDist = 0.1f;

    public bool isPickedUp = false;
    public BallCollider ballCollider;


    void Start()
    {
        initialScale = transform.localScale;
        initialRadius = initialScale.x / 2.0f;
        prevPosition = transform.position;
        prevPosition.y = 0.5f;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 currentPosition = transform.position;
        currentPosition.y = 0.5f;

        if (Vector3.Distance(currentPosition, prevPosition) > minDist)
        {
            prevPosition = currentPosition;
            color(2);
        }
        if (target != null && target.gameObject.activeInHierarchy)
        {
            isPickedUp = true;
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

        }
        else if (target == null && targetGameObject != null && !targetGameObject.activeInHierarchy)
        {
            isPickedUp = false;
            if (gameObject != null)
            {
                // Scale is 1
                transform.localScale = Vector3.one;
            }

            if (rb != null)
            {
                // Cannot move until player touch it
                rb.isKinematic = !isPickedUp;
            }
        }
        
    }

    // Release ball
    public void ReleaseBall()
    {
        isPickedUp = false;

        // Push the ball in the opposite direction from the player's line
        Vector3 direction = (transform.position - target.position).normalized;

        // Ball won't follow player
        target = null;

        // Add force index
        rb.isKinematic = false;
        rb.AddForce(direction * 700.0f);

        if (ballCollider != null)
        {
            
            ballCollider.SetBallFree(true);
            //Debug.Log("here");
        }
    }

    public void color(int radius)
    {
        // Size change
        // transform.localScale = initialScale + Vector3.one * growthRate * Time.time;
        float sizechange = ground.GetComponent<ground>().color((int)(10 * (transform.position.x + 15)), (int)(10 * (transform.position.z + 15)), radius * (transform.localScale.x), ballColor);
        colormesh.GetComponent<colormesh>().color((int)(6 * (transform.position.x + 15)), (int)(6 * (transform.position.z + 15)), radius * (transform.localScale.x));
        oppocolor.GetComponent<colormesh>().remove((int)(6 * (transform.position.x + 15)), (int)(6 * (transform.position.z + 15)), radius * (transform.localScale.x));

        //Debug.Log("color" + ballColor.ToString() + " " + sizechange.ToString() + " " + (10 * (transform.position.x + 15)).ToString() + " " + (10 * (transform.position.z + 15)).ToString());
        //float velo = Mathf.Sqrt(rb.velocity.x * rb.velocity.x + rb.velocity.y * rb.velocity.y);
        //Debug.Log("color" + ballColor.ToString() + " " + sizechange.ToString());
        if (sizechange > 0)
        {
            if (transform.localScale.x > 0.3) transform.localScale = transform.localScale - Vector3.one * growthRate;
        }
        else if (sizechange < -0.95f)
        {
            if (transform.localScale.x < 3) transform.localScale = transform.localScale + Vector3.one * growthRate;
        }
    }

    public void PickUp(Transform playerTransform)
    {
        target = playerTransform;
        isPickedUp = true;

        // Check the player tag and update the ballScript reference in the corresponding PlayerController
        PlayerController playerController = playerTransform.GetComponent<PlayerController>();
        if (playerController != null)
        {
            if (playerTransform.tag == "Player1")
            {
                playerController.ballScript = this;
            }
            else if (playerTransform.tag == "Player2")
            {
                playerController.ballScript = this;
            }
        }
    }

    // To find if player is diable or not
    bool IsPlayerInScene(string playerTag)
    {
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        return player != null && player.activeInHierarchy;
    }

    public void ballShrink(int level)
    {
        transform.localScale /= level;
    }
}

