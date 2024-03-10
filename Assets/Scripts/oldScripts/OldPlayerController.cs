using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(LineRenderer))]

public class OldPlayerController : MonoBehaviour
{
    private NavMeshAgent agent;
    private NavMeshPath path;

    private LineRenderer myLineRenderer;
    //private bool isDrawing = false;
    [SerializeField] private GameObject clickMarkerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        path = new NavMeshPath();
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
            //isDrawing = true;
        }
        while(clickMarkerPrefab.activeSelf){
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
        
        // this actually moves the agent to goal(if u want this, then for every path.corners, its agent.path.corners)
        //agent.SetDestination(target);

    }
    // Draw a path from the player to the destination (w Navmesh)
    void DrawPath(){
        // if straight line, no need to draw path
        if(path.corners.Length < 2){
            Debug.Log("no path to draw");
            return;
        } 
        myLineRenderer.positionCount = path.corners.Length; // checks the number of corners in the path
        myLineRenderer.SetPositions(path.corners);
        Debug.Log(path.corners.Length);

        // add each point to the line renderer
        for (int i = 0; i < path.corners.Length; i++)
        {
            // we add stuff to y to make the line "hover" above the ground
            Vector3 pointPosition = new Vector3(path.corners[i].x, path.corners[i].y + 0.1f, path.corners[i].z);
            myLineRenderer.SetPosition(i, pointPosition);
        }
    }
}
