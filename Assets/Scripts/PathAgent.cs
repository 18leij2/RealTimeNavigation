using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class PathAgent : Agent
{
    public Camera sensorCamera;
    public float speed;
    public Transform goal;

    private List<Vector2> currentPath = new List<Vector2>();
    private Vector2 currentTarget;

    private bool isGeneratingPath = true;

    public override void CollectObservations(VectorSensor sensor)
    {
        // Capture observations from the camera sensor
        // Add relevant information to the sensor for the agent to use
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        if (isGeneratingPath)
        {
            // Process the vectorAction to create a path for the agent to follow
            // Move the agent along the created path with the defined speed

            // Example: Assume vectorAction contains x, y coordinates for the path
            for (int i = 0; i < vectorAction.Length; i += 2)
            {
                Vector2 point = new Vector2(vectorAction[i], vectorAction[i + 1]);
                currentPath.Add(point);
            }

            if (currentPath.Count > 0)
            {
                currentTarget = new Vector3(currentPath[0].x, 0, currentPath[0].y);
                transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);

                if (Vector3.Distance(transform.position, currentTarget) < 0.1f)
                {
                    currentPath.RemoveAt(0);
                    isGeneratingPath = false;
                }
            }

            AddReward(-0.001f); // Penalize agent for each step
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Punish the agent when obstacles intersect the path
        AddReward(-1f); // Punish for collision
        EndEpisode();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Reward the agent for reaching the goal with a shorter path
        if (other.transform == goal)
        {
            float reward = 1f / currentPath.Count; // Reward for reaching the goal with shorter path
            AddReward(reward);
            EndEpisode();
        }
    }
}