using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SimpleMovement : MonoBehaviour
{
    public float speed = 5f;
    public Vector3 direction = new Vector3(0, 0, 1);
    public bool oscillate = false;
    public float targetTime = 5f;
    private float obstacleTime;

    // Start is called before the first frame update
    void Start()
    {
        obstacleTime = targetTime;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
        
        if (oscillate)
        {
            obstacleTime -= Time.deltaTime;
            if (obstacleTime <= 0.0f)
            {
                direction = -direction;
                obstacleTime = targetTime;
            }
        }
    }
}
