using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
   
    [SerializeField]
    private GameObject BulletPrefab;

 
    public void SpawnBullets(Vector2 spawnPos, Monster targetMons)
    {
        GameObject clone = Instantiate(BulletPrefab);
        Bullet bullet = clone.GetComponent<Bullet>();
        bullet.SetBullet(spawnPos, targetMons);

    }
}
