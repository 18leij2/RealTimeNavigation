using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class ReplaySystem : MonoBehaviour
{
    public GameObject playerObj;
    public Camera trackedCamera;

    public String fileName;

    public float replaySpeed = 1.0f; // Adjust the replay speed


    private List<ReplayData> replayDataList;
    private int currentDataIndex = 0;
    private float timer = 0.0f;

    [System.Serializable]
    private struct ReplayData
    {
        public float time;
        public Vector3 playerPosition;
        public float playerHeading;
        public Vector3 cameraPosition;
        public Vector3 cameraRotation;
    }

    void Start()
    {
        // Read the CSV file and populate the replayDataList
        replayDataList = ReadCSVFile(Directory.GetCurrentDirectory() + "\\DataCollected\\" + fileName);
    }

    void Update()
    {
        if (replayDataList.Count > 0)
        {
            timer += Time.deltaTime;
            float timeStep = replayDataList[currentDataIndex].time / replaySpeed;

            if (timer >= timeStep)
            {
                // Update player position, rotation, and camera position/rotation
                playerObj.transform.position = replayDataList[currentDataIndex].playerPosition;
                playerObj.transform.rotation = Quaternion.Euler(0, replayDataList[currentDataIndex].playerHeading, 0);
                trackedCamera.transform.position = replayDataList[currentDataIndex].cameraPosition;
                trackedCamera.transform.rotation = Quaternion.Euler(replayDataList[currentDataIndex].cameraRotation);

                timer = 0.0f;
                currentDataIndex++;

                if (currentDataIndex >= replayDataList.Count)
                {
                    currentDataIndex = 0; // Loop back to the beginning
                }
            }
        }
    }

    private List<ReplayData> ReadCSVFile(string filePath)
    {
        List<ReplayData> replayDataList = new List<ReplayData>();

        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);

            // Skip the header line
            for (int i = 1; i < lines.Length; i++)
            {
                string[] values = lines[i].Split(',');

                if (values.Length == 10)
                {
                    ReplayData data = new ReplayData();
                    data.time = float.Parse(values[0]);
                    data.playerPosition = new Vector3(float.Parse(values[1]), 0, float.Parse(values[2]));
                    data.playerHeading = float.Parse(values[3]);
                    data.cameraPosition = new Vector3(float.Parse(values[4]), float.Parse(values[5]), float.Parse(values[6]));
                    data.cameraRotation = new Vector3(float.Parse(values[7]), float.Parse(values[8]), float.Parse(values[9]));

                    replayDataList.Add(data);
                }
            }
        }

        return replayDataList;
    }
}