using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro.EditorUtilities;
using UnityEngine;
public class SimpleMovement : MonoBehaviour
{
    // targetTime must be at least 2x larger than forwardTime
    public float speed = 5f;
    public Vector3 direction = new Vector3(0, 0, 1);
    public bool oscillate = false;
    public float targetTime = 5f;
    private float obstacleTime;
    public float forwardTime = 0.5f;
    private float forwardCount;
    public Dictionary<string, object> forwardSim = new Dictionary<string, object>();

    public Vector3 position;
    public Vector3 projection;

    // using forward manager's time
    private float forwardProject;

    // Start is called before the first frame update
    void Start()
    {
        // find the specified forward simulation amount
        GameObject managerScriptObject = GameObject.Find("Floor");
        ForwardSimManager managerScript = managerScriptObject.GetComponent<ForwardSimManager>();
        forwardProject = managerScript.forwardProjectTime;

        // add to the obstacle manager
        // ForwardSimManager.instance.AddObject(this.gameObject);

        obstacleTime = targetTime;
        forwardCount = 0;
        forwardSim["Position"] = new List<Vector3>();
        forwardSim["Vector"] = new List<Vector3>();
        forwardSim["Frequency"] = obstacleTime;
        forwardSim["Velocity"] = speed;

        simulate();
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

        // Forward simulation
        forwardCount -= Time.deltaTime;
        if (forwardCount <= 0.0f)
        {
            //simulate();
            //forwardCount = forwardProject;
            //Debug.Log("Position now: " + ((List<Vector3>)forwardSim["Position"])[0]);
            //Debug.Log("Position in " + forwardProject + " seconds: " + ((List<Vector3>)forwardSim["Position"])[forwardSim.Count]);
            //Debug.Log("Future positions: " + ((List<Vector3>)forwardSim["Position"])[0] + " || " + ((List<Vector3>)forwardSim["Position"])[1] + " || " +
            //    ((List<Vector3>)forwardSim["Position"])[2] + " || " + ((List<Vector3>)forwardSim["Position"])[3] + " || " + ((List<Vector3>)forwardSim["Position"])[4]);
        }
    }

    public List<Vector3> simulate()
    {
        forwardSim["Position"] = new List<Vector3>();

        float interval = forwardProject / forwardTime;

        // Debug.Log("Interval is +" + interval);

        for (int i = 0; i < (interval + 1); i++)
        {
            // Cast the value to List<Vector3> before adding elements
            List<Vector3> positions = (List<Vector3>)forwardSim["Position"];

            // Calculate new position
            if (i == 0)
            {
                Vector3 newPosition = gameObject.transform.position;
                positions.Add(newPosition);
            }
            else
            {
                Vector3 newPosition = gameObject.transform.position + (direction * speed * forwardTime * i);
                positions.Add(newPosition);
            }

            // Update the value in the dictionary
            forwardSim["Position"] = positions;
        }

        List<Vector3> vectors = new List<Vector3>();
        vectors.Add(((List<Vector3>)forwardSim["Position"])[0]); // current position
        vectors.Add(((List<Vector3>)forwardSim["Position"])[((List<Vector3>)forwardSim["Position"]).Count - 1]); // position after the whole time step

        //Debug.Log("vectors size :" + vectors.Count);

        //position = ((List<Vector3>)forwardSim["Position"])[0];
        //projection = ((List<Vector3>)forwardSim["Position"])[forwardSim.Count];

        return vectors;

        // old code based on oscillating movement, we aren't using this
        //for (int i = 0; i < 0; i++) // i < 5
        //{
        //    // Cast the value to List<Vector3> before adding elements
        //    List<Vector3> positions = (List<Vector3>)forwardSim["Position"];
        //    List<Vector3> directions = (List<Vector3>)forwardSim["Vector"];

        //    // Calculate new position
        //    if (i == 0)
        //    {
        //        Vector3 newPosition = gameObject.transform.position;
        //        Vector3 newDirection = direction;
        //        positions.Add(newPosition);
        //        directions.Add(newDirection);
        //    }
        //    else if (obstacleTime - (i * forwardTime) == 0)
        //    {
        //        Vector3 newPosition = gameObject.transform.position + (direction * speed * forwardTime * (i - 1));
        //        Vector3 newDirection = -direction;
        //        positions.Add(newPosition);
        //        directions.Add(newDirection);
        //    }
        //    else if (obstacleTime - (i * forwardTime) > 0)
        //    {
        //        Vector3 newPosition = gameObject.transform.position + (direction * speed * forwardTime * i);
        //        Vector3 newDirection = direction;
        //        positions.Add(newPosition);
        //        directions.Add(newDirection);
        //    }
        //    else
        //    {
        //        // Turning point
        //        Vector3 newPosition = gameObject.transform.position + (direction * speed * forwardTime * (i - 1)) + (-direction * speed * forwardTime);
        //        Vector3 newDirection = -direction;
        //        positions.Add(newPosition);
        //        directions.Add(newDirection);
        //    }

        //    // Update the value in the dictionary
        //    forwardSim["Position"] = positions;
        //    forwardSim["Vector"] = directions;
        //}
    }
}
