using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    [SerializeField]
    private GameObject MonsterPrefab;
    [SerializeField] 
    MapPath[] path;            //Path�迭 (������ �̵���� Path)
    private int pathIndex = 0;
    public List<Monster> monsterList;
    [SerializeField] private ObjectPool<Monster>[] monsterPool;
    void Start()
    {
        for (int i = 0; i < monsterPool.Length; i++)
        {
            monsterPool[i].Initialize();
        }
        monsterList = new List<Monster>();
        StartCoroutine(SpawnMonster());
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator SpawnMonster()
    {
        while (true)
        {
            Monster cloneMonster;
            if(monsterPool[0].GetObject(out cloneMonster))
            {
                monsterList.Add(cloneMonster);
                cloneMonster.Initialize();
                cloneMonster.SetMonster(path[pathIndex].m_arrMovePoint);       //�ش� ���Ϳ� MovePoint ����
            }
            yield return new WaitForSeconds(4f);
            break;
        }
    }
    public void DeleteMonster(Monster monster)
    {
        monsterList.Remove(monster);
        monsterPool[0].PutInPool(monster);
    }
}
