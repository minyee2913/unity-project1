using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class AreaManage : MonoBehaviour
{
    public bool activate = false;
    public bool disposed = false;
    public bool force = false;
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
        if (type == 10)
        {
            StartCoroutine(BossType1());
            return -1;
        }
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

    IEnumerator BossType1()
    {
        int wave = 0;
        int clearI = 0;
        Text bossT = GameObject.Find("bossMessage").GetComponent<Text>();
        GameObject[] prefabs = GameObject.Find("prefabGroups").GetComponent<PrefabGroup>().prefab;

        GameObject boss = Instantiate(Array.Find(prefabs, element => element.name == "the_boss"));

        boss.transform.position = new Vector2(0, 0);

        //gameController.musicManager = GameObject.Find("").GetComponent<MusicManager>();
        gameController.pauseButton.style.display = DisplayStyle.None;
        gameController.virtualCamera.GetComponent<CinemachineCameraOffset>().m_Offset.y = 0.5f;

        for (float i = 2.6f; i <= 3.8f; i += 0.1f)
        {
            gameController.virtualCamera.m_Lens.OrthographicSize = i;
            yield return new WaitForSeconds(0.05f);
        }
        gameController.camScale = 3.8f;

        yield return new WaitForSeconds(1f);

        for (float i = 0; i < 4; i+=0.5f)
        {
            boss.transform.position = new Vector2(0, i);
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(1f);

        gameController.pauseButton.style.display = DisplayStyle.Flex;

        int isOver = 1;

        while (gameController.started)
        {
            if (gameController.overtime) isOver = 2;
            if (!gameController.paused)
            {
                if (wave == 0)
                {
                    int tick = 0;

                    GameObject[] areas = Array.FindAll(gameController.area, element => element.GetComponent<AreaManage>().pos.x == tick);

                    for (int i = 0; i < areas.Length; i++)
                    {
                        areas[i].GetComponent<AreaManage>().Activating(0, 0);
                    }

                    for (int i = 0; i < 8; i++) {
                        yield return new WaitForSeconds(0.4f/isOver);

                        for (int j = 0; j < areas.Length; j++)
                        {
                            AreaManage manage = areas[j].GetComponent<AreaManage>();
                            if (manage.activate) {
                                GameObject Object = Array.Find(gameController.area, element=> element.GetComponent<AreaManage>().pos.Equals(new Vector2(tick + 1, manage.pos.y)));
                                if (Object != null)
                                {
                                    areas[j] = Object;
                                    AreaManage manage2 = Object.GetComponent<AreaManage>();
                                    if (!manage2.disposed)
                                    {
                                        manage2.Activating(0, 0);
                                        manage.activate = false;
                                        manage.poision = 0;
                                    }

                                } else
                                {
                                    manage.poision = 0.8f;
                                }
                            }
                        }

                        tick++;
                    }



                    yield return new WaitForSeconds(1.5f / isOver);
                }

                if (wave == 1)
                {
                    Vector2 vec = new Vector2();
                    for (int i = 1; i <= 4; i++)
                    {
                        if (i <= 2) vec.x = 1;
                        else vec.x = 6;
                        if (i % 2 == 0) vec.y = 2;
                        else vec.y = 4;

                        GameObject Object = Array.Find(gameController.area, element => element.GetComponent<AreaManage>().pos.Equals(vec));
                        if (Object != null)
                        {
                            AreaManage manage = Object.GetComponent<AreaManage>();
                            manage.Activating(0, 1);
                        }

                        yield return new WaitForSeconds(1f / isOver);
                    }
                }
                if (wave >= 2 && wave <= 3)
                {
                    GameObject[] areas = Array.FindAll(gameController.area, element => element.GetComponent<AreaManage>().disposed != true && element.GetComponent<AreaManage>().activate != true);
                    for (int i = 0; i < 3; i++)
                    {
                        int rd = UnityEngine.Random.Range(0, areas.Length);
                        areas[rd].GetComponent<AreaManage>().Activating(0, 0);
                    }

                    yield return new WaitForSeconds(1.5f / isOver);
                }

                if (wave == 4)
                {
                    GameObject[] areas = Array.FindAll(gameController.area, element => element.GetComponent<AreaManage>().disposed != true && element.GetComponent<AreaManage>().activate != true);
                    for (int i = 0; i < 2; i++)
                    {
                        int rd = UnityEngine.Random.Range(0, areas.Length);
                        areas[rd].GetComponent<AreaManage>().Activating(0, 2);
                    }

                    yield return new WaitForSeconds(1.5f / isOver);
                }

                if (wave == 5)
                {
                    yield return new WaitForSeconds(0.5f);
                    GameObject[] areas = Array.FindAll(gameController.area, element => element.GetComponent<AreaManage>().disposed == true);
                    if (areas.Length > 0 && clearI >= 3)
                    {
                        clearI = 0;
                        for (int i = 0; i < areas.Length;i++)
                        {
                            AreaManage area = areas[i].GetComponent<AreaManage>();
                            area.force = true;
                            area.activate = false;
                            area.poision = 0;
                            area.disposed = false;
                            gameController.gameScore -= 40;
                            
                        }
                        bossT.text = "그녀는 오염된 토양을 억지로 사용했습니다!\r\n\n해당 토양의 오염속도가 증가합니다.";
                        gameController.overSound.Play();

                        yield return new WaitForSeconds(1.6f);
                        bossT.text = "";
                        clearI = -1;
                    }
                    clearI++;

                }
                wave++;
            }
            if (wave > 5) wave = 0;
            yield return null;
        }
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
            color.b = 0;
            color.g = 0;
            color.r = 0.1f;

            Renderer.color = color;
        }
        else {
            if (activate)
            {
                if (activeType == 0)
                {
                    if (force) poision += 0.007f;
                    else poision += 0.004f;
                }
            }


            Color color = Renderer.color;
            if (force) color = Color.yellow;
            color.a = poision / 100f * 80f;

            Renderer.color = color;

            if (poision >= 1f)
            {
                disposed = true;
                disposeSound.Play();
                GameObject particle = Instantiate(gameController.DisposeParticle, transform.position, transform.rotation);
                Destroy(particle, 0.5f);

                gameController.gameScore -= 80;
                if (gameController.gameScore < 0) gameController.gameScore = 0;
            }
        }
    }
}
