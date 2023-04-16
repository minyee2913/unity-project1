using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonDestroyData : MonoBehaviour
{
    public int stage;
    public string stageMusic;
    public int stageTheme;
    public float activeDelay;
    public int leastBlock;
    public int[] activeTypes;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
