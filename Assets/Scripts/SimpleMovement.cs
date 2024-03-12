using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
public class SimpleMovement : MonoBehaviour
{
    // targetTime must be at least 2x larger than forwardTime
    public float speed = 5f;
    public Vector3 direction = new Vector3(0, 0, 1);
    public bool oscillate = false;
    public float targetTime = 5f;
    private float obstacleTime;
    public float forwardTime = 0.5f;
    private float forwardCount;
    public Dictionary<string, object> forwardSim = new Dictionary<string, object>();

    // Start is called before the first frame update
    void Start()
    {
        obstacleTime = targetTime;
        forwardCount = forwardTime;
        forwardSim["Position"] = new List<Vector3>();
        forwardSim["Vector"] = new List<Vector3>();
        forwardSim["Frequency"] = obstacleTime;
        forwardSim["Velocity"] = speed;

        simulate();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
        
        if (oscillate)
        {
            obstacleTime -= Time.deltaTime;
            if (obstacleTime <= 0.0f)
            {
                direction = -direction;
                obstacleTime = targetTime;
            }
        }

        // Forward simulation
        forwardCount -= Time.deltaTime;
        if (forwardCount <= 0.0f)
        {
            simulate();
            forwardCount = forwardTime;
            Debug.Log("Future positions: " + ((List<Vector3>)forwardSim["Position"])[0] + " || " + ((List<Vector3>)forwardSim["Position"])[1] + " || " +
                ((List<Vector3>)forwardSim["Position"])[2] + " || " + ((List<Vector3>)forwardSim["Position"])[3] + " || " + ((List<Vector3>)forwardSim["Position"])[4]);
        }
    }

    private void simulate()
    {
        forwardSim["Position"] = new List<Vector3>();
        forwardSim["Vector"] = new List<Vector3>();
        for (int i = 0; i < 5; i++)
        {
            // Cast the value to List<Vector3> before adding elements
            List<Vector3> positions = (List<Vector3>)forwardSim["Position"];
            List<Vector3> directions = (List<Vector3>)forwardSim["Vector"];

            // Calculate new position
            if (i == 0)
            {
                Vector3 newPosition = gameObject.transform.position;
                Vector3 newDirection = direction;
                positions.Add(newPosition);
                directions.Add(newDirection);
            }
            else if (obstacleTime - (i * forwardTime) == 0)
            {
                Vector3 newPosition = gameObject.transform.position + (direction * speed * forwardTime * (i - 1));
                Vector3 newDirection = -direction;
                positions.Add(newPosition);
                directions.Add(newDirection);
            }
            else if (obstacleTime - (i * forwardTime) > 0)
            {
                Vector3 newPosition = gameObject.transform.position + (direction * speed * forwardTime * i);
                Vector3 newDirection = direction;
                positions.Add(newPosition);
                directions.Add(newDirection);
            }
            else
            {
                // Turning point
                Vector3 newPosition = gameObject.transform.position + (direction * speed * forwardTime * (i - 1)) + (-direction * speed * forwardTime);
                Vector3 newDirection = -direction;
                positions.Add(newPosition);
                directions.Add(newDirection);
            }

            // Update the value in the dictionary
            forwardSim["Position"] = positions;
            forwardSim["Vector"] = directions;
        }
    }
}
