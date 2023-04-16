using Cinemachine;
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
    public CinemachineVirtualCamera virtualCamera;

    private Text titleText;
    private Text subText;
    private Text scoreText;
    private Text conditionText;
    private Text timerText;
    private NonDestroyData data;
    private MusicManager musicManager;
    private AudioSource countSound;
    private AudioSource overSound;

    public bool started = false;
    public bool paused = false;
    public bool overtime = false;

    public float setTime = 180;
    public int gameScore = 0;

    void Start()
    {
        titleText = title.GetComponent<Text>();
        subText = subtitle.GetComponent<Text>();
        scoreText = score.GetComponent<Text>();
        conditionText = GameObject.Find("condition").GetComponent<Text>();
        timerText = GameObject.Find("timer").GetComponent<Text>();
        data = GameObject.Find("nonDestroyData").GetComponent<NonDestroyData>();
        musicManager = GameObject.Find(data.stageMusic).GetComponent<MusicManager>();
        countSound = GameObject.Find("count").GetComponent<AudioSource>();
        overSound = GameObject.Find("overNotific").GetComponent<AudioSource>();
        virtualCamera = GameObject.Find("vcam1").GetComponent<CinemachineVirtualCamera>();

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
            countSound.Play();
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
        while(started && !paused)
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

    private void Update()
    {
        if (started && !paused) {
            setTime -= Time.deltaTime;

            timerText.text = "남은 시간 : " + (int)setTime + "s";

            if (!overtime && setTime <= 60)
            {
                overtime = true;
                data.activeDelay /= 2f;
                StartCoroutine(OnOverTime());
            }

            if (setTime <= 0)
            {
                timerText.text = "남은 시간 : 0초";
                StartCoroutine(GameOver());
            }
        }
    }

    IEnumerator OnOverTime()
    {
        titleText.text = "60초 남음!";
        titleText.color = Color.yellow;
        overSound.Play();

        yield return new WaitForSeconds(1f);

        subText.text = "오염속도가 더 빨라집니다...";
        subText.color = Color.white;

        yield return new WaitForSeconds(2f);

        titleText.text = "";
        subText.text = "";
    }

    private void FixedUpdate()
    {
        if (!started && paused) return;
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
        if (area_ == null) return false;

        AreaManage areaManage = area_.GetComponent<AreaManage>();
        if (!areaManage.activate) return false;
        
        gameScore += Convert.ToInt32(Math.Floor((1f - areaManage.poision) * 200));
        areaManage.poision = 0;
        areaManage.activate = false;

        return true;
    }

    void Pause()
    {

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
