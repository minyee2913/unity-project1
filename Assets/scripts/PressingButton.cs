using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PressingButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Serializable]
    public class ButtonClickedEvent : UnityEvent { }

    [FormerlySerializedAs("onPress")]
    [SerializeField]
    private ButtonClickedEvent OnPress = new ButtonClickedEvent();
    public bool isPressed = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        OnPress.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }
}
