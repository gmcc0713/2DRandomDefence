using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Unit : MonoBehaviour,IPoolingObject
{
    [SerializeField] public UnitType type;
    public float attackRange;
    public float attackDamage;
    public float attackSpeed;


    [SerializeField] private GameObject unitImage;
    private Vector2 unitBackImagePos;
    [SerializeField] private BulletSpawner bulletSpawner;
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
    public float _attckDamage => attackDamage;
    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
        
    }

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
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        unitImage.transform.position = pos;
    }

    void Start()
    {
        bulletSpawner = transform.Find("BulletSpawner").GetComponent<BulletSpawner>();
        
        unitBackImagePos = transform.position;
        StartCoroutine(CheakNearMonster());
        //playerAnim = GetComponent<Animator>();
    }

    private IEnumerator CheakNearMonster() //근처에 적이 있는지 확인
    {
        while (true)
        {
            float nearDistance = Mathf.Infinity;
            int monsterCount = MonsterSpawner.Instance.monsterList.Count;
            for (int i = 0; i < monsterCount; i++)
            {
                //몬스터와 유닛사이의 거리
                float monsterDistance = Vector2.Distance(MonsterSpawner.Instance.monsterList[i].transform.position, transform.position);
                if (monsterDistance <= attackRange && monsterDistance <= nearDistance && MonsterSpawner.Instance.monsterList[i].gameObject.activeSelf)
                {
                    Debug.Log(MonsterSpawner.Instance.monsterList[i].gameObject.activeSelf);
                    nearDistance = monsterDistance;
                    targetMonster = MonsterSpawner.Instance.monsterList[i];
                }
            }

            if (targetMonster != null)
            {
                if (attackMonCor == null)
                {
                    attackMonCor = StartCoroutine(AttackMonster());
                }

            }
            else
            {
                StopCoroutine(AttackMonster());
                attackMonCor = null;
            }
            yield return null;
            targetMonster = null;
        }
    }
    private IEnumerator AttackMonster()
    {
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
            bulletSpawner.SpawnBullets(transform.position, targetMonster.transform.position);

            //playerAnim.SetTrigger("IsAttack");
            yield return new WaitForSeconds(attackSpeed);
        }

    }
}
