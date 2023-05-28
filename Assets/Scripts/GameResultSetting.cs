using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public enum GameResult_Type {GameOver=0,StageClear }
public class GameResultSetting : MonoBehaviour
{
    private Image fadeImage;
    [SerializeField]private GameObject resultPanel;
    [SerializeField]private TextMeshProUGUI resultText;

    private float time, fadeTime, start, end;

    void Start()
    {
        this.gameObject.SetActive(true);
        fadeImage = GetComponent<Image>();
        Init();
        this.gameObject.SetActive(false);
        resultPanel.SetActive(false);

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
        gameObject.SetActive(true);
        StartCoroutine(FadeIn());
    }
    public void CallFadeOut(GameResult_Type type)
    {
        gameObject.SetActive(true);
        StartCoroutine(FadeOut(type));
    }

    private IEnumerator FadeOut(GameResult_Type type)
    {
        this.gameObject.SetActive(true);
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
        PlayGameResultAnimation(type);
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
    private void PlayGameResultAnimation(GameResult_Type type)
    {
        resultPanel.SetActive(true);
        switch (type)
        {
            case GameResult_Type.GameOver:
                StartCoroutine(GameOverAnimation());
                break;
            case GameResult_Type.StageClear:
                StartCoroutine(GameWinAnimation());
                break;
        }
    }
    public IEnumerator GameOverAnimation()
    {
        Time.timeScale = 0;
        Debug.Log("���ӿ��� �ִϸ��̼� �ڷ�ƾ ����");
        resultText.text = "���� ����";
        yield return null;
    }
    public IEnumerator GameWinAnimation()
    {
        Debug.Log("���ӽ¸� �ִϸ��̼� �ڷ�ƾ ����");
        resultText.text = "�������� Ŭ����";
        yield return null;
    }
}