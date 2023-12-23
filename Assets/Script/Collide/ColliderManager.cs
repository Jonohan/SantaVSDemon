using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderManager : MonoBehaviour
{
    [Header("[Drag the prefab from Hierachy]")]
    public GameObject snowballPrefab; 
    public GameObject lavaballPrefab;

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
            // Get 2 player current position
            Vector3 playerPosition = player.transform.position;
            Vector3 opponentPosition = opponent.transform.position;



            // Remove the player was collided with ball
            opponent.SetActive(false);
            Debug.Log("Disabling opponent: " + opponent.name);

            // Create 2 ball of the dead player, on the line between this player and his opponent
            GameObject opponentPrefab = DetermineOpponentPrefab(ball);
            InstantiateBallAtPosition(Vector3.Lerp(playerPosition, opponentPosition, Random.Range(0.1f, 0.9f)), opponentPrefab);
            


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
                rollingBallScript.transform.localScale = Vector3.one;
            }

            Rigidbody rb = newBall.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Cannot move until player picks up it
                rb.isKinematic = !rollingBallScript.isPickedUp;
            }
        }      
    }

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

