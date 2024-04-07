using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollbackForwardSimulate : MonoBehaviour
{
    private static RollbackForwardSimulate _instance;
    public static RollbackForwardSimulate Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<RollbackForwardSimulate>();
                if (_instance == null)
                {
                    GameObject singleton = new GameObject("RollbackForwardSimulate");
                    _instance = singleton.AddComponent<RollbackForwardSimulate>();
                }
            }
            return _instance;
        }
    }

    private Maid Maid = new Maid();

    [Header("Settings")]
    public int ForwardPredictSeconds = 5;
    public int TimeScaleSpeed = 10;
    public int TicksPerSecond = 2;

    [SerializeField] private Dictionary<GameObject, List<CFrame>> positionTimeline = new Dictionary<GameObject, List<CFrame>>();
    public Dictionary<GameObject, List<CFrame>> GetPositionTimeline()
    {
        return positionTimeline;
    }


    public void RecordTimeline()
    {
        GameObject[] recordedObjs = Extensions.FindGameObjectsInLayer(LayerMask.NameToLayer("Recordable"));
        Debug.Log(recordedObjs.Length);
        float oldTimeScale = Time.timeScale;
        Time.timeScale = TimeScaleSpeed;

        var temp = Runservice.RunEvery(100, 1 / TicksPerSecond,
            (float dt) =>
            {
                foreach (GameObject obj in recordedObjs)
                {
                    if (!positionTimeline.ContainsKey(obj))
                    {
                        positionTimeline.Add(obj, new List<CFrame>());
                    }

                    positionTimeline[obj].Add(obj.transform.GetCFrame());

                }
                return true;
            }
        );
        Maid.GiveTask(temp);
        Maid.GiveTask(Runservice.RunAfter(0, ForwardPredictSeconds,
            (float dt) =>
            {
                Time.timeScale = oldTimeScale;
                RewindManager.Instance.InstantRewindTimeBySeconds(ForwardPredictSeconds);
                temp.Destroy();
                return false;
            }
        ));
    }

    private void Start()
    {
        if (!RewindManager.Instance)
        {
            Debug.LogError("Error: No RewindManager found!");
        }
        if (!Runservice.Instance) 
        {
            Debug.LogError("Error: No RewindManager found!");
        }

        RecordTimeline();
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
