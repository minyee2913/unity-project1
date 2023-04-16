using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public CinemachineCameraOffset camOffset;
    public StageData stageData;

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
        stageData = GameObject.Find("stageData").GetComponent<StageData>();
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
        int st = int.Parse(clickedButton.GetComponentInChildren<Text>().text) - 1;

        NonDestroyData data = GameObject.Find("nonDestroyData").GetComponent<NonDestroyData>();
        Stage stage = stageData.stages[st];
        data.stage = stage;

        LoadStage();
    }
}
