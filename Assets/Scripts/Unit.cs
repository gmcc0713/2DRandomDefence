using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Attack_Type { Type_CloseAttack = 0, Type_LongRangeAttack,Type_Count}
public class Unit : MonoBehaviour,IPoolingObject
{
    [SerializeField] public UnitType type;              //���� Ÿ��
    [SerializeField] private float attackRange;         //���ݹ���
    [SerializeField] private float defaultDamage;       //�⺻���ݷ�                      
    [SerializeField] private float attackSpeed;         //���ݼӵ�
    [SerializeField] private Attack_Type attackType;    //���� Ÿ��(���Ÿ�, �ٰŸ�)

    [SerializeField] private int unitGold;              //���� ����
    [SerializeField] private GameObject unitImage;      //���� �̹���
    [SerializeField] private BulletSpawner bulletSpawner;//�Ѿ˻���
    [SerializeField] private Animator playerAnim;       //���� �ִϸ��̼�

    private Vector2 unitBackImagePos;
    private Coroutine attackMonCor;
    private Monster targetMonster;

    public float _attckDamage => defaultDamage + defaultDamage*0.1f*UpgradeManager.Instance._UpgradeType[(int)attackType]; // �⺻ ���ݷ� + ���׷��̵� ���ݷ� �޾ƿ���
    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void SettingMoveImageWithMouse()                     //�巡�� �����϶� �̹����� ���콺 ���󰡵��� �ϱ��� ���� �Լ�
    {
        unitBackImagePos = unitImage.transform.position;
        unitImage.GetComponent<SpriteRenderer>().sortingOrder++;

    }
    public void BackToImagePosition()                               //�̹����� ���� ��ġ�� �ǵ��� �����Լ�
    {
        unitImage.transform.position = unitBackImagePos;
        unitImage.GetComponent<SpriteRenderer>().sortingOrder--;

    }
    public void MoveImageWithMouse()                                     //�巡�� �����϶� �̹����� ���콺 ���󰡵��� �ϴ� �Լ�
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        unitImage.transform.position = pos;
    }

    void Start()
    {
        
        bulletSpawner = transform.Find("BulletSpawner").GetComponent<BulletSpawner>();
        
        unitBackImagePos = transform.position;
        if (attackType == Attack_Type.Type_LongRangeAttack)     //���Ÿ� ���� ��� ����
        {
            playerAnim.SetFloat("NormalState", 0.5f);
        }
        else                                                    //�ٰŸ� ���� ��� ����
        {
            playerAnim.SetFloat("NormalState", 0.0f);
        }

        StartCoroutine(CheakNearMonster());                      //�ֺ� ���� Ž�� ����

    }
    void UnitDie()                                      //������ ����������
    {
        StopCoroutine(CheakNearMonster());              //Ž�� ����
    }
    void UnitSetInit()                                  //���� �����Ǿ����� Ž�� ����
    {
        StartCoroutine(CheakNearMonster());
    }
    private IEnumerator CheakNearMonster() //��ó�� ���� �ִ��� Ȯ��
    {

        while (true)
        {
            float nearDistance = Mathf.Infinity;
            int monsterCount = MonsterSpawner.Instance.monsterList.Count;
            Vector2 directionVector;
            for (int i = 0; i < monsterCount; i++)
            {
                //���Ϳ� ���ֻ����� �Ÿ�
                directionVector = MonsterSpawner.Instance.monsterList[i].transform.position - transform.position;
                float monsterDistance = Vector2.Distance(MonsterSpawner.Instance.monsterList[i].transform.position, transform.position);
                
                //���� ���� �Ÿ��� ���� ��Ÿ����� ª���鼭 ���� ����� ���� Ž��
                if (monsterDistance <= attackRange && monsterDistance <= nearDistance && MonsterSpawner.Instance.monsterList[i].gameObject.activeSelf)
                {
                    nearDistance = monsterDistance;
                    targetMonster = MonsterSpawner.Instance.monsterList[i];     //Ÿ�ٸ��ͷ� ����
                    if(directionVector.x>=0)
                    {
                        gameObject.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                    }
                    else
                    {
                        gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    }
                }
            }

            if (targetMonster != null)          //Ÿ�ٸ��Ͱ� ������
            {

                if (attackMonCor == null)       //�ڷ�ƾ�� �������� �ƴҶ�
                {
                    attackMonCor = StartCoroutine(AttackMonster());     //�ڷ�ƾ ����
                }

            }
            else                                //Ÿ�ٸ��Ͱ� ������
            {
                StopCoroutine(AttackMonster()); //�ڷ�ƾ ����
                attackMonCor = null;
            }
            yield return null;
            targetMonster = null;
        }
    }
    private IEnumerator AttackMonster()            //���� ���� �ڷ�ƾ
    {
        yield return new WaitForSeconds(0.1f);
        while (true)
        {
            if (targetMonster == null)
            {
                break;
            }

            float monsterDistance = Vector2.Distance(targetMonster.transform.position, transform.position);
            if (monsterDistance > attackRange)
            {
                targetMonster = null;
                break;
            }
            bulletSpawner.SpawnBullets(transform.position, targetMonster);

            playerAnim.SetTrigger("Attack");

            yield return new WaitForSeconds(attackSpeed);
        }
    }
}
