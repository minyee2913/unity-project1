using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public CinemachineCameraOffset camOffset;

    public bool isPlay = false;
    Vector2 playTargetPos = new Vector2(5.5f, 2);
    Vector2 defaultPos = new Vector2(0.5f, 1);

    private GameObject player;
    public Animator anim;

    public GameObject startButton;
    public GameObject returnButton;

    public string gameplayScene = "GameScreen";

    void Start()
    {
        camOffset = GameObject.Find("vcam").GetComponent<CinemachineCameraOffset>();
        player = GameObject.Find("player");
    }

    public void OnPlay()
    {
        startButton.SetActive(false);
        returnButton.SetActive(true);
        player.GetComponent<SpriteRenderer>().flipX = false;

        StartCoroutine(MoveCamPlay());
    }

    IEnumerator MoveCamPlay()
    {

        for (float x = 0; x <= 10; x += 1f)
        {
            Vector2 pos = camOffset.m_Offset;
            pos.x = x;
            camOffset.m_Offset = pos;

            yield return new WaitForSeconds(0.03f);
        }

        anim.SetBool("onHold", true);

        while(Vector2.Distance(player.transform.position, playTargetPos) >= 0.3f)
        {
            player.transform.position = Vector2.MoveTowards(player.transform.position, playTargetPos, 0.4f);
            yield return new WaitForSeconds(0.03f);
        }

        anim.SetBool("onHold", false);

        isPlay = true;
    }

    public void OnMenu()
    {
        isPlay = false;
        startButton.SetActive(true);
        returnButton.SetActive(false);
        player.GetComponent<SpriteRenderer>().flipX = true;

        StartCoroutine(MoveCamMenu());
    }

    IEnumerator MoveCamMenu()
    {

        for (float x = 10; x >= 0; x -= 1f)
        {
            Vector2 pos = camOffset.m_Offset;
            pos.x = x;
            camOffset.m_Offset = pos;

            yield return new WaitForSeconds(0.03f);
        }

        anim.SetBool("onHold", true);

        while (Vector2.Distance(player.transform.position, defaultPos) >= 0.3f)
        {
            player.transform.position = Vector2.MoveTowards(player.transform.position, defaultPos, 0.4f);
            yield return new WaitForSeconds(0.03f);
        }

        anim.SetBool("onHold", false);
    }

    private void LoadStage()
    {
        LoadingController.LoadScene(gameplayScene);
    }

    public void Stage()
    {
        if (!isPlay) return;

        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
        int stage = int.Parse(clickedButton.GetComponentInChildren<Text>().text);

        if (stage == 1)
        {
            int[] types = { 0 };
            NonDestroyData data = GameObject.Find("nonDestroyData").GetComponent<NonDestroyData>();
            data.stage = 1;
            data.stageMusic = "Prepare for Battle";
            data.stageTheme = 0;
            data.activeDelay = 3;
            data.leastBlock = 8;
            data.activeTypes = types;
        }

        if (stage == 2)
        {
            int[] types = { 0, 1 };
            NonDestroyData data = GameObject.Find("nonDestroyData").GetComponent<NonDestroyData>();
            data.stage = 2;
            data.stageMusic = "minigame";
            data.stageTheme = 0;
            data.activeDelay = 3;
            data.leastBlock = 10;
            data.activeTypes = types;
        }

        LoadStage();
    }
}
