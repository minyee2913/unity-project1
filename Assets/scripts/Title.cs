using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public string SceneToLoad;

    public void LoadGame()
    {
        LoadingController.LoadScene(SceneToLoad);
    }
}
