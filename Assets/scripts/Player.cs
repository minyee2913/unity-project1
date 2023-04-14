using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer Renderer;
    private GameController Controller;

    private void Start()
    {
        Renderer = GetComponent<SpriteRenderer>();
        Controller = GameObject.Find("GameController").GetComponent<GameController>();
    }
    public void MoveUp()
    {
        Vector2 pos = transform.position;
        if (pos.y > 1) return;

        pos.y += 1;
        transform.position = pos;
    }

    public void MoveDown()
    {
        Vector2 pos = transform.position;
        if (pos.y < -1) return;

        pos.y -= 1;
        transform.position = pos;
    }

    public void MoveLeft()
    {
        Vector2 pos = transform.position;
        if (pos.x < -2.5) return;

        Renderer.flipX = true;
        pos.x -= 1;
        transform.position = pos;
    }

    public void MoveRight()
    {
        Vector2 pos = transform.position;
        if (pos.x > 2.5) return;

        Renderer.flipX = false;
        pos.x += 1;
        transform.position = pos;
    }

    public void FixGround()
    {
        Vector2 pos = new Vector2(transform.position.x + 3.5f, transform.position.y + 3f);

        Controller.FixGround(pos);
    }
}
