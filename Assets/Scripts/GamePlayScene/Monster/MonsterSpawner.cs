using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public enum MonsterType { monster_1 = 0, monster_2, monster_3, monster_4, monster_5 }
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

    [SerializeField] private MapPath path;            //Path�迭 (������ �̵���� Path)
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
        int curMonsterCount = 0;                                //���� ���̺��� curMonIndex ��°�� ���� ���° ��ȯ����(ex)Monster_1�� ���Ͱ� 4��° ��ȯ��)
        int curMonIndex = 0;                                    //���� ���̺��� ���� ���° ��������(ex)Monster_1���� Monster_2����)
        thisWaveSpawnEnd = false;
        Monster_Data[] monWaveData = WaveData._monArr;          //���° ���Ͱ� ��� ��ȯ�Ǿ�� �ϴ��� ����Ǿ��ִ� �迭(ex)Monster_1�� 5����, Monster_2�� 1����)

        Monster_Data curMonster = monWaveData[curMonIndex];     //���� ��ȯ���� ���� ������(Monster_1�� 5���� ��ȯ�Ǿ�� �Ѵ�.)

        while (true)
        {
            yield return new WaitForSeconds(2.0f);
            Monster cloneMonster;

            if (monsterPool[curMonster.monsterTypeIndex].GetObject(out cloneMonster))   //Ǯ���� ����������
            {
                monsterList.Add(cloneMonster);                        //�ش� ���� ����Ʈ�� �߰�
                cloneMonster.Initialize();                           // �ش� ���� �ʱ�ȭ
                cloneMonster.SetMonster(path.m_arrMovePoint, WaveData._stageData.monHealthBuf, WaveData._stageData.monSpeedBuf);       //�ش� ���Ϳ� MovePoint ����
                curMonsterCount++;                                         //���� ������ ���Ͱ��� 1 ����
                if (curMonsterCount >= curMonster.monsterCount)           //�ش� ���̺��� �ش� ���Ͱ� �������� �������ͷ� �Ѿ�� ���� ����
                {
                    curMonsterCount = 0;                                 //���� ���̺���  curMonIndex ��° ���� 0���� �ʱ�ȭ 
                    if (curMonIndex >= monWaveData.Length-1)                 //�ش� ���̺꿡 ������ ��� ���Ͱ� ��������
                    {
                        thisWaveSpawnEnd = true;                        //�̹� ���̺� ����
                        curMonIndex = 0;                                //���� �����ε��� �ʱ�ȭ
                        yield break;
                    }
                    curMonIndex++;                                      //������° ���� ��ȯ�� ���� �ε��� ����
                    curMonster = monWaveData[curMonIndex];              //������͸� �������ͷ� ����
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
