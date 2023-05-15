using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
   
    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private ObjectPool<Bullet> bulletPool;

    private float damage;
    void Start()
    {
        bulletPool.Initialize();
    }

    public void SpawnBullets(Vector2 spawnPos, Monster targetMonster)
    {
        Bullet bul;
        if (bulletPool.GetObject(out bul))
        {
            damage = transform.gameObject.GetComponentInParent<Unit>()._attckDamage;
            bul.SetBulletInit(targetMonster, this, damage);
            bul.SetPosition(spawnPos);
        }
    }
    public void PutInPoolBullet(Bullet bullet)
    {
        bulletPool.PutInPool(bullet);
    }
}
