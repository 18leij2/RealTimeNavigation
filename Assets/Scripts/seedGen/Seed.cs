using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    // Start is called before the first frame update
    public string gameSeed = "Default";
    public int currentSeed = 0;
    private void Awake() {
        currentSeed = gameSeed.GetHashCode();
        Random.InitState(currentSeed);
    }
}
