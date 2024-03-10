using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Serializable attribute allows the Logger class to be serialized
[Serializable]
public class Logger : Singleton<Logger>
{
    // Public variables accessible in Unity Editor
    public GameObject PlayerObj;    // The GameObject of the player
    public Camera TrackedCamera;    // The Camera to track

    // Serialized private variables, visible in Unity Editor
    [Header("Settings")]
    [SerializeField] private bool loggingEnable = false;    // Enable logging
    [SerializeField] private bool overwriteEnable = false;  // Enable overwrite existing log
    [SerializeField] private string filename;               // Name of the log file
    private string filepath;                                  // Full path of the log file

    private string fileExt;  // File extension (in this case, ".csv")

    private float startTime = 0f;     // Time when logging starts
    private float elapsedTime = 0f;   // Elapsed time since logging started


    // Called before the first frame update
    void Start()
    {
        fileExt = ".csv";   // Setting the file extension

        // Get the full path for the log file
        filepath = GetFilePath(filename);

        // Write the header for the CSV file
        WriteHeader();

        // Record the starting time for logging
        startTime = Time.time;

        // Print messages based on logging settings
        if (loggingEnable)
        {
            print("Logging Enabled.");
            if (overwriteEnable)
            {
                print("Overwrite Enabled.");
            }
        }
    }

    // Called every fixed framerate frame
    void FixedUpdate()
    {
        // Calculate elapsed time
        elapsedTime = Time.time - startTime;

        // If logging is enabled, log data
        if (loggingEnable)
        {
            Log();
        }
    }

    // Function to get the file path for the log file
    private string GetFilePath(string filename)
    {
        int cntr = 1;

        // Create the initial filepath
        string filepath = Directory.GetCurrentDirectory() + "\\DataCollected\\" + filename;
        string filepath_w_ext = filepath + fileExt;
        string newFilepath;

        // If the file does not exist, return the filepath
        if (!File.Exists(filepath_w_ext))
        {
            return filepath_w_ext;
        }

        // If overwrite is enabled, delete the existing file and return filepath
        if (overwriteEnable)
        {
            File.Delete(filepath_w_ext);
            return filepath_w_ext;
        }

        // If the file exists and overwrite is not enabled, find a new filename
        while (true)
        {
            newFilepath = filepath + "(" + cntr.ToString() + ")" + fileExt;

            if (!File.Exists(newFilepath))
            {
                return newFilepath;
            }

            cntr++;
        }
    }

    // Function to write the header of the CSV file
    private void WriteHeader()
    {
        // Array of header values
        string[] header = new string[] {
            "Time",
            "X",
            "Y",
            "Heading",
            "CameraX",
            "CameraY",
            "CameraZ",
            "CameraAngX",
            "CameraAngY",
            "CameraAngZ",
        };

        // Join header array into a single string
        string headerStr = string.Join(",", header);

        // Write the header string to the CSV file
        WriteString(filepath, headerStr);
    }

    // Function to write a string to a file
    private void WriteString(string filepath, string outStr)
    {
        // Create a StreamWriter to write to the file
        StreamWriter writer = new StreamWriter(filepath, true);

        // Write the string and go to the next line
        writer.WriteLine(outStr);

        // Close the StreamWriter
        writer.Close();
    }

    // Function to log data to the CSV file
    private void Log()
    {
        // Convert various data to strings for logging
        string elapsedTimeString = elapsedTime.ToString();
        string playerX = PlayerObj.transform.position.x.ToString();
        string playerY = PlayerObj.transform.position.z.ToString();
        string playerHeading = PlayerObj.transform.eulerAngles.y.ToString();
        string cameraX = TrackedCamera.transform.position.x.ToString();
        string cameraY = TrackedCamera.transform.position.y.ToString();
        string cameraZ = TrackedCamera.transform.position.z.ToString();
        string cameraAngX = TrackedCamera.transform.eulerAngles.x.ToString();
        string cameraAngY = TrackedCamera.transform.eulerAngles.y.ToString();
        string cameraAngZ = TrackedCamera.transform.eulerAngles.z.ToString();

        // Array of data values
        string[] outArr = new string[] {
            elapsedTimeString,
            playerX,
            playerY,
            playerHeading,
            cameraX,
            cameraY,
            cameraZ,
            cameraAngX,
            cameraAngY,
            cameraAngZ,
        };

        // Join data array into a single string
        string outStr = string.Join(", ", outArr);

        // Write the data string to the CSV file
        WriteString(filepath, outStr);
    }
}
