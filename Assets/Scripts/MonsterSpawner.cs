using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    [SerializeField]
    private GameObject MonsterPrefab;
    [SerializeField] 
    MapPath[] path;            //Path배열 (몬스터의 이동경로 Path)
    private int pathIndex = 1;
    public List<Monster> monsterList;
    void Start()
    {
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
            GameObject clone = Instantiate(MonsterPrefab);              //해당 몬스터 복제
            Monster monster = clone.GetComponent<Monster>();
            monsterList.Add(monster);
            monster.SetMonster(path[pathIndex].m_arrMovePoint);       //해당 몬스터에 MovePoint 전달

            if (pathIndex == 0)
                pathIndex = 1;
            else
                pathIndex = 0;
            yield return new WaitForSeconds(4f);

        }
    }
    public void DeleteMonster(Monster monster)
    {
        monsterList.Remove(monster);
        Destroy(monster.gameObject);
    }
}
