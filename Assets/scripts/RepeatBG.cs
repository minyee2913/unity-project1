using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatBG : MonoBehaviour
{
    [SerializeField] [Range(0f, 20f)] float speed = 3f;
    [SerializeField] float posValue;

    Vector2 startPos;
    float newPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        newPos = Mathf.Repeat(Time.time * speed, posValue);
        transform.position = startPos + Vector2.right * newPos;
    }
}
