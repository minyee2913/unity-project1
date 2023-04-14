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

    private Text titleText;
    private Text subText;
    private Text scoreText;
    private Text conditionText;
    private bool started = false;
    private NonDestroyData data;
    private MusicManager musicManager;

    public int gameScore = 0;

    void Start()
    {
        titleText = title.GetComponent<Text>();
        subText = subtitle.GetComponent<Text>();
        scoreText = score.GetComponent<Text>();
        conditionText = GameObject.Find("condition").GetComponent<Text>();
        data = GameObject.Find("nonDestroyData").GetComponent<NonDestroyData>();
        musicManager = GameObject.Find(data.stageMusic).GetComponent<MusicManager>();

        titleText.text = "";
        subText.text = "";
        conditionText.text = string.Format("조건:\n땅 {0}칸 이상 보호", data.leastBlock);

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

        musicManager.Play();

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
                int type = UnityEngine.Random.Range(0, data.activeTypes.Length);

                AreaManage manage = areas[i].GetComponent<AreaManage>();
                manage.activate = true;
                manage.activeType = type;
            }

            yield return new WaitForSeconds(data.activeDelay);

        }
    }

    private void FixedUpdate()
    {
        if (!started) return;
        if (gameScore < 0) gameScore = 0;
        if (scoreText) scoreText.text = string.Format("점수: {0}", gameScore);

        GameObject[] areas = Array.FindAll(area, element => element.GetComponent<AreaManage>().disposed != true);
        if (areas.Length < data.leastBlock)
        {
            StartCoroutine(GameOver());
        }
    }

    public bool FixGround(Vector2 pos)
    {
        GameObject area_ = Array.Find(area, element => element.GetComponent<AreaManage>().pos.Equals(pos) && element.GetComponent<AreaManage>().disposed != true);

        AreaManage areaManage = area_.GetComponent<AreaManage>();
        if (!areaManage.activate) return false;
        
        gameScore += Convert.ToInt32(Math.Floor((1f - areaManage.poision) * 1000));
        areaManage.poision = 0;
        areaManage.activate = false;

        return true;
    }

    IEnumerator GameOver()
    {
        started = false;
        StopCoroutine(ActivingArea());

        yield return new WaitForSeconds(1f);

        titleText.text = "GAME OVER";
        titleText.color = Color.red;

        subText.text = "score " + gameScore;
        subText.color = Color.yellow;

        yield return new WaitForSeconds(3.5f);

        LoadingController.LoadScene("Menu");
    }
}
