using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(LineRenderer))]

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;
    private NavMeshPath path;
    private LineRenderer myLineRenderer;
    [SerializeField] private GameObject clickMarkerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        myLineRenderer = GetComponent<LineRenderer>();
        myLineRenderer.startWidth = 0.15f;
        myLineRenderer.endWidth = 0.15f;
        myLineRenderer.positionCount = 0;  // initially no line
    }

    // Update is called once per frame
    void Update()
    {
        path = new NavMeshPath();
        // first arg is target
        if (agent.CalculatePath(clickMarkerPrefab.transform.position, path))
        {
            // Path calculation was successful.
            // The 'path.corners' array now contains the points of the path.
            DrawPath();
        }
    }
    private void SetDestination(Vector3 target)
    {
        clickMarkerPrefab.SetActive(true);
        clickMarkerPrefab.transform.SetParent(transform.parent, false); // set parent to the same parent as the player (to avoid scaling issues)
        clickMarkerPrefab.transform.position = target;
        agent.SetDestination(target);

    }
    // Draw a path from the player to the destination (w Navmesh)
    void DrawPath(){
        // if straight line, no need to draw path
        if(path.corners.Length < 2) return;
        myLineRenderer.positionCount = path.corners.Length; // checks the number of corners in the path
        myLineRenderer.SetPositions(path.corners);

        // add each point to the line renderer
        for (int i = 0; i < path.corners.Length; i++)
        {
            // we add stuff to y to make the line "hover" above the ground
            Vector3 pointPosition = new Vector3(path.corners[i].x, path.corners[i].y + 0.1f, path.corners[i].z);
            myLineRenderer.SetPosition(i, pointPosition);
        }
    }
}
