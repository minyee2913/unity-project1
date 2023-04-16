using System;
using System.Collections;
using UnityEngine;

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
    public int type3Data = 0;
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

    public float Activating(float delay, int type) {
        activate = true;
        activeType = type;

        if (type == 1)
        {
            StartCoroutine(Type2());
        } else if (type == 2)
        {
            StartCoroutine(Type3());
            return delay + 0.5f;
        }


        return delay;
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
            if (!gameController.paused) poision += 0.25f;
            yield return new WaitForSeconds(0.7f);
        }
    }

    IEnumerator Type3()
    {
        yield return new WaitForSeconds(0.2f);
        while (gameController.paused)
        {
            yield return new WaitForSeconds(1f);
        }

        bool false1 = false;
        bool false2 = false;
        bool false3 = false;
        bool false4 = false;
        int did = type3Data;
        Vector2 pos_ = new Vector2(pos.x + 1, pos.y);
        while (true)
        {
            if (did == 0) pos_ = new Vector2(pos.x + 1, pos.y);
            else if (did == 1) pos_ = new Vector2(pos.x - 1, pos.y);
            else if (did == 2) pos_ = new Vector2(pos.x, pos.y + 1);
            else if (did == 3) pos_ = new Vector2(pos.x, pos.y - 1);
            else
            {
                did = 0;
            }

            if (!activate) break;
            if (disposed) break;
            GameObject area = Array.Find(gameController.area, element => element.GetComponent<AreaManage>().pos.Equals(pos_) && element.GetComponent<AreaManage>().disposed != true && element.GetComponent<AreaManage>().activate != true);
            if (area != null)
            {
                AreaManage manage = area.GetComponent<AreaManage>();
                manage.poision = poision + 0.07f;
                manage.type3Data = did;
                manage.Activating(0, 2);

                poision = 0;
                activate = false;
                disposed = false;
                break;
            } else
            {
                if (did == 0) false1 = true;
                else if (did == 1) false2 = true;
                else if (did == 2) false3 = true;
                else if (did == 3) false4 = true;
            }

            if (false1 && false2 && false3 && false4)
            {
                activeType = 0;
                break;
            }

            did++;
        }
    }

    private void FixedUpdate()
    {
        if (!gameController.started || gameController.paused) return;

        if (disposed)
        {
            if (activeType == 1) StopCoroutine(Type2());
            if (activeType == 2) StopCoroutine(Type3());
            Color color = Renderer.color;
            color.a = 0.6f;
            color.r = 0.1f;

            Renderer.color = color;
        }
        else {
            if (activate)
            {
                if (activeType == 0) poision += 0.006f;
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
