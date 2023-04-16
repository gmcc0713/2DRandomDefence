using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour,IPoolingObject
{
    private Vector3 targetMonsterPosition;
    private float bulletSpeed = 20;
    private BulletSpawner bulletSpawner;
    private float attackDamage;
    public float _attckDamage => attackDamage;
    void Update()
    {
          transform.position = Vector3.MoveTowards(transform.position, targetMonsterPosition, bulletSpeed * Time.deltaTime);
    }
    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }
    public void SetBulletInit(Vector3 targetMonPos,BulletSpawner bs,float damage)          //총알의 타겟설정
    {
        targetMonsterPosition = targetMonPos;
        bulletSpawner = bs;
        attackDamage = damage;
    }
    private void OnTriggerEnter2D(Collider2D other)           //총알이 몬스터와 충돌했을때
    {
        if (other.CompareTag("Monster"))
        {
            Debug.Log("몬스터와 충돌");
            bulletSpawner.PutInPoolBullet(this);
        }
    }
}
