using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class NewBehaviourScript : MonoBehaviour
{
    public float range;
    public int color;
    private void OnDrawGizmos()
    {
        switch (color)
        {
            case 1:
                Gizmos.color = Color.green;
                break;
            case 2:
                Gizmos.color = Color.red;
                break;
            case 3:
                Gizmos.color = Color.white;
                break;
        }

        Gizmos.DrawWireSphere(transform.position, range);
    }
}
