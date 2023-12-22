using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderManager : MonoBehaviour
{
    void OnEnable()
    {
        BallCollider.OnCollision += HandleCollision;
    }

    void OnDisable()
    {
        BallCollider.OnCollision -= HandleCollision;
    }

    private void HandleCollision(GameObject ball, Collider other)
    {
        Debug.Log($"Collision detected with {other.gameObject.name}");
    }
}

