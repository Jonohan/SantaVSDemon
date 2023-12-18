
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CharController : MonoBehaviour
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

    /*
     * Variable about if player snowballs
     */
    [SerializeField] private bool isPlayer1Snowball = false;
    [SerializeField] private bool isPlayer2Snowball = false;
    private float P1MSpeedAdj = 1.0f;
    private float P2MSpeedAdj = 1.0f;
    private float P1RSpeedAdj = 1.0f;
    private float P2RSpeedAdj = 1.0f;
    [SerializeField] private float moveAdjWhenSnowball = 0.6f; // You my adjust this index when snowball get bigger and bigger
    [SerializeField] private float rotateAdjWhenSnowball = 0.7f;

    // Start is called before the first frame update
    void Start()
    {
        basicmoveSpeed = moveSpeed;
        basicrotateSpeed = rotateSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        isPlayerSnowball();
        PlayerMove();
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
        else if (!isPlayer1Snowball)
        {
            P2MSpeedAdj = 1.0f;
            P2RSpeedAdj = 1.0f;
        }
    }


}
