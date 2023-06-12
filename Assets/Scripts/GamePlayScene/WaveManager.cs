using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    //===============================================================================
    [SerializeField] private WaveData[] monsterWave;            //몬스터 스테이지,웨이브 배열
    [SerializeField] private TextMeshProUGUI stageText;
    [SerializeField] private TextMeshProUGUI waitTimerText;
    [SerializeField] ParticleSystem monsterSpawnParticle;

    private int waveIndex = 0;
    public int _waveIndex => waveIndex;
    private int thisWaveMonsterCount;
    public int _thisWaveMonsterCount => thisWaveMonsterCount;
    void Start()
    {
        SetStageText();
        SetMonsterCounter();
        MonsterSpawner.Instance.WaveStart(monsterWave[waveIndex]);

    }
    public void EndThisWave()
    {
        if( monsterWave[waveIndex]._stageData.wave == 5 && !GameMgr.Instance._isGameEnd)
        {
            GameMgr.Instance._gameResultImage.GetComponent<GameResultSetting>().CallFadeOut(GameResult_Type.StageClear);
            SoundManager.Instance.PlayAudioClipOneShot(Sound_Type.Sound_SFX, (int)SFX_Num.Win);
            SoundManager.Instance.StopBGM();
            PlayerPrefs.SetInt("MaxClearStage", monsterWave[waveIndex]._stageData.stage);
        }
        else if(MonsterSpawner.Instance.MonsterIsEmptyInField() && !GameMgr.Instance._isGameEnd)
        {
            StartCoroutine(WaitForNextWave(10));
        }
    }
    IEnumerator WaitForNextWave(int waitSecond)
    {
        monsterSpawnParticle.Stop();
        for (int i = 9;i>=0;i--)
        {
            SetTimerText(i);

            yield return new WaitForSeconds(1);
        }

        waitTimerText.text = "";

        waveIndex++;
        SetStageText();
        SetMonsterCounter();
        MonsterSpawner.Instance.WaveStart(monsterWave[waveIndex]);
        monsterSpawnParticle.Play();
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
        SoundManager.Instance.PlayAudioClipOneShot(Sound_Type.Sound_SFX, (int)SFX_Num.CountDown);

        System.Text.StringBuilder timerString = new System.Text.StringBuilder("남은 시간 : ");
        timerString.Append(i);
        waitTimerText.text = timerString.ToString();
    }
    public void SceneLoad()
    {
        waveIndex = PlayerPrefs.GetInt("SelectStage") * 5;
    }
    public void SetMonsterCounter()
    {
        thisWaveMonsterCount = 0;
        for (int i = 0; i < monsterWave[waveIndex]._monArr.Length; i++)
        {
            thisWaveMonsterCount += monsterWave[waveIndex]._monArr[i].monsterCount;
        }
    }

}