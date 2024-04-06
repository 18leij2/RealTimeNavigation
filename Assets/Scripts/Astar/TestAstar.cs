using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAstar : MonoBehaviour
{
    private Pathfinding pathfinding;
    public Vector3[] obstaclepos;
    public GameObject player;
    public GameObject target;
    private Vector3 playerlocation;
    private Vector3 targetlocation;
    private LineRenderer Astarline;
    public float obsradius = 0.5f;
    public float scale = 1f;
    // Start is called before the first frame update
    void Start()
    {
        Astarline = gameObject.AddComponent<LineRenderer>();
        Astarline.material.color = Color.cyan;
        Astarline.startWidth = 0.1f;
        Astarline.endWidth = 0.1f;

        //set obstacles
        pathfinding = new Pathfinding((int)(8 * (1/scale)), (int)(8 * (1/scale)), 1f*scale);
        setObstacles(obstaclepos);
    }

    // Update is called once per frame
    void Update()
    {
        playerlocation = player.transform.position;
        targetlocation = target.transform.position;
        int px, py;
        pathfinding.GetGrid().GetXY(playerlocation, out px, out py);
        int tx, ty;
        pathfinding.GetGrid().GetXY(targetlocation, out tx, out ty);
        List<PathNode> path = pathfinding.FindPath(px, py, tx, ty);

        

        LineRenderer Astarline = GetComponent<LineRenderer>();
        Astarline.positionCount = path.Count;
        Vector3[] points = new Vector3[path.Count];
        for (int i = 0; i < path.Count; i++) {
            points[i] = pathfinding.GetGrid().GetWorldPosition(path[i].x, path[i].y);
        }
        //Debug.Log(string.Join(", ", points));
        //Astarline.startWidth *= points.Length;
        Astarline.SetPositions(points);
    }

    public void setObstacles(Vector3[] obstaclepos) {
        int[] negpos = new int[2] {-1, 1};
        foreach (Vector3 pos in obstaclepos) {
            Grid<PathNode> grid = pathfinding.GetGrid();
            PathNode node = grid.GetGridObject(pos);
            PathNode temp = grid.GetGridObject(pos + new Vector3(obsradius, 0, 0));
            if (temp == null) {
                temp = grid.GetGridObject(pos - new Vector3(obsradius, 0, 0));
            }
            int nodeoffset = Mathf.Abs(temp.x - node.x);
            for (int i = 0; i < nodeoffset; i++) {
                for (int j = 0; j < nodeoffset; j++) {
                    foreach (int sign in negpos) {
                        int x = node.x + i*sign;
                        int y = node.y + j*sign;
                        temp = grid.GetGridObject(x, y);
                        if (temp != null) {
                            temp.isWalkable = false;
                        }
                    }
                    
                }
            }
            //Debug.Log(pathfinding.GetGrid().GetGridObject(pos).isWalkable);
        }
    }

    public List<Vector3> addBuffer(Vector3[] obstaclepos) {
        List<Vector3> obsbuffpos = new List<Vector3>();
        Vector3 x = new Vector3(0.5f, 1, 0);
        Vector3 z = new Vector3(0, 1, 0.5f);
        foreach (Vector3 pos in obstaclepos) {
            obsbuffpos.Add(pos);
            obsbuffpos.Add(pos + x);

        }
        return obsbuffpos;
    }
}
