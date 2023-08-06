using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuUIController : MonoBehaviour
{

    private SettingController _settingController;
    private VisualElement _mainbase;
    private Button _openSetting;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        _settingController = GameObject.Find("Setting").GetComponent<SettingController>();

        _mainbase = root.Q<VisualElement>("main_base");

        _openSetting = root.Q<Button>("setting_button");

        _openSetting.RegisterCallback<ClickEvent>(_settingController.OpenSetting);
    }
}
