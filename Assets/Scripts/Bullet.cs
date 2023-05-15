using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour,IPoolingObject
{
    private Monster targetMonster;
    private float bulletSpeed = 23.0f;
    private BulletSpawner bulletSpawner;
    private float attackDamage;
    public float _attckDamage => attackDamage;
    void Update()
    {
        
        transform.position = Vector3.MoveTowards(transform.position, targetMonster.transform.position, bulletSpeed * Time.deltaTime);
        if(!targetMonster.gameObject.activeSelf)
        {
            bulletSpawner.PutInPoolBullet(this);
        }
    }
    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }
    public void SetBulletInit(Monster targetMon,BulletSpawner bs,float damage)          //총알의 타겟설정
    {
        targetMonster = targetMon;
        bulletSpawner = bs;
        attackDamage = damage;
    }
    private void OnTriggerEnter2D(Collider2D other)           //총알이 몬스터와 충돌했을때
    {
        if (other.CompareTag("Monster"))
        {
            bulletSpawner.PutInPoolBullet(this);
        }
    }

}
