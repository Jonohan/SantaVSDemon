using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollider : MonoBehaviour
{
    //CollisionAction(Snowball, object collide with snow ball)
    public delegate void CollisionAction(GameObject ball, Collider other);
    public static event CollisionAction OnCollision;
    public string PlayerTag;

    public GameObject explosionPrefab;
    public Vector3 offset = new Vector3(0, 0, 0);

    public bool isBallFree = true;


    void OnCollisionEnter(Collision collision)
    {
        if (isBallFree)
        {
            if (collision.gameObject.tag != PlayerTag && collision.gameObject.tag != "Ground")
            {
                OnCollision(gameObject, collision.collider);

                // Create explosion animation where the ball collide
                GameObject explosionInstance = Instantiate(explosionPrefab, transform.position + offset, Quaternion.identity);
                
                // Get size of the ball
                float ballScale = transform.localScale.x;
                Debug.Log("Ball scale: " + ballScale);

                // Adjust explosion size
                ParticleSystem explosionParticles = explosionInstance.GetComponent<ParticleSystem>();
                var mainModule = explosionParticles.main;
                // Changer size index here
                mainModule.startSize = new ParticleSystem.MinMaxCurve(1.5f* ballScale); 

                Destroy(explosionInstance, 2f);
                //Debug.Log("Collide with others");
            }
        }
        
    }
}
