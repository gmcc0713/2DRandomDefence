using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Monster : MonoBehaviour, IPoolingObject
{
    private GameObject[] m_arrMovePoint;//Path배열 (몬스터의 이동경로 Path)
    private int movePointCount;                  //movePointCount
    private int curMovePointIndex;              //현재 나의 MovePoint 번호
    [SerializeField] private Slider HPBar;                        //체력바

    [SerializeField] private DamageUI damageUI;                        //데미지 받은 값
    private MonsterHPBar monsterHPBar;

    private bool isDie;


    private int dieGold;                                          //현재 나의 MovePoint 번호
    [SerializeField] private float moveSpeed;                    //이동 속도
    [SerializeField] private float defaultHealth;                    //최대 생명력
    private float maxHealth;
    public float curHealth;                                    //현재 생명력
    private Animator monsterAnimator;
    void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        monsterAnimator = gameObject.GetComponent<Animator>();
        monsterHPBar = HPBar.GetComponent<MonsterHPBar>();

        maxHealth = defaultHealth;
        monsterHPBar.HPBarInit(maxHealth);
        moveSpeed = 3;
        curMovePointIndex = 0;                  //현재 나의 MovePoint
        curHealth = maxHealth;
        dieGold = 50;
        isDie = false;
    }
    // Update is called once per frame
    void Update()
    {
       MoveToNextMovePoint();
       monsterHPBar.MonsterHpBarUpdate(new Vector3(transform.position.x, transform.position.y-0.8f,0), curHealth);
       damageUI.MoveWithMonster(new Vector3(transform.position.x, transform.position.y + 0.8f, 0));

    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void SetMonster(GameObject[] movePoint,float healthBuf=1.0f,float speedBuf= 1.0f)
    {
        movePointCount = movePoint.Length;
        m_arrMovePoint = new GameObject[movePointCount];
        m_arrMovePoint = movePoint;
        transform.position = m_arrMovePoint[curMovePointIndex].transform.position;
        curMovePointIndex = 0;                  //현재 나의 MovePoint

        maxHealth = defaultHealth * healthBuf;
        curHealth = maxHealth;
        monsterHPBar.HPBarInit(maxHealth);
        isDie = false;
    }
    void MoveToNextMovePoint()
    {
        if(!isDie)
        {
            transform.position = Vector2.MoveTowards(transform.position, m_arrMovePoint[curMovePointIndex].transform.position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(m_arrMovePoint[curMovePointIndex].transform.position, transform.position) == 0f)                                          
            {
                if (curMovePointIndex < movePointCount - 1)     // 몬스터가 이동할 movePoint를 다 이동하지 않았을때
                {

                    if (m_arrMovePoint[curMovePointIndex].transform.position.x > m_arrMovePoint[++curMovePointIndex].transform.position.x)      //진행방향이 오른쪽일때
                    {
                        transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));   //몬스터 바라보는 방향 회전
                    }
                    else //진행방향이 왼쪽일때
                    {
                        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                    }
                }
                else                //몬스터가 마지막 MovePoint에 도달했을때
                {
                    GameMgr.Instance.GetDamagePlayer((int)this.curHealth);
                    Monsterdie();
                    MonsterSpawner.Instance.DeleteMonster(this);

                    Debug.Log("몬스터 목적지 도달");
                }
            }
        }

    }
    void DieCheck()
    {
        if(curHealth<=0)
        {
            StartCoroutine(DieAniTimer());
        }

    }
    private void OnTriggerEnter2D(Collider2D other)           //총알이 몬스터와 충돌했을때
    {
        if(other.CompareTag("Bullet"))
        {

            float attackDamage = other.gameObject.GetComponent<Bullet>()._attckDamage;
            monsterAnimator.SetTrigger("Hit");
            damageUI.MonsterDamageUpdate(attackDamage);
            Debug.Log(attackDamage);
            curHealth -= attackDamage;
            DieCheck();
        }
    }
    private IEnumerator DieAniTimer()
    {

        monsterAnimator.SetBool("Dead", true);
        isDie = true;
        GameMgr.Instance.GetGold(dieGold);
        UIManager.Instance.SetGoldUI();
        Debug.Log("몬스터 리스트에서 삭제");
        Monsterdie();
        yield return new WaitForSeconds(1.0f);

        monsterAnimator.SetBool("Dead", false);
        MonsterSpawner.Instance.DeleteMonster(this);

    }
    private void Monsterdie()
    {
        damageUI.DamageUIInIt();
        monsterHPBar.HPBarInit(maxHealth);
        MonsterSpawner.Instance.DeleteMonsterInList(this);
        
    }
}