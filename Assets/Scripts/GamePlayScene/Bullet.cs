using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour,IPoolingObject
{
    private Monster targetMonster;
    private float bulletSpeed = 23.0f;

    [SerializeField] private Attack_Type bulletType;
    public Attack_Type _bulletType => bulletType;

    private float attackDamage;
    public float _attackDamage => attackDamage;
    void Update()
    {
        if(targetMonster!=null)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetMonster.transform.position, bulletSpeed * Time.deltaTime);
            if (!targetMonster.gameObject.activeSelf)
            {
                BulletSpawner.Instance.PutInPoolBullet(this);
            }
        }
        else
        {
            BulletSpawner.Instance.PutInPoolBullet(this);
        }
       
    }
    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }
    public void SetBulletInit(Monster targetMon,float damage)          //�Ѿ��� Ÿ�ټ���
    {
        targetMonster = targetMon;
        attackDamage = damage;
    }
    private void OnTriggerEnter2D(Collider2D other)           //�Ѿ��� ���Ϳ� �浹������
    {
        if (other.CompareTag("Monster"))
        {
            BulletSpawner.Instance.PutInPoolBullet(this);
        }
    }

}
