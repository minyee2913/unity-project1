using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragNDrop : MonoBehaviour
{
    string onStart;
    GameObject atStart;
    GameObject atEnd;
    string onEnd;
    private void Start()
    {
        
    }

    private void OnMouseDown()
    {
    }

    private void OnMouseDrag()
    {
        Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldObjPos = Camera.main.ScreenToWorldPoint(mousePos);

        transform.position = worldObjPos;
    }

    private void OnMouseUp()
    {
    }
}
