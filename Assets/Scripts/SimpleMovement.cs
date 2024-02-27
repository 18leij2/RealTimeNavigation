using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SimpleMovement : MonoBehaviour
{
    public float speed = 5f;
    public Vector3 direction = new Vector3(0, 0, 1);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }
}
