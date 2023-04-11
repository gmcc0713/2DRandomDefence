using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] public UnitType type;
    public float attackRange;
    public float attackDamage;
    public float attackSpeed;

    [SerializeField] private GameObject unitImage;
    private Vector2 unitBackImagePos;

    private BulletSpawner bS;
    private Animator playerAnim;

    private Coroutine attackMonCor;
    private bool isAttackType;
    private Monster targetMonster;

    /*    private int unitNumber = -1;
        public int _unitNumber
        {
            get { return unitNumber; }
            set { unitNumber = value; }
        }
    */
    public void SettingMoveImageWithMouse()
    {
        unitBackImagePos = unitImage.transform.position;
        unitImage.GetComponent<SpriteRenderer>().sortingOrder++;
    }
    public void BackToImagePosition()
    {
        unitImage.transform.position = unitBackImagePos;
        unitImage.GetComponent<SpriteRenderer>().sortingOrder--;
    }
    public void MoveImageWithMouse()
    {
        unitImage.transform.position = Input.mousePosition;
    }
    void Start()
    {
        bS = GetComponent<BulletSpawner>();
        unitBackImagePos = transform.position;
/*       StartCoroutine(CheakNearMonster());*/
        //playerAnim = GetComponent<Animator>();
    }

 /*   private IEnumerator CheakNearMonster() //근처에 적이 있는지 확인
    {
        while (true)
        {
            float nearDistance = Mathf.Infinity;
            int monsterCount = MonsterSpawner.Instance.monsterList.Count;
            for (int i = 0; i < monsterCount; i++)
            {
                //몬스터와 유닛사이의 거리
                float monsterDistance = Vector2.Distance(MonsterSpawner.Instance.monsterList[i].transform.position, transform.position);
                if (monsterDistance <= attackRange && monsterDistance <= nearDistance)
                {
                    nearDistance = monsterDistance;
                    targetMonster = MonsterSpawner.Instance.monsterList[i];
                }
            }

            if (targetMonster != null)
            {
                if (attackMonCor == null)
                {
                    isAttackType = true;

                    attackMonCor = StartCoroutine(AttackMonster());
                }

            }
            else
            {
                isAttackType = false;
                StopCoroutine(AttackMonster());
                attackMonCor = null;
            }
            yield return null;
        }

    }
    private IEnumerator AttackMonster()
    {
        while (true)
        {
            if (targetMonster == null)
            {
                isAttackType = false;
                break;
            }

            float monsterDistance = Vector2.Distance(targetMonster.transform.position, transform.position);
            if (monsterDistance > attackRange)
            {
                targetMonster = null;
                isAttackType = false;
                break;
            }
            bS.SpawnBullets(transform.position, targetMonster);
            //playerAnim.SetTrigger("IsAttack");
            yield return new WaitForSeconds(attackSpeed);
        }


    }*/
}
