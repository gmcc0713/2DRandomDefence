using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{
    //======================싱글톤=================================
    public static UIManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    //===============================================================================
    [SerializeField] private Ground ground;
    [SerializeField] private GameObject unitImageObject;

    [SerializeField] private Sprite[] combinationListPage;

    public Sprite[] arrUnitImage;

    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI healthText;
    private bool canClick = true;
    private int pageIndex;
    public void ClickShowPanelButton(GameObject panel)//특정 panel 보여주기; 버튼 클릭
    {
        if (GameMgr.Instance._click_Type == CLICK_TYPE.IDLE)
        {
            GameMgr.Instance._click_Type = CLICK_TYPE.Show_Pannel;
            panel.SetActive(true);
        }
    }
    public void ClickHidePanelButton(GameObject panel)      //특정 panel 숨기기 버튼 클릭
    {
        panel.SetActive(false);
        GameMgr.Instance._click_Type = CLICK_TYPE.IDLE;
    }
    public void ClickBuyUnitBtn()           //유닛 구매 버튼 클릭
    {

        if (GameMgr.Instance.IsEnoughtGold(GameMgr.Instance._playerBuyUnitGold) && GameMgr.Instance._click_Type == CLICK_TYPE.IDLE)
        {
            SetGoldUI();
            GameMgr.Instance._playerBuyUnitGold += 10;
            int unitIndex;
            ground.ShowGroundSetUnit();
            unitIndex = (int)UnitSpawner.Instance.SpawnNewUnit();
            unitImageObject.GetComponent<Image>().sprite = arrUnitImage[unitIndex];
            GameMgr.Instance._click_Type = CLICK_TYPE.PLACING_UNIT;
        }
    }
    public void ClickSellUnitBtn()                          //유닛 판매 버튼 클릭
    {
        if (GameMgr.Instance._click_Type == CLICK_TYPE.IDLE)
        {
            SetGoldUI();
            ground.ShowUnitGround();
            GameMgr.Instance._click_Type = CLICK_TYPE.Sell_Unit;
        }
    }
    public void ClickCompositionUnitBtn()                       //유닛 조합 버튼 클릭
    {
        if (GameMgr.Instance._click_Type == CLICK_TYPE.IDLE)
        {
            ground.ShowUnitGround();
            GameMgr.Instance._click_Type = CLICK_TYPE.COMPOSITION_UNIT;
        }

    }

    public void ClickCombinationListBtn(GameObject combinationListPannel)
    {
        if (GameMgr.Instance._click_Type == CLICK_TYPE.IDLE)
        {
            ClickShowPanelButton(combinationListPannel);

            //combinationListPannel.GetComponent<Image>().sprite = combinationListPage[pageIndex];
            combinationListPannel.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = combinationListPage[pageIndex];
        }
    }
    public void ClickCombinationListNextPageBtn(GameObject combinationListPannel)
    {

        if (pageIndex < combinationListPage.Length - 1)
        {
            pageIndex++;
            combinationListPannel.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = combinationListPage[pageIndex];

        }
    }
    public void ClickCombinationListPreviousPageBtn(GameObject combinationListPannel)
    {
        if (pageIndex > 0)
        {
            pageIndex--;
            combinationListPannel.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = combinationListPage[pageIndex];
        }
    }
    public void ClickUpgradeAttackTypeBtn(int type)
    {
        SetGoldUI();
        UpgradeManager.Instance.UpgradeUnit((Attack_Type)type);
    }
    public void UnitSpawnWaitImageSet(int index)
    {
        unitImageObject.SetActive(true);
    }
    public void UnitSpawnWaitImageDelete()
    {
        unitImageObject.SetActive(false);

    }
    public void SetHealthUI()
    {
        healthText.text = (GameMgr.Instance._playerHealth).ToString();
    }
    public void SetGoldUI()
    {
        goldText.text = (GameMgr.Instance._playerGold).ToString();
    }

}
