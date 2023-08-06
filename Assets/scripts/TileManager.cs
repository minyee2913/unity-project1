using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject[] tiles;

    private void Start()
    {
        NonDestroyData data = GameObject.Find("nonDestroyData").GetComponent<NonDestroyData>();
        for (int i = 0; i < tiles.Length; i++)
        {
            if (i == data.stage.stageTheme)
            {
                tiles[i].SetActive(true);
            } else tiles[i].SetActive(false);
        }
    }
}
