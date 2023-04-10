using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Monster targetMonster;
    private float bulletSpeed = 50;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetMonster.transform.position, bulletSpeed * Time.deltaTime);
    }
       public void SetBullet(Vector2 spawnPos, Monster targetMon)
    {
        targetMonster = targetMon;
        transform.position = spawnPos;
    }
}
