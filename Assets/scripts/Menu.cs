using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Menu : MonoBehaviour
{
    public CinemachineCameraOffset camOffset;
    public StageData stageData;

    public bool isPlay = false;
    public bool isShop = false;
    Vector2 playTargetPos = new Vector2(5.5f, 2);
    Vector2 shopTargetPos = new Vector2(-15, 1);
    Vector2 defaultPos = new Vector2(0.5f, 1);

    private GameObject player;
    public Animator anim;

    UnityEngine.UIElements.Button startButton;
    UnityEngine.UIElements.Button returnButton;
    public GameObject toShop;
    public GameObject backShop;
    public GameObject stageMenu;


    public GameObject soseogText;
    public GameObject soseog_Text;
    public GameObject moneyText;
    public GameObject money_Text;

    UnityEngine.UIElements.Label soText;
    UnityEngine.UIElements.Label moText;

    private NonDestroyData data;

    int selectedstage = 0;

    public string gameplayScene = "GameScreen";

    void Start()
    {
        var root = GameObject.Find("UI").GetComponent<UIDocument>().rootVisualElement;

        camOffset = GameObject.Find("vcam").GetComponent<CinemachineCameraOffset>();
        player = GameObject.Find("player");
        stageData = GameObject.Find("stageData").GetComponent<StageData>();
        data = GameObject.Find("nonDestroyData").GetComponent<NonDestroyData>();

        startButton = root.Q<UnityEngine.UIElements.Button>("play_button");
        returnButton = root.Q<UnityEngine.UIElements.Button>("return_button");

        soText = root.Q<UnityEngine.UIElements.Label>("infoSoseog");
        moText = root.Q<UnityEngine.UIElements.Label>("infoMoney");

        returnButton.style.display = DisplayStyle.None;

        backShop.SetActive(false);

        startButton.RegisterCallback<ClickEvent>(OnPlay);
        returnButton.RegisterCallback<ClickEvent>(OnMenu);
    }

    private void FixedUpdate()
    {
        Text soT1 = soseogText.GetComponent<Text>();
        Text soT2 = soseog_Text.GetComponent<Text>();

        soT1.text = data.GetSoseog() + " / 120";
        soT2.text = soT1.text;
        soText.text = "¼Ò¼®È¸, " + data.GetSoseog() + "g/eq";

        Text moT1 = moneyText.GetComponent<Text>();
        Text moT2 = money_Text.GetComponent<Text>();

        moT1.text = data.GetMoney().ToString();
        moT2.text = moT1.text;
        moText.text = "µ·, " + data.GetMoney() + "¿ø";
    }

    public void OnPlay(ClickEvent ev)
    {
        startButton.style.display = DisplayStyle.None;
        returnButton.style.display = DisplayStyle.Flex;
        toShop.SetActive(false);

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

    public void OnMenu(ClickEvent ev)
    {
        isPlay = false;
        startButton.style.display = DisplayStyle.Flex;
        returnButton.style.display = DisplayStyle.None;

        toShop.SetActive(true);
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

    public void OnShop()
    {
        startButton.style.display = DisplayStyle.None;
        toShop.SetActive(false);
        backShop.SetActive(true);
        player.GetComponent<SpriteRenderer>().flipX = true;

        StartCoroutine(MoveCamShop());
    }

    IEnumerator MoveCamShop()
    {

        for (float x = 0; x >= -13; x -= 1f)
        {
            Vector2 pos = camOffset.m_Offset;
            pos.x = x;
            camOffset.m_Offset = pos;

            yield return new WaitForSeconds(0.03f);
        }

        anim.SetBool("onHold", true);

        while (Vector2.Distance(player.transform.position, shopTargetPos) >= 0.3f)
        {
            player.transform.position = Vector2.MoveTowards(player.transform.position, shopTargetPos, 0.8f);
            yield return new WaitForSeconds(0.03f);
        }

        anim.SetBool("onHold", false);

        isShop = true;
    }

    public void OnMenuFromShop()
    {
        isPlay = false;
        startButton.style.display = DisplayStyle.Flex;
        toShop.SetActive(true);
        backShop.SetActive(false);
        player.GetComponent<SpriteRenderer>().flipX = false;

        StartCoroutine(MoveCamMenuFromShop());
    }

    IEnumerator MoveCamMenuFromShop()
    {

        for (float x = -13; x <= 0; x += 1f)
        {
            Vector2 pos = camOffset.m_Offset;
            pos.x = x;
            camOffset.m_Offset = pos;

            yield return new WaitForSeconds(0.03f);
        }

        anim.SetBool("onHold", true);

        while (Vector2.Distance(player.transform.position, defaultPos) >= 0.3f)
        {
            player.transform.position = Vector2.MoveTowards(player.transform.position, defaultPos, 0.8f);
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

        selectedstage = st;

        stageMenu.SetActive(true);
    }

    public void GoStage()
    {
        if (!isPlay) return;

        NonDestroyData data = GameObject.Find("nonDestroyData").GetComponent<NonDestroyData>();
        Stage stage = stageData.stages[selectedstage];
        data.stage = stage;

        data.AddSoseog(-10);

        LoadStage();
    }
}
