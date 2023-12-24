using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement Speed")]
    public float moveSpeed = 5.0f;
    public float rotateSpeed = 250.0f;
    public float currentSpeed = 0.0f;

    private float basicmoveSpeed;
    private float basicrotateSpeed;

    [Header("Private Variables")]
    [SerializeField] private float acceleration = 3.0f;
    [SerializeField] private float deceleration = 3.0f;
    private float rotationSpeedIndex = 0.2f; // if you move slower, you rotate slower

    [Header("Snowball Status Variables")]
    /*
     * Variable about if player snowballs
     */
    private bool isPlayer1Snowball = true;
    private bool isPlayer2Snowball = true;
    private float P1MSpeedAdj = 1.0f;
    private float P2MSpeedAdj = 1.0f;
    private float P1RSpeedAdj = 1.0f;
    private float P2RSpeedAdj = 1.0f;
    [SerializeField] private float moveAdjWhenSnowball = 0.6f; // You my adjust this index when snowball get bigger and bigger
    [SerializeField] private float rotateAdjWhenSnowball = 0.7f;

    public RollingBall ballScript;

    [Header("Player disable Variables")]
    public bool isInvincible = false;

    [Header("Drag prefab from ball pool")]
    public GameObject ballPrefab;

    public bool active;



    // Start is called before the first frame update
    void Start()
    {
        basicmoveSpeed = moveSpeed;
        basicrotateSpeed = rotateSpeed;
        active = true;
    }

    // Update is called once per frame
    void Update()
    {
        isPlayerSnowball();
        if (active)
        {
            PlayerMove();
        }
        pLayerReleaseBall();
        CheckBallStatus();
    }


    void PlayerMove()
    {
        KeyCode forwardKey = KeyCode.W;
        KeyCode leftKey = KeyCode.A;
        KeyCode rightKey = KeyCode.D;

        // 2 P Movement key
        if (gameObject.tag == "Player1")
        {
            forwardKey = KeyCode.W;
            leftKey = KeyCode.A;
            rightKey = KeyCode.D;
            moveSpeed = basicmoveSpeed * P1MSpeedAdj;
            rotateSpeed = basicrotateSpeed * P1RSpeedAdj;
        }
        else if (gameObject.tag == "Player2")
        {
            forwardKey = KeyCode.UpArrow;
            leftKey = KeyCode.LeftArrow;
            rightKey = KeyCode.RightArrow;
            moveSpeed = basicmoveSpeed * P2MSpeedAdj;
            rotateSpeed = basicrotateSpeed * P2RSpeedAdj;
        }

        bool isMovingForward = Input.GetKey(forwardKey);

        // Press W to acceleration till max speend
        if (isMovingForward)
        {
            currentSpeed += acceleration * Time.deltaTime;
            if (currentSpeed > moveSpeed) currentSpeed = moveSpeed;
        }
        else // deceleration
        {
            currentSpeed -= deceleration * Time.deltaTime;
            if (currentSpeed < 0) currentSpeed = 0;
        }


        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);

        // Player can only turn left or right when current speed > 0
        if (currentSpeed > 0)
        {
            if (Input.GetKey(leftKey))
            {
                transform.Rotate(Vector3.up, -rotateSpeed * rotationSpeedIndex * currentSpeed * Time.deltaTime);
            }
            else if (Input.GetKey(rightKey))
            {
                transform.Rotate(Vector3.up, rotateSpeed * rotationSpeedIndex * currentSpeed * Time.deltaTime);
            }
        }
    }

    void isPlayerSnowball()
    {
        if (isPlayer1Snowball)
        {
            P1MSpeedAdj = moveAdjWhenSnowball;
            P1RSpeedAdj = rotateAdjWhenSnowball;
        }
        else if (!isPlayer1Snowball)
        {
            P1MSpeedAdj = 1.0f;
            P1RSpeedAdj = 1.0f;
        }

        if (isPlayer2Snowball)
        {
            P2MSpeedAdj = moveAdjWhenSnowball;
            P2RSpeedAdj = rotateAdjWhenSnowball;
        }
        else if (!isPlayer2Snowball)
        {
            P2MSpeedAdj = 1.0f;
            P2RSpeedAdj = 1.0f;
        }
    }

    

    // Make player invincible when player return
    IEnumerator InvincibilityFlash()
    {
        isInvincible = true;

        // Reset snowball status when player returns
        if (gameObject.tag == "Player1")
        {
            isPlayer1Snowball = false;
        }
        else if (gameObject.tag == "Player2")
        {
            isPlayer2Snowball = false;
        }

        // Make player object flash
        Renderer playerRenderer = GetComponent<Renderer>();

        float duration = 3f;
        float flashDuration = 0.1f;
        while (duration > 0f)
        {
            playerRenderer.enabled = !playerRenderer.enabled;
            duration -= flashDuration;
            yield return new WaitForSeconds(flashDuration);
        }

        // Reset
        playerRenderer.enabled = true;
        isInvincible = false;
    }

    public void StartInvincibilityFlash()
    {
        StartCoroutine(InvincibilityFlash());
    }

    // When Player enter the collider of ball, pick up the ball
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SnowBall") && !isPlayer1Snowball)
        {
            other.gameObject.GetComponent<RollingBall>()?.PickUp(transform);
            isPlayer1Snowball = true;
        }
        else if (other.gameObject.CompareTag("LavaBall") && !isPlayer2Snowball)
        {
            other.gameObject.GetComponent<RollingBall>()?.PickUp(transform);
            isPlayer2Snowball = true;
        }
    }

    //P1 press E, P2 press right controll to release ball (Only when player has ball)
    public void pLayerReleaseBall()
    {
        if (gameObject.tag == "Player1" && Input.GetKeyDown(KeyCode.E))
        {
            if (isPlayer1Snowball && ballScript != null)
            {
                // Release the ball if the player has one
                ballScript.ReleaseBall();
                isPlayer1Snowball = false;
                ballScript = null;
                Debug.Log("isPlayer1Snowball " + isPlayer1Snowball);
            }
            else if (!isPlayer1Snowball)
            {
                GenerateNewBall();
                isPlayer1Snowball = true;
                Debug.Log("isPlayer1Snowball " + isPlayer1Snowball);
            }
        }

        if (gameObject.tag == "Player2" && Input.GetKeyDown(KeyCode.RightControl))
        {

            if (isPlayer2Snowball && ballScript != null)
            {
                // Release the ball if the player has one
                ballScript.ReleaseBall();
                isPlayer2Snowball = false;
                ballScript = null;
                Debug.Log("isPlayer2Snowball " + isPlayer2Snowball);
            }
            else if (!isPlayer2Snowball)
            {
                GenerateNewBall();
                isPlayer2Snowball = true;
                Debug.Log("isPlayer2Snowball " + isPlayer2Snowball);
            }
        }
    }

    private void GenerateNewBall()
    {
        GameObject newBall = Instantiate(ballPrefab, transform.position, Quaternion.identity);
        //newBall.transform.localScale = Vector3.one;
        RollingBall newBallScript = newBall.GetComponent<RollingBall>();
        if (newBallScript != null)
        {
            Rigidbody rb = newBall.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }

            // Set ball color of mesh
            if (gameObject.tag == "Player1")
            {
                newBallScript.ballColor = 1;
            }
            else if (gameObject.tag == "Player2")
            {
                newBallScript.ballColor = 2;
            }

            newBallScript.PickUp(transform);
            ballScript = newBallScript;
        }
    }

    private void CheckBallStatus()
    {
        if ((gameObject.tag == "Player1" && (ballScript == null || !ballScript.isPickedUp)) ||
                (gameObject.tag == "Player2" && (ballScript == null || !ballScript.isPickedUp)))
        {
            if (gameObject.tag == "Player1")
            {
                isPlayer1Snowball = false;
            }
            else if (gameObject.tag == "Player2")
            {
                isPlayer2Snowball = false;
            }
            ballScript = null;
        }
    }
}

