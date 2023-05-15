using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType { monster_1 = 0, monster_2, monster_3, }
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

    List<GameObject> m_Monster;
    [SerializeField] private GameObject MonsterPrefab;
    [SerializeField] private MapPath path;            //Path배열 (몬스터의 이동경로 Path)
    public List<Monster> monsterList;
    [SerializeField] public ObjectPool<Monster>[] monsterPool;
    private bool thisWaveSpawnEnd = false;

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
        StartCoroutine(SpawnMonster(monWaveData));
    }

    // Update is called once per frame
    IEnumerator SpawnMonster(WaveData WaveData)
    {
        int curMonsterCount = 0;                                //현재 웨이브의 curMonIndex 번째의 몬스터 몇번째 소환인지
        int curMonIndex = 0;                                    //현재 웨이브의 몬스터 몇번째 몬스터인지
        thisWaveSpawnEnd = false;
        Monster_Data[] monWaveData = WaveData._monArr;

        Monster_Data curMonster = monWaveData[curMonIndex];
        
        while (true)
        {
            yield return new WaitForSeconds(4.5f);
            Monster cloneMonster;
            if (monsterPool[curMonster.monsterTypeIndex].GetObject(out cloneMonster))   //풀링에 성공했을때
            {
                monsterList.Add(cloneMonster);                        //해당 몬스터 리스트에 추가
                cloneMonster.Initialize();                           // 해당 몬스터 초기화
                cloneMonster.SetMonster(path.m_arrMovePoint, WaveData._stageData.monHealthBuf, WaveData._stageData.monSpeedBuf);       //해당 몬스터에 MovePoint 전달
                curMonsterCount++;                                         //몬스터 생성후 몬스터갯수 1 증가
                if (curMonsterCount >= curMonster.monsterCount)           //해당 웨이브의 해당 몬스터가 끝났을때 다음몬스터로 넘어가기 위한 조건
                {
                    Debug.Log("해당 웨이브 몬스터 생성 종료");
                    curMonsterCount = 0;                                 //현재 웨이브의  curMonIndex 번째 몬스터 0으로 초기화 
                    if (curMonIndex >= monWaveData.Length-1)                 //해당 웨이브에 나오는 모든 몬스터가 끝났을때
                    {
                        WaveManager.Instance.EndThisWave();
                        thisWaveSpawnEnd = true;
                        curMonIndex = 0;
                        yield break;
                    }
                    curMonIndex++;
                    curMonster = monWaveData[curMonIndex];

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
        monsterPool[0].PutInPool(monster);
    }
    public void DeleteMonsterInList(Monster monster)
    {
        monsterList.Remove(monster);
        if(thisWaveSpawnEnd)
        {
            WaveManager.Instance.EndThisWave();
        }
    }
}
