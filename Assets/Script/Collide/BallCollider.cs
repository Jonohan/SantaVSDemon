using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollider : MonoBehaviour
{
    //CollisionAction(Snowball, object collide with snow ball)
    public delegate void CollisionAction(GameObject ball, GameObject Player, GameObject opponent, Collider other, bool isBallFree);
    public static event CollisionAction OnCollision;

    [Header("Player disable Variables")]
    public GameObject Player;
    public GameObject opponentPlayer;
    //[Header("[Drag the prefab from Hierachy]")]
    //public GameObject opponentBallPrefab;

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
                OnCollision(gameObject, Player, opponentPlayer, collision.collider, isBallFree);

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

                ballCollideWithWall();

                ballCollideWithBall(collision);

            }
        }  
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
            GetComponent<RollingBall>().color(10);
            Destroy(gameObject);
        }
    }

    public void ballCollideWithBall(Collision ball)
    {
        if (isBallFree) 
        {
            if (ball.gameObject != null && ball.gameObject.GetComponent<BallCollider>() != null)
            { 
                if (ball.gameObject.GetComponent<BallCollider>().isBallFree)
                {
                    Destroy(ball.gameObject);
                    Destroy(gameObject);
                } else
                {
                    GetComponent<RollingBall>().color(10);
                    Destroy(gameObject);
                    ball.gameObject.GetComponent<RollingBall>().ballShrink(2);
                }
            }
        }
    }
}
