using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class ForwardSimManager : MonoBehaviour
{
    public static ForwardSimManager instance;
    public List<GameObject> obstacleList = new List<GameObject>();
    public List<List<Vector3>> positionList = new List<List<Vector3>>();
    public float forwardProjectTime;
    public GameObject[] obstacles = new GameObject[0];

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
        obstacles = GameObject.FindGameObjectsWithTag("Moving");
        foreach (GameObject obj in obstacles)
        {
            // later do an if check for within step-range
            AddObject(obj);
        }

        forwardProject();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void forwardProject()
    {
        foreach (GameObject obj in obstacleList)
        {
            SimpleMovement otherScript = obj.GetComponent<SimpleMovement>();
            positionList.Add(otherScript.simulate()); // adds a list of 2 vectors: index 0 is the current position, index 1 is the projected position
        }

        // Print the contents of the list of lists of vectors
        Debug.Log("Contents of the list of lists of vectors:");
        for (int i = 0; i < positionList.Count; i++)
        {
            Debug.Log("Inner List " + i + ":");
            // Iterate through the elements of the inner list
            for (int j = 0; j < positionList[i].Count; j++)
            {
                // Print each vector in the inner list
                Debug.Log("Vector " + j + ": " + positionList[i][j]);
            }
        }
    }

    public void AddObject(GameObject obj)
    {
        obstacleList.Add(obj);
    }

    public void RemoveObject(GameObject obj)
    {
        obstacleList.Remove(obj);
    }
}
