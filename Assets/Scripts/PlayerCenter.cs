using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCenter : MonoBehaviour
{
    public Vector3 centerPos;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.transform.position = centerPos;
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position = centerPos;
    }
}
