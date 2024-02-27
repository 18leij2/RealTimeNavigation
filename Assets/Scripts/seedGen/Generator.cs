using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public GameObject[] wallObjects;
    public GameObject[] movingObjects;

    public int wallObjectsCount = 3;
    public int movingObjectsCount = 2;
    // Start is called before the first frame update
    void Start()
    {
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
