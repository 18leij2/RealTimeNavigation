using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTracker : MonoBehaviour
{
    public Transform copy; 
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.position = copy.position;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = copy.position;
    }
}
