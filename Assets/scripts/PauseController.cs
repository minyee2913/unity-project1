using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PauseController: MonoBehaviour
{

    public VisualElement root;
    private VisualElement _pauseContainer;
    private VisualElement _pauseMain;
    private GameController _gameController;
    public GameObject setting;

    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        var root2 = GameObject.Find("UI").GetComponent<UIDocument>().rootVisualElement;
        _gameController = GameObject.Find("GameController").GetComponent<GameController>();

        _pauseContainer = root.Q<VisualElement>("pause_base");
        _pauseMain = root.Q<VisualElement>("pause_main");

        _pauseContainer.style.display = DisplayStyle.None;
        root2.Q<Button>("pause_button").RegisterCallback<ClickEvent>(Openpause);
        root.Q<Button>("resume_button").RegisterCallback<ClickEvent>(_gameController.Continue);
        root.Q<Button>("exit").RegisterCallback<ClickEvent>(_gameController.Exit);
        root.Q<Button>("setting").RegisterCallback<ClickEvent>(Setting);
    }

    public void Openpause(ClickEvent ev)
    {
        _pauseContainer.style.display = DisplayStyle.Flex;
        _pauseMain.AddToClassList("setting_panel-up");
        _pauseContainer.AddToClassList("setting_base_fade");
    }

    public void Setting(ClickEvent ev) {
        transform.gameObject.SetActive(false);
        setting.SetActive(true);
        setting.GetComponent<SettingController>().OpenSetting(ev);
    }

    public void Closepause(ClickEvent ev)
    {
        StartCoroutine(Closing());
    }

    IEnumerator Closing()
    {
        _pauseContainer.RemoveFromClassList("setting_base_fade");
        _pauseMain.RemoveFromClassList("setting_panel-up");

        yield return new WaitForSeconds(0.8f);
        _pauseContainer.style.display = DisplayStyle.None;
    }

    void Update()
    {
        
    }
}
