using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class ForwardSimManager : MonoBehaviour
{
    public static ForwardSimManager instance;
    public List<GameObject> obstacleList = new List<GameObject>();
    public float forwardProjectTime;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
