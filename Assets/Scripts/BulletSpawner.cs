using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
   
    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private ObjectPool<Bullet> bulletPool;

    private float damage;
    private void Start()
    {
        bulletPool.Initialize();
    }

    public void SpawnBullets(Vector2 spawnPos, Vector3 targetMonsterPos)
    {
        Bullet bul;
        if (bulletPool.GetObject(out bul))
        {
            damage = transform.gameObject.GetComponentInParent<Unit>()._attckDamage;
            bul.SetPosition(spawnPos);
            bul.SetBulletInit(targetMonsterPos, this, damage);
        }
    }
    public void PutInPoolBullet(Bullet bullet)
    {
        bulletPool.PutInPool(bullet);
    }
}
