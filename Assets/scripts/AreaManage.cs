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

    private SpriteRenderer Renderer;
    private AudioSource disposeSound;
    private GameController gameController;

    public Vector2 pos;
    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        disposeSound = GameObject.Find("explosion").GetComponent<AudioSource>();
        Renderer = transform.GetComponent<SpriteRenderer>();
        Color color = Renderer.color;
        color.a = 0f;

        string[] name = transform.name.Split('-');

        pos = new Vector2(Int32.Parse(name[0]), Int32.Parse(name[1]));

        Renderer.color = color;
    }

    IEnumerator Type2()
    {
        GameObject area1 = Array.Find(gameController.area, element => element.GetComponent<AreaManage>().pos.Equals(new Vector2(pos.x + 1, pos.y)));
        if (area1 != null)
        {
            AreaManage manage = area1.GetComponent<AreaManage>();
            if (manage != null && !manage.activate) manage.activate = true;
        }

        GameObject area2 = Array.Find(gameController.area, element => element.GetComponent<AreaManage>().pos.Equals(new Vector2(pos.x - 1, pos.y)));
        if (area2 != null)
        {
            AreaManage manage = area2.GetComponent<AreaManage>();
            if (manage != null && !manage.activate) manage.activate = true;
        }

        GameObject area3 = Array.Find(gameController.area, element => element.GetComponent<AreaManage>().pos.Equals(new Vector2(pos.x, pos.y + 1)));
        if (area3 != null)
        {
            AreaManage manage = area3.GetComponent<AreaManage>();
            if (manage != null && !manage.activate) manage.activate = true;
        }

        GameObject area4 = Array.Find(gameController.area, element => element.GetComponent<AreaManage>().pos.Equals(new Vector2(pos.x, pos.y - 1)));
        if (area4 != null)
        {
            AreaManage manage = area4.GetComponent<AreaManage>();
            if (manage != null && !manage.activate) manage.activate = true;
        }

        while (gameController.started && activate)
        {
            poision += 0.25f;
            yield return new WaitForSeconds(0.7f);
        }
    }

    private void FixedUpdate()
    {
        if (!gameController.started) return;

        if (disposed)
        {
            if (activeType == 1) StopCoroutine(Type2());
            Color color = Renderer.color;
            color.a = 0.6f;
            color.r = 0.1f;

            Renderer.color = color;
        }
        else {
            if (activate)
            {
                if (activeType == 0) poision += 0.003f;
                if (activeType == 1 && poision == 0) StartCoroutine(Type2());
            }


            Color color = Renderer.color;
            color.a = poision / 100f * 80f;

            Renderer.color = color;

            if (poision >= 1f)
            {
                disposed = true;
                disposeSound.Play();

                gameController.gameScore -= 80;
                if (gameController.gameScore < 0) gameController.gameScore = 0;
            }
        }
    }
}
