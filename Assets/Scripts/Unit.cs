using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Attack_Type { Type_CloseAttack = 0, Type_LongRangeAttack,Type_Count}
public class Unit : MonoBehaviour,IPoolingObject
{
    [SerializeField] public UnitType type;              //유닛 타입
    [SerializeField] private float attackRange;         //공격범위
    [SerializeField] private float defaultDamage;       //기본공격력                      
    [SerializeField] private float attackSpeed;         //공격속도
    [SerializeField] private Attack_Type attackType;    //공격 타입(원거리, 근거리)

    [SerializeField] private int unitGold;              //유닛 가격
    [SerializeField] private GameObject unitImage;      //유닛 이미지
    [SerializeField] private BulletSpawner bulletSpawner;//총알생성
    [SerializeField] private Animator playerAnim;       //유닛 애니메이션

    private Vector2 unitBackImagePos;
    private Coroutine attackMonCor;
    private Monster targetMonster;

    public float _attckDamage => defaultDamage + defaultDamage*0.1f*UpgradeManager.Instance._UpgradeType[(int)attackType]; // 기본 공격력 + 업그레이드 공격력 받아오기
    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void SettingMoveImageWithMouse()                     //드래그 상태일때 이미지가 마우스 따라가도록 하기전 세팅 함수
    {
        unitBackImagePos = unitImage.transform.position;
        unitImage.GetComponent<SpriteRenderer>().sortingOrder++;

    }
    public void BackToImagePosition()                               //이미지가 원래 위치로 되돌아 가는함수
    {
        unitImage.transform.position = unitBackImagePos;
        unitImage.GetComponent<SpriteRenderer>().sortingOrder--;

    }
    public void MoveImageWithMouse()                                     //드래그 상태일때 이미지가 마우스 따라가도록 하는 함수
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        unitImage.transform.position = pos;
    }

    void Start()
    {
        
        bulletSpawner = transform.Find("BulletSpawner").GetComponent<BulletSpawner>();
        
        unitBackImagePos = transform.position;
        if (attackType == Attack_Type.Type_LongRangeAttack)     //원거리 공격 모션 설정
        {
            playerAnim.SetFloat("NormalState", 0.5f);
        }
        else                                                    //근거리 공격 모션 설정
        {
            playerAnim.SetFloat("NormalState", 0.0f);
        }

        StartCoroutine(CheakNearMonster());                      //주변 몬스터 탐색 시작

    }
    void UnitDie()                                      //유닛이 없어졌을때
    {
        StopCoroutine(CheakNearMonster());              //탐색 종료
    }
    void UnitSetInit()                                  //유닛 생성되었을때 탐색 시작
    {
        StartCoroutine(CheakNearMonster());
    }
    private IEnumerator CheakNearMonster() //근처에 적이 있는지 확인
    {

        while (true)
        {
            float nearDistance = Mathf.Infinity;
            int monsterCount = MonsterSpawner.Instance.monsterList.Count;
            Vector2 directionVector;
            for (int i = 0; i < monsterCount; i++)
            {
                //몬스터와 유닛사이의 거리
                directionVector = MonsterSpawner.Instance.monsterList[i].transform.position - transform.position;
                float monsterDistance = Vector2.Distance(MonsterSpawner.Instance.monsterList[i].transform.position, transform.position);
                
                //유닛 과의 거리가 공격 사거리보다 짧으면서 가장 가까운 몬스터 탐색
                if (monsterDistance <= attackRange && monsterDistance <= nearDistance && MonsterSpawner.Instance.monsterList[i].gameObject.activeSelf)
                {
                    nearDistance = monsterDistance;
                    targetMonster = MonsterSpawner.Instance.monsterList[i];     //타겟몬스터로 설정
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

            if (targetMonster != null)          //타겟몬스터가 있을때
            {

                if (attackMonCor == null)       //코루틴이 실행중이 아닐때
                {
                    attackMonCor = StartCoroutine(AttackMonster());     //코루틴 실행
                }

            }
            else                                //타겟몬스터가 없으면
            {
                StopCoroutine(AttackMonster()); //코루틴 종료
                attackMonCor = null;
            }
            yield return null;
            targetMonster = null;
        }
    }
    private IEnumerator AttackMonster()            //몬스터 공격 코루틴
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
