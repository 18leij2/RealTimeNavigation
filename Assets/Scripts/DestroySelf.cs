using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    public float destroyDelay = 5.0f; // Delay in seconds

    void Start()
    {
        // Schedule the GameObject for destruction after the specified delay
        Destroy(gameObject, destroyDelay);
    }
}
