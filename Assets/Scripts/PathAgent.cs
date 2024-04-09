using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

[RequireComponent(typeof(CameraSensor))]
[RequireComponent(typeof(DecisionRequester))]
public class PathAgent : Agent
{
    public CameraSensor cameraSensor;
    public float speed;
    public Vector3 start; 
    [Header("Goal")]
    public Transform goal;
    public float nearGoalThreshold = 3f;

    private List<Vector2> currentPath = new List<Vector2>();
    private Vector2 currentTarget;

    private bool isGeneratingPath = true;
    private bool isFollowingPath = false;

    private void Awake() {

    }

    public override void OnEpisodeBegin()
    {
        isGeneratingPath = true;
        isFollowingPath = false;
    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[1] = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;
        discreteActionsOut[0] = Input.GetKey(KeyCode.D) ? 1 : Input.GetKey(KeyCode.A) ? -1 : 0;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        if (currentPath.Count == 0)
        {
            sensor.AddObservation(Vector3.Distance(transform.position, goal.position));
        } else {
            sensor.AddObservation(Vector3.Distance(currentPath[currentPath.Count - 1], goal.position));
        }
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        if (isGeneratingPath)
        {
            if (currentPath.Count == 0)
            {
                currentPath.Add(new Vector2(start.x, start.z)); // Add start position to the path
            }


            Vector2 point = new Vector2(currentPath[currentPath.Count - 1].x + actionBuffers.DiscreteActions[0], currentPath[currentPath.Count - 1].y + actionBuffers.DiscreteActions[1]);
            currentPath.Add(point);
            transform.position = new Vector3(point.x, 0, point.y);

            // Check if the last point added is near the goal
            Debug.Log(Vector3.Distance(new Vector3(point.x, 0, point.y), goal.position) < nearGoalThreshold);
            if (Vector3.Distance(new Vector3(point.x, 0, point.y), goal.position) < nearGoalThreshold)
            {
                Debug.Log("dsadsa");
                isGeneratingPath = false;
                isFollowingPath = true;
                currentPath.Add(goal.position);
                transform.position = currentPath[0];
            }

            AddReward(-0.01f); // Penalty for not creating path fast enough
        }
    }

    private int currentWaypointIndex = 0;
    private void FollowPath()
    {
        if (currentWaypointIndex < currentPath.Count)
        {
            Vector3 currentWaypoint = new Vector3(currentPath[currentWaypointIndex].x, 0, currentPath[currentWaypointIndex].y);
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, currentWaypoint) < 0.1f)
            {
                currentWaypointIndex++;
            }
        } else {
            EndEpisode();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") && !isFollowingPath)
        {
            AddReward(-1f); // Punish for collision with obstacle
            //EndEpisode();
        }
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (currentPath.Count > 1)
        {
            for (int i = 0; i < currentPath.Count - 1; i++)
            {
                Vector3 startPoint = new Vector3(currentPath[i].x, 0, currentPath[i].y);
                Vector3 endPoint = new Vector3(currentPath[i + 1].x, 0, currentPath[i + 1].y);
                Gizmos.DrawLine(startPoint, endPoint);
            }
        }
    }
}