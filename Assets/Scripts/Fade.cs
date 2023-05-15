using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{

    private Image fadeImage;
    private float time, fadeTime, start, end;

    void Start()
    {
        this.gameObject.SetActive(true);
        fadeImage = GetComponent<Image>();
        Init();
        this.gameObject.SetActive(false);
    }
    void Update()
    {
     
    }
    private void Init()
    {
        time = 0;
        fadeTime = 1;

        start = 0;
        end = 1;
    }

    public void CallFadeIn()
    {
        StartCoroutine(FadeIn());
    }
    public void CallFadeOut()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        Init();
        Color color = fadeImage.color;
        color.a = 0;
        fadeImage.color = color;
        while (color.a < end)
        {
            time += Time.deltaTime / fadeTime;
            color.a = Mathf.Lerp(start, end, time);
            fadeImage.color = color;
            yield return null;
        }
        
        yield return null;
    }

    private IEnumerator FadeIn()
    {
        Init();
        Color color = fadeImage.color;
        color.a = 1;
        fadeImage.color = color;

        while (color.a > start)
        {
            time += Time.deltaTime / fadeTime;
            color.a = Mathf.Lerp(end, start, time);
            fadeImage.color = color;
            yield return null;
        }
        yield return null;
    }
}