using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BlinkText : MonoBehaviour
{
    // Start is called before the first frame update
    private float blinkTime;
    [SerializeField]private TextMeshProUGUI text;
    string originText;
    void Start()
    {
        blinkTime = 0.5f;
        originText = "게임을 시작하려면 누르세요";
        Debug.Log(originText);
    }

    void OnEnable()
    {
        Debug.Log(originText);
        StartCoroutine(BlinkStart());
    }
    void OnDisable()
    {
        StopCoroutine(BlinkStart());
    }
    IEnumerator BlinkStart()
    {
        while(true)
        {
            text.text = originText;
            yield return new WaitForSeconds(blinkTime);
            text.text = "";
            yield return new WaitForSeconds(blinkTime);
            
            
        }
        
    }
    
}
