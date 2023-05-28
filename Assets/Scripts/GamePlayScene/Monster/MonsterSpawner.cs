using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public enum MonsterType { monster_1 = 0, monster_2, monster_3, monster_4, monster_5 }
[System.Serializable]
public struct MapPath   //MovePoint 여러개일때를 위한 구조체
{
    public GameObject[] m_arrMovePoint;
}
public class MonsterSpawner : MonoBehaviour
{
    public static MonsterSpawner Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    //===============================================================================

    [SerializeField] private MapPath path;            //Path배열 (몬스터의 이동경로 Path)
    public List<Monster> monsterList;
    [SerializeField] public ObjectPool<Monster>[] monsterPool;
    private bool thisWaveSpawnEnd = false;
    private Coroutine spawnMonsterCor;
    private int remainingMonsterCount;
    public int _remainingMonsterCount => remainingMonsterCount;
    void Start()
    {
        for (int i = 0; i < monsterPool.Length; i++)
        {
            monsterPool[i].Initialize();
        }

        monsterList = new List<Monster>();

    }

    public void WaveStart(WaveData monWaveData)
    {
        if(spawnMonsterCor == null)
        {
            remainingMonsterCount = WaveManager.Instance._thisWaveMonsterCount;
            UIManager.Instance.SetMonsterCountUI();
            spawnMonsterCor = StartCoroutine(SpawnMonster(monWaveData));
        }
    }

    // Update is called once per frame
    IEnumerator SpawnMonster(WaveData WaveData)
    {
        int curMonsterCount = 0;                                //현재 웨이브의 curMonIndex 번째의 몬스터 몇번째 소환인지(ex)Monster_1의 몬스터가 4번째 소환됨)
        int curMonIndex = 0;                                    //현재 웨이브의 몬스터 몇번째 몬스터인지(ex)Monster_1인지 Monster_2인지)
        thisWaveSpawnEnd = false;
        Monster_Data[] monWaveData = WaveData._monArr;          //몇번째 몬스터가 몇마리 소환되어야 하는지 저장되어있는 배열(ex)Monster_1이 5마리, Monster_2가 1마리)

        Monster_Data curMonster = monWaveData[curMonIndex];     //현재 소환중인 몬스터 데이터(Monster_1이 5마리 소환되어야 한다.)

        while (true)
        {
            yield return new WaitForSeconds(2.0f);
            Monster cloneMonster;

            if (monsterPool[curMonster.monsterTypeIndex].GetObject(out cloneMonster))   //풀링에 성공했을때
            {
                monsterList.Add(cloneMonster);                        //해당 몬스터 리스트에 추가
                cloneMonster.Initialize();                           // 해당 몬스터 초기화
                cloneMonster.SetMonster(path.m_arrMovePoint, WaveData._stageData.monHealthBuf, WaveData._stageData.monSpeedBuf);       //해당 몬스터에 MovePoint 전달
                curMonsterCount++;                                         //몬스터 생성후 몬스터갯수 1 증가
                if (curMonsterCount >= curMonster.monsterCount)           //해당 웨이브의 해당 몬스터가 끝났을때 다음몬스터로 넘어가기 위한 조건
                {
                    curMonsterCount = 0;                                 //현재 웨이브의  curMonIndex 번째 몬스터 0으로 초기화 
                    if (curMonIndex >= monWaveData.Length-1)                 //해당 웨이브에 나오는 모든 몬스터가 끝났을때
                    {
                        thisWaveSpawnEnd = true;                        //이번 웨이브 종료
                        curMonIndex = 0;                                //현재 몬스터인덱스 초기화
                        yield break;
                    }
                    curMonIndex++;                                      //다음번째 몬스터 소환을 위한 인덱스 증가
                    curMonster = monWaveData[curMonIndex];              //현재몬스터를 다음몬스터로 설정
                }
            }
        }

    }
    public bool MonsterIsEmptyInField()
    {
        if (monsterList.Count == 0)
        {
            return true;
        }
        return false;
    }
    public void DeleteMonster(Monster monster)              //몬스터가 죽거나 끝에 도달했을때
    {
        monsterPool[(int)monster._monsterType].PutInPool(monster);

    }
    public void DeleteMonsterInList(Monster monster)
    {
        monsterList.Remove(monster);
        remainingMonsterCount--;
        UIManager.Instance.SetMonsterCountUI();
        if (thisWaveSpawnEnd && MonsterIsEmptyInField() && spawnMonsterCor!=null)
        {
            spawnMonsterCor = null;
            WaveManager.Instance.EndThisWave();
        }
    }
}
