using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHPBar : MonoBehaviour
{
    [SerializeField] private Slider monsterHPBar;
    private float maxHP;
    private float curHP;
    public void HPBarInit(float mHP)
    {
        maxHP = mHP;
        curHP = maxHP;
        transform.position = new Vector3(2000, 2000, 0);
    }
    public void MonsterHpBarUpdate(Vector3 movePos,float cHP)
    {
        transform.position = Camera.main.WorldToScreenPoint(movePos);
        curHP = cHP;
        monsterHPBar.value = curHP / maxHP;
    }
}
