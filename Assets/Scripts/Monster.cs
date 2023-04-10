using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Monster : MonoBehaviour
{
    
    private GameObject[] m_arrMovePoint;//Path�迭 (������ �̵���� Path)
    private int movePointCount;                  //movePointCount
    private int curMovePointIndex;              //���� ���� MovePoint ��ȣ

    private float speed;
    void Start()
    {
        curMovePointIndex = 0;                  //���� ���� MovePoint
        speed = 3;
    }

    // Update is called once per frame
    void Update()
    {
       MoveToNextMovePoint();

    }
    public void SetMonster(GameObject[] movePoint)
    {
        movePointCount = movePoint.Length;
        m_arrMovePoint = new GameObject[movePointCount];
        m_arrMovePoint = movePoint;
        transform.position = m_arrMovePoint[curMovePointIndex].transform.position;
    }
    void MoveToNextMovePoint()
    {
        transform.position = Vector2.MoveTowards(transform.position, m_arrMovePoint[curMovePointIndex].transform.position, speed * Time.deltaTime);
        if(Vector2.Distance(m_arrMovePoint[curMovePointIndex].transform.position, transform.position)==0f)
        {
            if(curMovePointIndex < movePointCount-1)
            {
                    curMovePointIndex++;
            }
        }
    }
    void Die()
    {
        MonsterSpawner.Instance.DeleteMonster(this);
    }
}