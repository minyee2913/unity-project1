using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaManage : MonoBehaviour
{
    public bool activate = false;
    public bool disposed = false;
    public int activeType = 0;
    public float poision = 0f;

    private new SpriteRenderer renderer;

    public Vector2 pos;
    void Start()
    {
        renderer = transform.GetComponent<SpriteRenderer>();
        Color color = renderer.color;
        color.a = 0f;

        string[] name = transform.name.Split('-');

        pos = new Vector2(Int32.Parse(name[0]), Int32.Parse(name[1]));

        renderer.color = color;
    }

    private void FixedUpdate()
    {

        if (disposed)
        {
            Color color = renderer.color;
            color.a = 0.6f;
            color.r = 0.1f;

            renderer.color = color;
        }
        else {
            if (activate)
            {
                if (activeType == 0) poision += 0.005f;
            }


            Color color = renderer.color;
            color.a = poision / 100f * 80f;

            renderer.color = color;

            if (poision >= 1f)
            {
                disposed = true;
                GameController controller = GameObject.Find("GameController").GetComponent<GameController>();
                controller.gameScore -= 300;
                if (controller.gameScore < 0) controller.gameScore = 0;
            }
        }
    }
}
