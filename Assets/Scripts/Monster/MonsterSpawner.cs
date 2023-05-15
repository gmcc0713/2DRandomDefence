using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType { monster_1 = 0, monster_2, monster_3, }
[System.Serializable]
public struct MapPath   //MovePoint �������϶��� ���� ����ü
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
    [SerializeField] private MapPath path;            //Path�迭 (������ �̵���� Path)
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
        int curMonsterCount = 0;                                //���� ���̺��� curMonIndex ��°�� ���� ���° ��ȯ����
        int curMonIndex = 0;                                    //���� ���̺��� ���� ���° ��������
        thisWaveSpawnEnd = false;
        Monster_Data[] monWaveData = WaveData._monArr;

        Monster_Data curMonster = monWaveData[curMonIndex];
        
        while (true)
        {
            yield return new WaitForSeconds(4.5f);
            Monster cloneMonster;
            if (monsterPool[curMonster.monsterTypeIndex].GetObject(out cloneMonster))   //Ǯ���� ����������
            {
                monsterList.Add(cloneMonster);                        //�ش� ���� ����Ʈ�� �߰�
                cloneMonster.Initialize();                           // �ش� ���� �ʱ�ȭ
                cloneMonster.SetMonster(path.m_arrMovePoint, WaveData._stageData.monHealthBuf, WaveData._stageData.monSpeedBuf);       //�ش� ���Ϳ� MovePoint ����
                curMonsterCount++;                                         //���� ������ ���Ͱ��� 1 ����
                if (curMonsterCount >= curMonster.monsterCount)           //�ش� ���̺��� �ش� ���Ͱ� �������� �������ͷ� �Ѿ�� ���� ����
                {
                    Debug.Log("�ش� ���̺� ���� ���� ����");
                    curMonsterCount = 0;                                 //���� ���̺���  curMonIndex ��° ���� 0���� �ʱ�ȭ 
                    if (curMonIndex >= monWaveData.Length-1)                 //�ش� ���̺꿡 ������ ��� ���Ͱ� ��������
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
    public void DeleteMonster(Monster monster)              //���Ͱ� �װų� ���� ����������
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
