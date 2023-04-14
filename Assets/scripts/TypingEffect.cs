using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypingEffect : MonoBehaviour
{
    public string text;
    public Text[] tx;
    public float typingSpeed;
    void Start()
    {
        StartCoroutine(_typing());
    }

    IEnumerator _typing()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i <= text.Length; i++)
        {
            foreach (Text t in tx)
            {
                t.text = text.Substring(0, i);
            }

            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
