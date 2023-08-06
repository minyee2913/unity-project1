using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer Renderer;
    private GameController Controller;

    private AudioSource click;
    private AudioSource check;
    public GameObject FixParticle;
    public GameObject[] FixEffect;

    private void Start() 
    {
        Renderer = GetComponent<SpriteRenderer>();
        Controller = GameObject.Find("GameController").GetComponent<GameController>();
        click = GameObject.Find("click").GetComponent<AudioSource>();
        check = GameObject.Find("check").GetComponent<AudioSource>();
    }
    public void MoveUp()
    {
        Vector2 pos = transform.position;
        if (pos.y > 1) return;

        pos.y += 1;
        transform.position = pos;

        click.Play();
    }

    public void MoveDown()
    {
        Vector2 pos = transform.position;
        if (pos.y < -1) return;

        pos.y -= 1;
        transform.position = pos;

        click.Play();
    }

    public void MoveLeft()
    {
        Vector2 pos = transform.position;
        if (pos.x < -2.5) return;

        Renderer.flipX = true;
        pos.x -= 1;
        transform.position = pos;

        click.Play();
    }

    public void MoveRight()
    {
        Vector2 pos = transform.position;
        if (pos.x > 2.5) return;

        Renderer.flipX = false;
        pos.x += 1;
        transform.position = pos;

        click.Play();
    }

    public void FixGround()
    {
        Vector2 pos = new Vector2(transform.position.x + 3.5f, transform.position.y + 3f);

        bool fix = Controller.FixGround(pos);

        if (fix)
        {
            var pt = FixEffect[Random.Range(0, FixEffect.Length-1)];
            GameObject particle = Instantiate(FixParticle, transform.position, transform.rotation);
            Instantiate(pt, new Vector2(transform.position.x, transform.position.y - 0.3f), transform.rotation);
            Destroy(particle, 0.5f);
            check.Play();
        }
        else click.Play();
    }
}
