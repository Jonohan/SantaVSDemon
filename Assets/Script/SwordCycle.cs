using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SwordCycle : MonoBehaviour
{
    //public Transform initialPosition; 
    //public Transform groundPosition; 
    public float hoverHeight = 1f; 
    public float vibrationIntensity = 0.1f;
    public float vibrationDuration = 2f; 
    public float speed = 10f; 

    private Vector3 startPosition = new Vector3(0, 51.0f, 0);
    private Vector3 endPosition = new Vector3(0, -5.5f, 0);
    private bool isVibrating = false;

    public GameObject insertionEffectPrefab;

    private void Start()
    {
        //startPosition = initialPosition.position;
        //endPosition = groundPosition.position;
        StartCoroutine(SwordRoutine());
    }

    private IEnumerator SwordRoutine()
    {
        while (true) 
        {
            yield return MoveSword(startPosition, endPosition, speed);
            InstantiateInsertionEffect();
            yield return new WaitForSeconds(5f); 

            StartCoroutine(VibrateSword(vibrationDuration, vibrationIntensity));
            yield return new WaitForSeconds(vibrationDuration); 

            yield return MoveSword(endPosition, startPosition + new Vector3(0, hoverHeight, 0), speed);
            yield return new WaitForSeconds(5f); 
        }
    }

    private IEnumerator MoveSword(Vector3 from, Vector3 to, float speed)
    {
        float step = (speed / (from - to).magnitude) * Time.fixedDeltaTime;
        float t = 0;
        while (t <= 1.0f)
        {
            t += step; 
            transform.position = Vector3.Lerp(from, to, t); 
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator VibrateSword(float duration, float intensity)
    {
        isVibrating = true;
        float timer = 0;

        while (timer < duration)
        {
            transform.position += Random.insideUnitSphere * intensity; 
            timer += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;
        isVibrating = false;
    }

    private void InstantiateInsertionEffect()
    {
        Vector3 effectPosition = new Vector3(0, 0.5f, -1.0f);
        GameObject effectInstance = Instantiate(insertionEffectPrefab, effectPosition, Quaternion.identity);
        Destroy(effectInstance, 3.0f);
    }
}

