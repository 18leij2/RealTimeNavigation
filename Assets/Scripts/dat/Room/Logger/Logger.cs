using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Logger : MonoBehaviour
{
	public GameObject PlayerObj;
	public Camera TrackedCamera;

	[Header("Settings")]
	[SerializeField] private bool loggingEnable = false;
	[SerializeField] private bool overwriteEnable = false;
	[SerializeField] private string filename;
	private string filepath;

	private string fileExt;

	private float startTime = 0f;
	private float elapsedTime = 0f;

	void Awake()
	{

	}

	void Start()
	{
		fileExt = ".csv";

		filepath = GetFilePath(filename);
		WriteHeader();

		startTime = Time.time;

		if (loggingEnable)
		{
			print("Logging Enabled.");
			if (overwriteEnable)
			{
				print("Overwrite Enabled.");
			}
		}

	}

	void FixedUpdate()
	{
		elapsedTime = Time.time - startTime;
		if (loggingEnable)
		{
			Log();
		}
	}

	private string GetFilePath(string filename)
	{
		int cntr = 1;

		string filepath = Directory.GetCurrentDirectory() + "\\DataCollected\\" + filename;
		string filepath_w_ext = filepath + fileExt;
		string newFilepath;

		if (!File.Exists(filepath_w_ext))
		{
			return filepath_w_ext;
		}

		if (overwriteEnable)
		{
			File.Delete(filepath_w_ext);
			return filepath_w_ext;
		}

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

	private void WriteHeader()
	{
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
		string headerStr = string.Join(",", header);
		WriteString(filepath, headerStr);
	}


	private void WriteString(string filepath, string outStr)
	{
		StreamWriter writer = new StreamWriter(filepath, true);
		writer.WriteLine(outStr);
		writer.Close();
	}

	private void Log()
	{
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

		string outStr = string.Join(", ", outArr);
		WriteString(filepath, outStr);
	}
}