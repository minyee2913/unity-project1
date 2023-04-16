using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonDestroyData : MonoBehaviour
{
    public Stage stage;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
