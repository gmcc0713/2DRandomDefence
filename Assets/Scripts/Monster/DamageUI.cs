using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageUI : MonoBehaviour
{
    private TextMeshProUGUI damageUI;
    void Start()
    {
        damageUI = gameObject.GetComponent<TextMeshProUGUI>();
        DamageUIInIt();
    }
    public void DamageUIInIt()
    {
        gameObject.SetActive(false);
    }
    public void MoveWithMonster(Vector3 movePos)
    {
        transform.position = Camera.main.WorldToScreenPoint(movePos);
    }
    public void MonsterDamageUpdate(float damage)
    {
        gameObject.SetActive(true);
        damageUI.text = damage.ToString();
        StartCoroutine(DisapearDamageui());
    }
    IEnumerator DisapearDamageui()
    { 
        yield return new WaitForSeconds(0.3f);
        
        gameObject.SetActive(false);
    }
}