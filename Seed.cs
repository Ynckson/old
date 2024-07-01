using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    public string gameSeed = "DefaultSeed";
    public int currentSeed;

    void Awake()
    {
        currentSeed = gameSeed.GetHashCode();
        Random.InitState(currentSeed);
    }
}
