using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(LineRenderer))]

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;
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
        if (Input.GetMouseButtonDown(0))
        {
            ClickToMove();
        }
        if (agent.hasPath)
        {
            DrawPath();
        }
    }
    private void ClickToMove()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray, out hit);
        if (hasHit)
        {
            SetDestination(hit.point);
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
        if(agent.path.corners.Length < 2) return;
        myLineRenderer.positionCount = agent.path.corners.Length; // checks the number of corners in the path
        myLineRenderer.SetPositions(agent.path.corners);

        // add each point to the line renderer
        for (int i = 0; i < agent.path.corners.Length; i++)
        {
            // we add stuff to y to make the line "hover" above the ground
            Vector3 pointPosition = new Vector3(agent.path.corners[i].x, agent.path.corners[i].y + 0.1f, agent.path.corners[i].z);
            myLineRenderer.SetPosition(i, pointPosition);
        }
    }
}
