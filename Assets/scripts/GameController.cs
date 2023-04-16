using Cinemachine;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject[] area;
    public GameObject title;
    public GameObject subtitle;
    public GameObject score;
    public CinemachineVirtualCamera virtualCamera;
    public GameObject Player;

    public GameObject pause;
    public GameObject ingame;

    private Text titleText;
    private Text subText;
    private Text scoreText;
    private Text conditionText;
    private Text timerText;
    private NonDestroyData data;
    private MusicManager musicManager;
    private AudioSource countSound;
    private AudioSource overSound;
    private GameObject pauseButton;

    public bool started = false;
    public bool paused = false;
    public bool overtime = false;

    public float setTime = 160;
    public int gameScore = 0;

    void Start()
    {
        titleText = title.GetComponent<Text>();
        subText = subtitle.GetComponent<Text>();
        scoreText = score.GetComponent<Text>();
        conditionText = GameObject.Find("condition").GetComponent<Text>();
        timerText = GameObject.Find("timer").GetComponent<Text>();
        data = GameObject.Find("nonDestroyData").GetComponent<NonDestroyData>();
        musicManager = GameObject.Find(data.stage.stageMusic).GetComponent<MusicManager>();
        countSound = GameObject.Find("count").GetComponent<AudioSource>();
        overSound = GameObject.Find("overNotific").GetComponent<AudioSource>();
        virtualCamera = GameObject.Find("vcam1").GetComponent<CinemachineVirtualCamera>();
        pauseButton = GameObject.Find("pauseButton");

        titleText.text = "";
        subText.text = "";
        conditionText.text = string.Format("조건:\n땅 {0}칸 이상 보호", data.stage.leastBlock);
        pause.SetActive(false);
        pauseButton.SetActive(false);

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

        pauseButton.SetActive(true);
        musicManager.Play();

        yield return new WaitForSeconds(1f);

        titleText.text = "";

        started = true;

        StartCoroutine(ActivingArea());
    }

    IEnumerator ActivingArea()
    {
        while (started)
        {
            float waitDelay = data.stage.activeDelay;
            if (!paused)
            {
                GameObject[] areas = Array.FindAll(area, element => element.GetComponent<AreaManage>().disposed != true && element.GetComponent<AreaManage>().activate != true);

                if (areas.Length > 0)
                {
                    int i = UnityEngine.Random.Range(0, areas.Length);
                    int type = UnityEngine.Random.Range(0, data.stage.activeTypes.Length);

                    AreaManage manage = areas[i].GetComponent<AreaManage>();
                    waitDelay = manage.Activating(data.stage.activeDelay, data.stage.activeTypes[type]);
                }
            }

            yield return new WaitForSeconds(waitDelay);

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

    public void Pause()
    {
        paused = true;
        pause.SetActive(true);
        ingame.SetActive(false);

        Vector2 pos = Player.transform.position;

        virtualCamera.transform.position = new Vector3(pos.x, pos.y, virtualCamera.transform.position.z);
        virtualCamera.m_Lens.Dutch = -30;
        virtualCamera.m_Lens.OrthographicSize = 1.5f;

        GameObject.Find("pauseText").GetComponent<Text>().text = "현재 점수: " + gameScore + "\n남은 시간: " + (int)setTime + "s";
    }

    public void Continue()
    {
        paused = false;
        pause.SetActive(false);
        ingame.SetActive(true);
        virtualCamera.transform.position = new Vector3(0, 0, virtualCamera.transform.position.z);

        virtualCamera.m_Lens.Dutch = 0;
        virtualCamera.m_Lens.OrthographicSize = 2.6f;
    }

    public void Exit()
    {
        LoadingController.LoadScene("Menu");
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
