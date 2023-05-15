using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }
    public int[] UpgradeType;
    public int[] UpgradeGold;

    public int[] _UpgradeType => UpgradeType;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        UpgradeType = new int[(int)Attack_Type.Type_Count] {0,0 };
        UpgradeGold = new int[(int)Attack_Type.Type_Count] {100,100};
    }
    //===============================================================================

    public void UpgradeUnit(Attack_Type type)
    {
        if(GameMgr.Instance.IsEnoughtGold(UpgradeGold[(int)type]))
        {
            UpgradeType[(int)type]++;
            UpgradeGold[(int)type]+=10;
            Debug.Log("업그레이드 완료");
        }
    }
}
