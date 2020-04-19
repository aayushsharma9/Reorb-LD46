using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float magnitude;
    [SerializeField] private float duration;
    [SerializeField] private float roughness;
    private Vector3 originalPosition;
    private float time;
    private bool shakeCalled;

    void Start()
    {
        originalPosition = transform.position;
        time = 0;
        shakeCalled = false;
    }

    void OnEnable() {
        
    }

    void Update()
    {
        time += Time.deltaTime;
        Coroutine shakeCoroutine = StartCoroutine(Shake());

        if (time > duration)
        {
            // transform.position = Vector3.Lerp(transform.position, originalPosition, 10);
            StopCoroutine(shakeCoroutine);
            StartCoroutine(Steady());
            time = 0;
            this.enabled = false;
        }
    }

    IEnumerator Shake()
    {
        float x = Random.Range(-magnitude, magnitude);
        float y = Random.Range(-magnitude, magnitude);
        float increment = 0;

        Vector3 newPosition = new Vector3(x, y, originalPosition.z);

        while (transform.position != newPosition)
        {
            transform.position = Vector3.Lerp(transform.position, newPosition, increment);
            increment += Time.deltaTime * roughness;
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator Steady()
    {
        float increment = 0;

        while (transform.position != originalPosition)
        {
            transform.position = Vector3.Lerp(transform.position, originalPosition, increment);
            increment += Time.deltaTime * roughness;
            yield return new WaitForEndOfFrame();
        }
    }
}