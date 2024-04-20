using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleSpawner : MonoBehaviour
{
    [SerializeField]
    List<GameObject> people;
    public GameObject spawnPoint;
    public GameObject personToInstantiate;

    public int numberOfPeopleToSpawn = 30;

    private float timer = 0f;
    private float interval = 0f; // spawn immed
    private float minInterval = 10f; // Minimum interval (in seconds).
    private float maxInterval = 30f; // Maximum interval (in seconds).
    private int seed = 12345;       // same set of "random" numbers

    private float destroyDelay = 50f;
    void Start()
    {
        Random.InitState(seed);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        // Check if the timer has reached the desired interval.
        if (timer >= interval)
        {
            SpawnObjects();
            timer = 0f;
            // change up the time interval
            interval = Random.Range(minInterval, maxInterval);
        }
    }
    void SpawnObjects()
    {
        personToInstantiate = people[Random.Range(0, people.Count)];
        GameObject person = Instantiate(personToInstantiate, spawnPoint.transform.position, spawnPoint.transform.rotation);
        Destroy(person, destroyDelay);
    }
}
