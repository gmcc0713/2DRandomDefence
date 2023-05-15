using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
public enum GroundDisplayType
{
    Type1,
    Type2
}

public class Ground : MonoBehaviour
{
    //Render���
    private Renderer[] childGrounds;
    private Color[] GroundColor = new Color[2]{ new Color(0, 0.5f, 1, 1) , new Color(1, 0.3f, 0, 1)};
    void Start()
    {
        childGrounds = new Renderer[transform.childCount];
        for(int i = 0; i < childGrounds.Length; i++)
        {
            childGrounds[i] = transform.GetChild(i).GetComponent<Renderer>();
        }
    }

    public void ShowGroundSetUnit()
    {
        foreach(Renderer child in childGrounds)
        {
            if (child.CompareTag("GroundWithoutPlayer"))
            {
                child.material.color = GroundColor[0];
            }
            else if(child.CompareTag("GroundWithPlayer"))
            {
                child.material.color = GroundColor[1];
            }
        }
    }
    public void ShowUnitGround()
    {
        foreach (Renderer child in childGrounds)
        {
            if (child.CompareTag("GroundWithoutPlayer"))
            {
                child.material.color = GroundColor[1];
            }
            else if (child.CompareTag("GroundWithPlayer"))
            {
                child.material.color = GroundColor[0];
            }
        }
    }
    public void ShowUnitGroundRemove()
    {
        foreach (Renderer child in childGrounds)
        {
            child.material.color = Color.white;
        }
    }
    public void ChangeGroundTag(Vector3 pos)
    {
        foreach (Renderer child in childGrounds)
        {
            if(child.transform.position == pos)
            {
                child.gameObject.tag = "GroundWithoutPlayer";

            }
        }
    }
    void Update()
    {
        
    }
}
