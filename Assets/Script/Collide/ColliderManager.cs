using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderManager : MonoBehaviour
{
    [Header("[Drag the prefab from Hierachy]")]
    public GameObject snowballPrefab; 
    public GameObject lavaballPrefab;

    [Header("Explosion Variables")]
    public GameObject playerRevivePrefab;
    private Vector3 reviveOffset = new Vector3(0, -10, 0);
    private Vector3 explosionOffset = new Vector3(0, 0, 0);

    void OnEnable()
    {
        BallCollider.OnCollision += HandleCollision;
    }

    void OnDisable()
    {
        BallCollider.OnCollision -= HandleCollision;
    }

    private void HandleCollision(GameObject ball, GameObject Player, GameObject opponent, Collider other, bool isBallFree)
    {
        Debug.Log($"Collision detected between {ball.name} and {other.gameObject.name}");

        if (other.gameObject == opponent)
        {
            StartCoroutine(HandleOpponentCollision(ball, Player, opponent , isBallFree));
        }
    }


    IEnumerator HandleOpponentCollision(GameObject ball, GameObject player, GameObject opponent , bool ifBallFree)
    {

        if (ifBallFree)
        {
            PlayerController playerController = opponent.GetComponent<PlayerController>();
            /*            // Get 2 player current position
                        Vector3 playerPosition = player.transform.position;
                        Vector3 opponentPosition = opponent.transform.position;*/

            if (!playerController.isInvincible) 
            {
                // Remove the player was collided with ball
                opponent.SetActive(false);
                Debug.Log("Disabling opponent: " + opponent.name);

                /*            // Create 2 ball of the dead player, on the line between this player and his opponent
                            GameObject opponentPrefab = DetermineOpponentPrefab(ball);
                            InstantiateBallAtPosition(Vector3.Lerp(playerPosition, opponentPosition, Random.Range(0.3f, 0.8f)), opponentPrefab);*/

                // Time interval of return
                yield return new WaitForSeconds(5f);


                opponent.SetActive(true);
                //Get opponent function
                //PlayerController playerController = opponent.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.StartInvincibilityFlash();
                    Instantiate(playerRevivePrefab, opponent.transform.position + explosionOffset, Quaternion.identity);

                    // Color the ground (Create new ball and destroy)
                    GameObject ballToInstantiate = null;
                    if (opponent.tag == "Player1")
                    {
                        ballToInstantiate = snowballPrefab;
                    }
                    else if (opponent.tag == "Player2")
                    {
                        ballToInstantiate = lavaballPrefab;
                    }

                    if (ballToInstantiate != null)
                    {
                        GameObject instantiatedBall = Instantiate(ballToInstantiate, opponent.transform.position + reviveOffset, Quaternion.identity);
                        instantiatedBall.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                        Rigidbody rb = instantiatedBall.GetComponent<Rigidbody>();
                        rb.constraints = RigidbodyConstraints.FreezePositionY;

                        RollingBall rollingBallScript = instantiatedBall.GetComponent<RollingBall>();
                        rollingBallScript.color(15);

                        Destroy(instantiatedBall, 0.1f);
                    }
                }
            }
      
        }
    }

/*    // Create a new ball
    void InstantiateBallAtPosition(Vector3 position, GameObject prefab)
    {
        GameObject newBall =  Instantiate(prefab, position, Quaternion.identity);

        
        RollingBall rollingBallScript = newBall.GetComponent<RollingBall>();
        // Ball is not picked
        bool isBallPicked = false;
        if (!isBallPicked)
        {
            if (rollingBallScript != null)
            {
                // Make new ball don't follow any player
                rollingBallScript.target = null;
                // Scale is 1
                rollingBallScript.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

                // Set ball color based on prefab
                if (prefab == snowballPrefab)
                {
                    rollingBallScript.ballColor = 1; // Snowball color
                }
                else if (prefab == lavaballPrefab)
                {
                    rollingBallScript.ballColor = 2; // Lavaball color
                }
            }

            Rigidbody rb = newBall.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Cannot move until player picks up it
                rb.isKinematic = !rollingBallScript.isPickedUp;
            }
        }      
    }*/

    // Find each ball's opponent
    GameObject DetermineOpponentPrefab(GameObject ball)
    {
        if (ball.CompareTag("SnowBall"))
        {
            return lavaballPrefab;
        }
        else if (ball.CompareTag("LavaBall"))
        {
            return snowballPrefab;
        }
        Debug.LogError("Tag: " + ball.tag + " is not defined.");
        return null;
    }
}

