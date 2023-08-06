using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SettingController: MonoBehaviour
{

    private VisualElement _settingContainer;
    private VisualElement _settingMain;
    private Button _closeSetting;

    void Start()
    {
        transform.gameObject.SetActive(false);
        Debug.Log("ww");
        var root = GetComponent<UIDocument>().rootVisualElement;

        _settingContainer = root.Q<VisualElement>("setting_base");
        _settingMain = root.Q<VisualElement>("setting_main");
        _closeSetting = root.Q<Button>("setting_close");

        _settingContainer.style.display = DisplayStyle.None;
        _closeSetting.RegisterCallback<ClickEvent>(CloseSetting);
    }

    public void OpenSetting(ClickEvent ev)
    {
        transform.gameObject.SetActive(true);
        _settingContainer.style.display = DisplayStyle.Flex;
        _settingMain.AddToClassList("setting_panel-up");
        _settingContainer.AddToClassList("setting_base_fade");
    }

    public void CloseSetting(ClickEvent ev)
    {
        StartCoroutine(Closing());
        var pause = GameObject.Find("Pause");
        if (pause) pause.SetActive(true);
        transform.gameObject.SetActive(false);
    }

    IEnumerator Closing()
    {
        _settingContainer.RemoveFromClassList("setting_base_fade");
        _settingMain.RemoveFromClassList("setting_panel-up");

        yield return new WaitForSeconds(0.8f);
        _settingContainer.style.display = DisplayStyle.None;
    }

    void Update()
    {
        
    }
}
