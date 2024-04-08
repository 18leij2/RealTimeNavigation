using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

public class ForwardSimManager : MonoBehaviour
{
    public static ForwardSimManager instance;
    public List<GameObject> obstacleList = new List<GameObject>(); // list of soon-to-be implemented obstacles in range of step
    public List<List<Vector3>> positionList = new List<List<Vector3>>();
    public List<Vector3> projectedList = new List<Vector3>(); // list of all the projected positions
    public float forwardProjectTime;
    public List<GameObject> obstacles = new List<GameObject>(); // list of ALL obstacles

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
            
    }
    // Start is called before the first frame update
    void Start()
    {
        // obstacles = GameObject.FindGameObjectsWithTag("Moving");
        foreach (GameObject obj in obstacles)
        {
            // later do an if check for within step-range
            obstacleList.Add(obj);
        }
        //Debug.Log(obstacleList.Count);
        forwardProject();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Vector3> forwardProject()
    {
        positionList.Clear();
        foreach (GameObject obj in obstacles) // change to obstacleList later
        {
            SimpleMovement otherScript = obj.GetComponent<SimpleMovement>();
            positionList.Add(otherScript.simulate()); // adds a list of 2 vectors: index 0 is the current position, index 1 is the projected position
        }

        projectedList.Clear();

        // Print the contents of the list of lists of vectors
        //Debug.Log("Contents of the list of lists of vectors:");
        for (int i = 0; i < positionList.Count; i++)
        {
            //Debug.Log("Inner List " + i + ":");
            // Iterate through the elements of the inner list
            for (int j = 0; j < positionList[i].Count; j++)
            {
                // Print each vector in the inner list
                //Debug.Log("Vector " + j + ": " + positionList[i][j]);
                if (j == 1)
                {
                    projectedList.Add(positionList[i][j]);
                }               
            }
        }

        // for (int i = 0; i < projectedList.Count; i++)
        // {
        //     Debug.Log("it's " + projectedList[i]);
        // }

        return projectedList;
    }

    public void AddObject(GameObject obj)
    {
        obstacles.Add(obj);
    }
}
