using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixEffect : MonoBehaviour
{
    float tick = 0.7f;
    SpriteRenderer Renderer;

    private void Start()
    {
        Renderer = transform.GetComponent<SpriteRenderer>();
        StartCoroutine(scaling());
    }

    IEnumerator scaling()
    {
        for (int scale = 1; scale < 10f; scale += 2)
        {
            transform.localScale = new Vector2 (scale, scale);
            yield return new WaitForSeconds(0.01f);
        }
    }

    private void FixedUpdate()
    {
        tick -= 0.02f;
        Color color = Renderer.color;
        color.a = tick;

        Renderer.color = color;
        if (tick < 0) Destroy(this.gameObject);
    }
}
