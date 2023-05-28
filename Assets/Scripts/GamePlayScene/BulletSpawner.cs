using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] private ObjectPool<Bullet>[] bulletPool;
    public static BulletSpawner Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        for (int i = 0; i < bulletPool.Length; i++)
        {
            bulletPool[i].Initialize();
        }
    }


    public void SpawnBullets(Vector2 spawnPos, Monster targetMonster, float damage, Attack_Type index)
    {
        Bullet bul;
        if (bulletPool[(int)index].GetObject(out bul))
        {
            bul.SetPosition(spawnPos);
            bul.SetBulletInit(targetMonster, damage);
        }
    }
    public void PutInPoolBullet(Bullet bullet)
    {
        bulletPool[(int)bullet._bulletType].PutInPool(bullet);
    }
}


