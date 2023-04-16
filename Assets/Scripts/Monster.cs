using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Monster : MonoBehaviour, IPoolingObject
{
    
    private GameObject[] m_arrMovePoint;//Path�迭 (������ �̵���� Path)
    private int movePointCount;                  //movePointCount
    private int curMovePointIndex;              //���� ���� MovePoint ��ȣ

    [SerializeField]private float moveSpeed;                        //�̵� �ӵ�
    [SerializeField]private float maxHealth;                      //�ִ� �����
    private float curHealth;                      //���� �����
    void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        curMovePointIndex = 0;                  //���� ���� MovePoint
        moveSpeed = 3;
        curHealth = maxHealth;
    }
    // Update is called once per frame
    void Update()
    {
       MoveToNextMovePoint();
        DieCheck();
    }
    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
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
        transform.position = Vector2.MoveTowards(transform.position, m_arrMovePoint[curMovePointIndex].transform.position, moveSpeed * Time.deltaTime);
        if(Vector2.Distance(m_arrMovePoint[curMovePointIndex].transform.position, transform.position)==0f)
        {
            if (curMovePointIndex < movePointCount-1)
            {
                    curMovePointIndex++;
            }
        }
    }
    void DieCheck()
    {
        if(curHealth<=0)
        {
            MonsterSpawner.Instance.DeleteMonster(this);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)           //�Ѿ��� ���Ϳ� �浹������
    {
        if(other.CompareTag("Bullet"))
        {
            curHealth -= other.gameObject.GetComponent<Bullet>()._attckDamage;
        }
    }
}