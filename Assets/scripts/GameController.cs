using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    public GameObject[] area;
    public GameObject title;
    public GameObject subtitle;
    public GameObject score;
    public CinemachineVirtualCamera virtualCamera;
    public GameObject Player;
    public GameObject DisposeParticle;

    public GameObject pause_;
    PauseController pause;
    public GameObject ingame;

    public Text titleText;
    public Text subText;
    private Text scoreText;
    private Text conditionText;
    private Text timerText;
    private NonDestroyData data;
    public MusicManager musicManager;
    private AudioSource countSound;
    public AudioSource overSound;
    public UnityEngine.UIElements.Button pauseButton;
    public float camScale = 2.6f;

    public bool started = false;
    public bool paused = false;
    public bool overtime = false;

    public float setTime = 160;
    public int gameScore = 3000;

    void Start()
    {
        var root = GameObject.Find("UI").GetComponent<UIDocument>().rootVisualElement;

        titleText = title.GetComponent<Text>();
        subText = subtitle.GetComponent<Text>();
        scoreText = score.GetComponent<Text>();
        conditionText = GameObject.Find("condition").GetComponent<Text>();
        timerText = GameObject.Find("timer").GetComponent<Text>();

        data = GameObject.Find("nonDestroyData").GetComponent<NonDestroyData>();
        if (data.stage.stageMusic != null) musicManager = GameObject.Find(data.stage.stageMusic).GetComponent<MusicManager>();

        countSound = GameObject.Find("count").GetComponent<AudioSource>();
        overSound = GameObject.Find("overNotific").GetComponent<AudioSource>();
        virtualCamera = GameObject.Find("vcam1").GetComponent<CinemachineVirtualCamera>();
        pauseButton = root.Q<UnityEngine.UIElements.Button>();

        titleText.text = "";
        subText.text = "";
        conditionText.text = string.Format("조건:\n땅 {0}칸 이상 보호", data.stage.leastBlock);

        pause = pause_.GetComponent<PauseController>();
        pauseButton.style.display = DisplayStyle.None;
        pauseButton.RegisterCallback<ClickEvent>(Pause);

        StartCoroutine(StartCool());
    }

    IEnumerator StartCool()
    {
        yield return new WaitForSeconds(2f);
        int coolTime = 3;
        while (coolTime > 0) {
            subText.text = coolTime.ToString();
            countSound.Play();
            coolTime--;

            yield return new WaitForSeconds(1f);
        }

        titleText.text = "시작!";
        subText.text = "";

        pauseButton.style.display = DisplayStyle.Flex;
        if (musicManager != null) musicManager.Play();

        yield return new WaitForSeconds(1f);

        titleText.text = "";

        started = true;

        StartCoroutine(ActivingArea());
    }

    IEnumerator ActivingArea()
    {
        int before = 0;
        while (started)
        {
            float waitDelay = data.stage.activeDelay;
            if (!paused)
            {
                GameObject[] areas = Array.FindAll(area, element => element.GetComponent<AreaManage>().disposed != true && element.GetComponent<AreaManage>().activate != true);

                if (areas.Length > 0)
                {
                    int i = GenerateRandomNumber(0, areas.Length, before);

                    int type = UnityEngine.Random.Range(0, data.stage.activeTypes.Length);

                    AreaManage manage = areas[i].GetComponent<AreaManage>();
                    waitDelay = manage.Activating(data.stage.activeDelay, data.stage.activeTypes[type]);
                }
            }

            if (waitDelay == -1) break;
            yield return new WaitForSeconds(waitDelay);

        }
    }

    private int GenerateRandomNumber(int min, int max, int not)
    {
        int randomValue = UnityEngine.Random.Range(min, max);
        while (not == randomValue)
        {
            randomValue = UnityEngine.Random.Range(min, max);
        }
        return randomValue;
    }

    private void Update()
    {
        if (started && !paused) {
            setTime -= Time.deltaTime;

            timerText.text = "남은 시간 : " + (int)setTime + "s";

            if (!overtime && setTime <= 60)
            {
                overtime = true;
                data.stage.activeDelay /= 2f;
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
        if (!started || paused) return;
        if (gameScore < 0) gameScore = 0;
        if (scoreText) scoreText.text = string.Format("점수: {0}", gameScore);

        GameObject[] areas = Array.FindAll(area, element => element.GetComponent<AreaManage>().disposed != true);
        if (areas.Length < data.stage.leastBlock)
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

    public void Pause(ClickEvent ev)
    {
        paused = true;
        pause.Openpause(ev);
        ingame.SetActive(false);

        Vector2 pos = Player.transform.position;

        virtualCamera.transform.position = new Vector3(pos.x, pos.y, virtualCamera.transform.position.z);
        virtualCamera.m_Lens.Dutch = -30;
        virtualCamera.m_Lens.OrthographicSize = 1.5f;

        GameObject.Find("pauseText").GetComponent<Text>().text = "현재 점수: " + gameScore + "\n남은 시간: " + (int)setTime + "s";
    }

    public void Continue(ClickEvent ev)
    {
        paused = false;
        pause.Closepause(ev);
        ingame.SetActive(true);
        virtualCamera.transform.position = new Vector3(0, 0, virtualCamera.transform.position.z);

        virtualCamera.m_Lens.Dutch = 0;
        virtualCamera.m_Lens.OrthographicSize = camScale;
    }

    public void Exit(ClickEvent ev)
    {
        LoadingController.LoadScene("Menu");
    }

    IEnumerator GameOver()
    {
        started = false;
        StopCoroutine(ActivingArea());

        yield return new WaitForSeconds(1f);

        if (setTime > 0)
        {
            titleText.text = "GAME OVER";
            titleText.color = Color.red;
        } else
        {
            titleText.text = "TIME OVER";
            titleText.color = Color.white;
        }

        subText.text = "score " + gameScore;
        subText.color = Color.yellow;

        yield return new WaitForSeconds(3.5f);

        LoadingController.LoadScene("Menu");
    }
}
