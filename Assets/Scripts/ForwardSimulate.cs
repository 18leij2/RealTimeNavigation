using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardSimulate : MonoBehaviour
{
    public GameObject[] movingObjects;
    // Start is called before the first frame update
    void Start()
    {
        movingObjects = GameObject.FindGameObjectsWithTag("Moving");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
