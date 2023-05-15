using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    //===============================================================================
    [SerializeField] private WaveData[] monsterWave;
    [SerializeField] private TextMeshProUGUI stageText;
    [SerializeField] private TextMeshProUGUI waitTimerText;
    private bool isWaveEnd;
    public int waveIndex;
    void Start()
    {
        isWaveEnd = false;
        waveIndex = 0;
        MonsterSpawner.Instance.WaveStart(monsterWave[waveIndex]);
        SetStageText();
    }
    public void EndThisWave()
    {

        if(waveIndex < monsterWave.Length && MonsterSpawner.Instance.MonsterIsEmptyInField())
        {
            StartCoroutine(WaitForNextWave(10));
        }
    }
    IEnumerator WaitForNextWave(int waitSecond)
    {
        for(int i = 9;i>=0;i--)
        {
            
            SetTimerText(i);

            yield return new WaitForSeconds(1);
        }
        waitTimerText.text = "";
        MonsterSpawner.Instance.WaveStart(monsterWave[waveIndex]);
        waveIndex++;
        SetStageText();
    }
    private void SetStageText()
    {
        System.Text.StringBuilder stageString = new System.Text.StringBuilder("Stage : ");
        stageString.Append(monsterWave[waveIndex]._stageData.stage);
        stageString.Append("-");
        stageString.Append(monsterWave[waveIndex]._stageData.wave);
        stageText.text = stageString.ToString();
    }
    private void SetTimerText(int i)
    {
        System.Text.StringBuilder timerString = new System.Text.StringBuilder("남은 시간 : ");
        timerString.Append(i);
        waitTimerText.text = timerString.ToString();
    }
}