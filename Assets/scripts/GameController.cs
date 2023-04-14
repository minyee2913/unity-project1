using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject[] area;
    public GameObject title;
    public GameObject subtitle;
    public GameObject score;

    private Material cameraMaterial;
    private Text titleText;
    private Text subText;
    private Text scoreText;
    private Text conditionText;
    private bool started = false;

    public int gameScore = 0;
    public float activingCool = 0;
    public int[] activeTypes = { 0 };
    public int leastBlock = 10;

    void Start()//(3.5, 3)
    {
        titleText = title.GetComponent<Text>();
        subText = subtitle.GetComponent<Text>();
        scoreText = score.GetComponent<Text>();
        conditionText = GameObject.Find("condition").GetComponent<Text>();
        cameraMaterial = new Material(Shader.Find("Custom/Grayscale"));

        titleText.text = "";
        subText.text = "";
        conditionText.text = string.Format("조건:\n땅 {0}칸 이상 보호", leastBlock);

        StartCoroutine(StartCool());
    }

    IEnumerator StartCool()
    {
        yield return new WaitForSeconds(2f);
        int coolTime = 3;
        while(coolTime > 0) {
            subText.text = coolTime.ToString();
            coolTime--;

            yield return new WaitForSeconds(1f);
        }

        titleText.text = "시작!";
        subText.text = "";
        yield return new WaitForSeconds(1f);

        titleText.text = "";

        started = true;

        StartCoroutine(ActivingArea());
    }

    IEnumerator ActivingArea()
    {
        while(started)
        {

            GameObject[] areas = Array.FindAll(area, element => element.GetComponent<AreaManage>().disposed != true && element.GetComponent<AreaManage>().activate != true);
            
            if (areas.Length > 0)
            {
                int i = UnityEngine.Random.Range(0, areas.Length);
                int type = UnityEngine.Random.Range(0, activeTypes.Length);

                AreaManage manage = areas[i].GetComponent<AreaManage>();
                manage.activate = true;
                manage.activeType = type;
            }

            yield return new WaitForSeconds(activingCool);

        }
    }

    private void FixedUpdate()
    {
        if (scoreText) scoreText.text = string.Format("점수: {0}", gameScore);

        GameObject[] areas = Array.FindAll(area, element => element.GetComponent<AreaManage>().disposed != true);
        if (areas.Length < leastBlock)
        {
            StartCoroutine(GameOver());
        }
    }

    public void FixGround(Vector2 pos)
    {
        GameObject area_ = Array.Find(area, element => element.GetComponent<AreaManage>().pos.Equals(pos));

        AreaManage areaManage = area_.GetComponent<AreaManage>();
        if (!areaManage.activate) return;
        
        gameScore += Convert.ToInt32(Math.Floor((1f - areaManage.poision) * 1000));
        areaManage.poision = 0;
        areaManage.activate = false;
    }

    IEnumerator GameOver()
    {
        started = false;
        subText.text = "GAME OVER";
        subText.color = Color.magenta;
        cameraMaterial.SetFloat("_Grayscale", 1);

        StopCoroutine(ActivingArea());

        yield return new WaitForSeconds(1.5f);
    }
}
