using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarFSim : MonoBehaviour
{
    public GameObject player;
    public GameObject target;
    private Vector3 playerlocation;
    private Vector3 targetlocation;
    private LineRenderer Astarline;
    public float obsradius = 0.5f;
    public float scale = 1f;
    public float timestepsize = 0.1f;
    private int astarstep;
    private List<Vector3> path = new List<Vector3>();
    private Pathfinding Astar;
    private Grid<PathNode> grid;
    // Start is called before the first frame update
    void Start()
    {
        Astarline = gameObject.AddComponent<LineRenderer>();
        Astarline.material.color = Color.yellow;
        Astarline.startWidth = 0.1f;
        Astarline.endWidth = 0.1f;
        Astar = new Pathfinding((int)(8 * (1/scale)), (int)(8 * (1/scale)), scale);

        grid = Astar.GetGrid();
        astarstep = Mathf.RoundToInt(timestepsize/scale);
        

        //target location
        targetlocation = target.transform.position;
        int tx, ty;
        grid.GetXY(targetlocation, out tx, out ty);

        //initial player location
        playerlocation = player.transform.position;
        path.Add(playerlocation);

        int timestep = 1;
        int steppointer = 1;

        // Debug.Log(astarstep);

        while ((grid.GetWorldPosition(tx, ty) - path[path.Count - 1]).magnitude > 0.5) {
            //get forward sim
            GameObject managerScriptObject = GameObject.Find("Floor");
            ForwardSimManager managerScript = managerScriptObject.GetComponent<ForwardSimManager>();
            

            //get projected obstacle positions
            // is working...
            managerScript.forwardProjectTime = timestep * timestepsize;
            List<Vector3> obstaclepos = managerScript.forwardProject(timestep * timestepsize);

            //DEBUG: PRINTS FORWARD SIMULATION OBJECT LOCATIONS
            // Debug.Log("Forward Project Time is: " + managerScript.forwardProjectTime);
            // Debug.Log("The projected obstacle positions are: ");
            // foreach (Vector3 g in obstaclepos) {
            //     Debug.Log(g);
            // }

            //set obstacles
            setObstacles(obstaclepos);
            //Debug.Log(timestep*timestepsize);
            if (Mathf.Abs((float)(timestep * timestepsize) - 0.1f) < 0.00001f) {
                Debug.Log("drawing square");
                foreach (Vector3 pos in obstaclepos) {
                    drawObst(pos);
                }
            }

            //astar path
            int px, py;
            grid.GetXY(playerlocation, out px, out py);
            List<PathNode> steppath = Astar.FindPath(px, py, tx, ty);

            //DEBUG: PRINTS INDIVIDUAL ASTAR PATH FOR EACH TIME STEP
            // Debug.Log("Astar");
            // foreach (PathNode d in steppath) {
            //     Debug.Log(grid.GetWorldPosition(d.x, d.y));
            // }

            //add to forwardsimed path
            //Need Changes For Optimization(not now lol)
            if (astarstep == 0) {
                if ((float)(timestepsize/scale) * steppointer > 1) {
                    for (int i = 0; i < Mathf.Min(Mathf.RoundToInt((float)(timestepsize/scale) * steppointer), steppath.Count-1); i++) {
                        PathNode node = steppath[i+1];
                        path.Add(grid.GetWorldPosition(node.x, node.y));
                    }
                    clearObstacles();
                    steppointer = 1;
                } else {
                    steppointer++;
                }
            } else {
                for (int i = 0; i < Mathf.Min(astarstep, steppath.Count-1); i++) {
                    PathNode node = steppath[i+1];
                    path.Add(grid.GetWorldPosition(node.x, node.y));
                }
                //clear obstacles
                setObstacles(obstaclepos, clear: true);
            }
            //set player position and next timestep
            timestep += 1;
            playerlocation = path[path.Count - 1];

            //DEBUG: PRINTS THE FORWARD SIMULATED PATH
            // Debug.Log("Forward Path");
            // foreach (Vector3 p in path) {
            //     Debug.Log(p);
            // }
            // Debug.Log(timestep);
            // Debug.Log(timestep * timestepsize);
        }

        // DEBUG: PRINTS THE FORWARD SIMULATED PATH
        // Debug.Log("Forward Path");
        // foreach (Vector3 p in path) {
        //     Debug.Log(p);
        // }
        Astarline.positionCount = path.Count;
        Astarline.SetPositions(path.ToArray());


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setObstacles(IEnumerable<Vector3> obstaclepos, bool clear = false) {
        int[,] signs = {{-1, 1}, {1, -1}, {-1, -1}, {1, 1}};
        foreach (Vector3 pos in obstaclepos) {
            PathNode node = grid.GetGridObject(pos);
            PathNode temp = grid.GetGridObject(pos + new Vector3(obsradius, 0, 0));
            for (int i = 0; i < obsradius; i++) {
                for (int j = 0; j < obsradius; j++) {
                    for (int k = 0; k < 4; k++) {
                        int x = node.x + i*signs[k, 0];
                        int y = node.y + j*signs[k, 1];
                        temp = grid.GetGridObject(x, y);
                        if (temp != null) {
                            temp.isWalkable = clear;
                        }
                    }
                    
                }
            }
        }
    }

    public void clearObstacles() {
        for (int i = 0; i < grid.GetWidth(); i++) {
            for (int j = 0; j < grid.GetHeight(); j++) {
                PathNode node = grid.GetGridObject(i, j);
                if (!node.isWalkable) {
                    node.isWalkable = true;
                }
            }
        } 
    }

    public void drawObst(Vector3 pos) {
        Gizmos.color = Color.red; // Set color to red
        Vector3 center = pos + new Vector3(0, 1, 0); // Get position of the GameObject

        // Calculate the corners of the square
        Vector3 topLeft = center + new Vector3(-obsradius, 0, obsradius);
        Vector3 topRight = center + new Vector3(obsradius, 0, obsradius);
        Vector3 bottomLeft = center + new Vector3(-obsradius, 0, -obsradius);
        Vector3 bottomRight = center + new Vector3(obsradius, 0, -obsradius);

        // Draw lines to form the square
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }
}
