using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleMovement : MonoBehaviour
{
    [SerializeField]
    float speed = 3.0f;

    private Vector3 moveDirection;
    private Rigidbody rb;
    private bool isPlayerStopped = false;

    // at random intervals, randomly change directions
    private float timer = 0f;
    private float interval;
    private float minInterval = 1000f; // Minimum interval (in seconds).
    private float maxInterval = 10000f; // Maximum interval (in seconds).

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Ensure the Rigidbody is set to Kinematic in the Inspector.
        rb.isKinematic = true;
    }

    void FixedUpdate()
    {
        MoveCharacter();
    }
    void MoveCharacter()
    {
        // Check for obstacles (e.g., vehicles) in front of the NPC.
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 8.0f)) // Adjust the raycast length as needed.
        {
            Debug.DrawRay(transform.position, transform.forward * 2.0f, Color.red);
            if (hit.collider.CompareTag("AutonomousVehicle") || hit.collider.gameObject.layer == LayerMask.NameToLayer("People"))
            {
                // A vehicle is in front; stop the player movement.
                isPlayerStopped = true;
                Debug.Log("Raycast hit, stoping: " + hit.collider.gameObject.name + ", Tag: " + hit.collider.tag + ", Layer: " + LayerMask.LayerToName(hit.collider.gameObject.layer));
            }
            else
            {
                // No vehicle detected; continue moving.
                isPlayerStopped = false;
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
        }
        else
        {
            isPlayerStopped = false;
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

    }

    void Flip()
    {
        moveDirection *= -1;
    }
    void ChangeDirection()
    {
        // Randomly choose a new direction.
        float randomAngle = Random.Range(0, 360); // Change direction in a 360-degree range.
        moveDirection = Quaternion.Euler(0, randomAngle, 0) * transform.forward * -1;
        // Delay the next direction change.
        float changeDelay = Random.Range(1.0f, 3.0f); // Adjust as needed.
        Invoke("ResetDirectionChangeFlag", changeDelay);
    }
}