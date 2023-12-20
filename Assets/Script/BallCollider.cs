using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollider : MonoBehaviour
{
    //CollisionAction(Snowball, object collide with snow ball)
    public delegate void CollisionAction(GameObject ball, Collider other);
    public static event CollisionAction OnCollision;
    public string PlayerTag;

    void OnCollisionEnter(Collision collision)
    {
        if (OnCollision != null && collision.gameObject.tag != PlayerTag)
        {
            OnCollision(gameObject, collision.collider);
            Debug.Log("Collide with others");
        }
    }
}
