using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Monster : MonoBehaviour, IPoolingObject
{
    private GameObject[] m_arrMovePoint;//Path�迭 (������ �̵���� Path)
    private int movePointCount;                  //movePointCount
    private int curMovePointIndex;              //���� ���� MovePoint ��ȣ
    [SerializeField] private Slider HPBar;                        //ü�¹�

    [SerializeField] private DamageUI damageUI;                        //������ ���� ��
    private MonsterHPBar monsterHPBar;

    private bool isDie;


    private int dieGold;                                          //���� ���� MovePoint ��ȣ
    [SerializeField] private float moveSpeed;                    //�̵� �ӵ�
    [SerializeField] private float defaultHealth;                    //�ִ� �����
    private float maxHealth;
    public float curHealth;                                    //���� �����
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
        curMovePointIndex = 0;                  //���� ���� MovePoint
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
        curMovePointIndex = 0;                  //���� ���� MovePoint

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
                if (curMovePointIndex < movePointCount - 1)     // ���Ͱ� �̵��� movePoint�� �� �̵����� �ʾ�����
                {

                    if (m_arrMovePoint[curMovePointIndex].transform.position.x > m_arrMovePoint[++curMovePointIndex].transform.position.x)      //��������� �������϶�
                    {
                        transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));   //���� �ٶ󺸴� ���� ȸ��
                    }
                    else //��������� �����϶�
                    {
                        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                    }
                }
                else                //���Ͱ� ������ MovePoint�� ����������
                {
                    GameMgr.Instance.GetDamagePlayer((int)this.curHealth);
                    Monsterdie();
                    MonsterSpawner.Instance.DeleteMonster(this);

                    Debug.Log("���� ������ ����");
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
    private void OnTriggerEnter2D(Collider2D other)           //�Ѿ��� ���Ϳ� �浹������
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
        Debug.Log("���� ����Ʈ���� ����");
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