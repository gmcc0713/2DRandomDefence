using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI[] upgradeCountText;
    [SerializeField] private TextMeshProUGUI[] upgradePriceText;
    private int[] UpgradeType;
    private int[] UpgradeGold;

    public int[] _UpgradeType => UpgradeType;
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
        UpgradeType = new int[(int)Attack_Distance_Type.Type_Count] {0,0 };
        UpgradeGold = new int[(int)Attack_Distance_Type.Type_Count] {100,100};
        SetUpgradeUI();
    }
    //===============================================================================

    public void UpgradeUnit(Attack_Type type)
    {
        if(GameMgr.Instance.IsEnoughtGold(UpgradeGold[(int)type]))
        {
            UpgradeType[(int)type]++;
            UpgradeGold[(int)type]+=10;
            SetUpgradeUI();
        }
    }
    public void SetUpgradeUI()
    {
        for(int i =0;i<(int)Attack_Distance_Type.Type_Count;i++)
        {
            System.Text.StringBuilder upgradeCountString = new System.Text.StringBuilder("+");
            upgradeCountString.Append(UpgradeType[i].ToString());
            upgradeCountString.Append(" °­");
            upgradeCountText[i].text = upgradeCountString.ToString();


            System.Text.StringBuilder upgradGoldString = new System.Text.StringBuilder("");
            upgradGoldString.Append(UpgradeGold[i].ToString());
            upgradGoldString.Append("G");
            upgradePriceText[i].text = upgradGoldString.ToString();
        }

    }

}
