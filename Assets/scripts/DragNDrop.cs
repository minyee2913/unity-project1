using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragNDrop : MonoBehaviour
{
    string onStart;
    GameObject atStart;
    GameObject atEnd;
    string onEnd;
    public Animator anim;
    private void Start()
    {
        
    }

    private void OnMouseDown()
    {
        anim.SetBool("onHold", true);
    }

    private void OnMouseDrag()
    {
        Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y+0.5f);
        Vector2 worldObjPos = Camera.main.ScreenToWorldPoint(mousePos);

        transform.position = worldObjPos;
    }

    private void OnMouseUp()
    {
        anim.SetBool("onHold", false);
        Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldObjPos = Camera.main.ScreenToWorldPoint(mousePos);

        transform.position = worldObjPos;
    }
}
