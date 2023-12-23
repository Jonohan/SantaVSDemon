using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollider : MonoBehaviour
{
    //CollisionAction(Snowball, object collide with snow ball)
    public delegate void CollisionAction(GameObject ball, Collider other);
    public static event CollisionAction OnCollision;

    [Header("Player disable Variables")]
    public GameObject Player;
    public GameObject opponentPlayer;
    public GameObject opponentBallPrefab;

    [Header("Explosion Variables")]
    public GameObject explosionPrefab;
    public Vector3 offset = new Vector3(0, 0, 0);

    private bool isBallFree = false;


    void Update()
    {
        //Debug.Log("Bool: " + isBallFree);
    }

    void OnCollisionEnter(Collision collision)
    {
        // isBallFree: Player push the ball, no one control this ball
        if (isBallFree)
        {
            if (collision.gameObject != Player && collision.gameObject.tag != "Ground")
            {
                OnCollision(gameObject, collision.collider);

                // Create explosion animation where the ball collide
                GameObject explosionInstance = Instantiate(explosionPrefab, transform.position + offset, Quaternion.identity);
                
                // Get size of the ball
                float ballScale = transform.localScale.x;
                //Debug.Log("Ball scale: " + ballScale);

                // Adjust explosion size
                ParticleSystem explosionParticles = explosionInstance.GetComponent<ParticleSystem>();
                var mainModule = explosionParticles.main;
                // Changer size index here
                mainModule.startSize = new ParticleSystem.MinMaxCurve(1.5f* ballScale); 

                Destroy(explosionInstance, 2f);
                //Debug.Log("Collide with others");

                if (collision.gameObject == opponentPlayer)
                {
                    StartCoroutine(HandleOpponentCollision(opponentPlayer));
                }
            }
        }
        
    }

    IEnumerator HandleOpponentCollision(GameObject opponent)
    {
        if (isBallFree)
        {
            // Get 2 player current position
            Vector3 playerPosition = Player.transform.position;
            Vector3 opponentPosition = opponent.transform.position;



            // Remove the player was collided with ball
            opponent.SetActive(false);

            // Create 2 ball of the dead player, on the line between this player and his opponent
            InstantiateBallAtPosition(Vector3.Lerp(playerPosition, opponentPosition, Random.Range(0.25f, 0.75f)));
            InstantiateBallAtPosition(Vector3.Lerp(playerPosition, opponentPosition, Random.Range(0.25f, 0.75f)));


            // Time interval of return
            yield return new WaitForSeconds(5f);

            opponent.SetActive(true);
            //Get opponent function
            PlayerController playerController = opponent.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.StartInvincibilityFlash();
            }
        }
    }

    // Create a new ball
    void InstantiateBallAtPosition(Vector3 position)
    {
        Instantiate(opponentBallPrefab, position, Quaternion.identity);
    }

    // Set ball free
    public void SetBallFree(bool isFree)
    {
        isBallFree = isFree;
    }

    public void ballCollideWithWall()
    {
        if (isBallFree)
        {
            GetComponent<RollingBall>().color(5);
            gameObject.SetActive(false);
        }
    }

    public void ballCollideWithBall(Collision ball)
    {
        if (isBallFree) 
        {
            if (ball.gameObject.GetComponent<BallCollider>().isBallFree)
            {
                ball.gameObject.SetActive(false);
                gameObject.SetActive(false);
            } else
            {
                GetComponent<RollingBall>().color(5);
                gameObject.SetActive(false);
                ball.gameObject.GetComponent<RollingBall>().ballShrink(2);
            }
        }
    }
}
