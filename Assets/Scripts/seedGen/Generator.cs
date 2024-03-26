using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public GameObject[] wallObjects;
    public GameObject[] movingObjects;

    public int wallObjectsCount = 3;
    public int movingObjectsCount = 2;
    
    public GameObject player;
    public Vector3 playerLocation = new Vector3(-2.8f, 1.04f, -2.66f);
    public GameObject target;
    public Vector3 targetLocation = new Vector3(0.05f, 0.54f, 0.66f);

    void Start()
    {
        player.transform.position = playerLocation;
        //target.transform.position = targetLocation;

        for(int i = 0; i < wallObjectsCount; i++) {
            Vector3 pos = Random.insideUnitSphere * 7;
            pos.y = 20;
            
            RaycastHit hit;
            if (Physics.Raycast(pos, Vector3.down, out hit)) {
                pos.y = 1;
                GameObject selected = wallObjects[Random.Range(0, wallObjects.Length)];
                GameObject wall = Instantiate(selected, pos, selected.transform.rotation);
                wall.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            }
            
        }

        for(int i = 0; i < movingObjectsCount; i++) {
            Vector3 pos = Random.insideUnitSphere * 7;
            pos.y = 20;
            RaycastHit hit;
            if (Physics.Raycast(pos, Vector3.down, out hit)) {
                pos.y = 1.08f;
                GameObject selected = movingObjects[Random.Range(0, movingObjects.Length)];
                GameObject movobs = Instantiate(selected, pos, selected.transform.rotation);
                SimpleMovement simpleMovementScript = movobs.GetComponent<SimpleMovement>();
                Vector3 direction = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
                direction.Normalize();
                simpleMovementScript.direction = direction;
                //movobs.direction = Vector3.Normalize(new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)));
                //movobs.transform.rotation = Quaternion.Euler(-90, Random.Range(0, 360), 0);
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
