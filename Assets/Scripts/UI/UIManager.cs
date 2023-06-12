using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] private TextMeshProUGUI MonsterCountText;
    private int pageIndex;
    public void ClickShowPanelButton(GameObject panel)//특정 panel 보여주기 버튼 클릭
    {
        if (GameMgr.Instance._click_Type == CLICK_TYPE.IDLE)
        {
            SoundManager.Instance.PlayAudioClipOneShot(Sound_Type.Sound_SFX,(int)SFX_Num.Click_Button);
            GameMgr.Instance._click_Type = CLICK_TYPE.Show_Pannel;
            panel.SetActive(true);
        }
    }
    public void ClickShowOptionPanelButton(GameObject panel)//특정 panel 보여주기 버튼 클릭
    {
        if (GameMgr.Instance._click_Type == CLICK_TYPE.IDLE)
        {
            SoundManager.Instance.PlayAudioClipOneShot(Sound_Type.Sound_SFX, (int)SFX_Num.Click_Button);
            GameMgr.Instance._click_Type = CLICK_TYPE.Show_Pannel;
            panel.SetActive(true);
            Time.timeScale = 0;
        }

    }
    public void ClickHidePanelButton(GameObject panel)      //특정 panel 숨기기 버튼 클릭
    {
        SoundManager.Instance.PlayAudioClipOneShot(Sound_Type.Sound_SFX, (int)SFX_Num.Click_Button);
        panel.SetActive(false);
        GameMgr.Instance._click_Type = CLICK_TYPE.IDLE;
        Time.timeScale = 1;
    }
    public void ClickHideOptionPanelButton(GameObject panel)      //특정 panel 숨기기 버튼 클릭
    {
        SoundManager.Instance.PlayAudioClipOneShot(Sound_Type.Sound_SFX, (int)SFX_Num.Click_Button);
        panel.SetActive(false);
        GameMgr.Instance._click_Type = CLICK_TYPE.IDLE;
    }
    public void ClickBuyUnitBtn()           //유닛 구매 버튼 클릭
    {

        if (GameMgr.Instance.IsEnoughtGold(GameMgr.Instance._playerBuyUnitGold) && GameMgr.Instance._click_Type == CLICK_TYPE.IDLE)
        {
            SoundManager.Instance.PlayAudioClipOneShot(Sound_Type.Sound_SFX, (int)SFX_Num.Click_Button);
            SetGoldUI();
            GameMgr.Instance._playerBuyUnitGold += 10;
            ground.ShowGroundSetUnit();
            int unitIndex = (int)UnitSpawner.Instance.SpawnNewUnit();
            unitImageObject.GetComponent<Image>().sprite = arrUnitImage[unitIndex];
            GameMgr.Instance._click_Type = CLICK_TYPE.PLACING_UNIT;
        }
    }
    public void ClickSellUnitBtn()                          //유닛 판매 버튼 클릭
    {
        if (GameMgr.Instance._click_Type == CLICK_TYPE.IDLE)
        {
            SoundManager.Instance.PlayAudioClipOneShot(Sound_Type.Sound_SFX, (int)SFX_Num.Click_Button);

            ground.ShowUnitGround();
            GameMgr.Instance._click_Type = CLICK_TYPE.Sell_Unit;
        }
    }
    public void ClickCompositionUnitBtn()                       //유닛 조합 버튼 클릭
    {
        if (GameMgr.Instance._click_Type == CLICK_TYPE.IDLE)
        {
            SoundManager.Instance.PlayAudioClipOneShot(Sound_Type.Sound_SFX, (int)SFX_Num.Click_Button);
            ground.ShowUnitGround();
            GameMgr.Instance._click_Type = CLICK_TYPE.COMPOSITION_UNIT;
        }

    }

    public void ClickCombinationListBtn(GameObject combinationListPannel)
    {
        if (GameMgr.Instance._click_Type == CLICK_TYPE.IDLE)
        {
            SoundManager.Instance.PlayAudioClipOneShot(Sound_Type.Sound_SFX, (int)SFX_Num.Click_Button);
            ClickShowPanelButton(combinationListPannel);

            combinationListPannel.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = combinationListPage[pageIndex];
        }
    }
    public void ClickCombinationListNextPageBtn(GameObject combinationListPannel)
    {

        if (pageIndex < combinationListPage.Length - 1)
        {
            SoundManager.Instance.PlayAudioClipOneShot(Sound_Type.Sound_SFX, (int)SFX_Num.Click_Button);
            pageIndex++;
            combinationListPannel.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = combinationListPage[pageIndex];

        }
    }
    public void ClickCombinationListPreviousPageBtn(GameObject combinationListPannel)
    {
        if (pageIndex > 0)
        {
            SoundManager.Instance.PlayAudioClipOneShot(Sound_Type.Sound_SFX, (int)SFX_Num.Click_Button);
            pageIndex--;
            combinationListPannel.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = combinationListPage[pageIndex];
        }
    }
    public void ClickUpgradeAttackTypeBtn(int type)// 해당 업그레이드 타입 추가
    {
        SoundManager.Instance.PlayAudioClipOneShot(Sound_Type.Sound_SFX, (int)SFX_Num.Click_Button);
        SetGoldUI();
        UpgradeManager.Instance.UpgradeUnit((Attack_Type)type);
    }
    public void ClickBackMenuButton()      //메뉴 씬 돌아가기 버튼
    {
        SoundManager.Instance.PlayAudioClipOneShot(Sound_Type.Sound_SFX, (int)SFX_Num.Click_Button);
        if (PlayerPrefs.GetInt("MaxClearStage") <= WaveManager.Instance._waveIndex  / 5)
        {
            PlayerPrefs.SetInt("MaxClearStage", WaveManager.Instance._waveIndex / 5);

        }
        SceneManager.LoadScene("MenuScene");
    }
    public void UnitSpawnWaitImageSet(int index)
    {
        unitImageObject.SetActive(true);
    }
    public void UnitSpawnWaitImageDelete()
    {
        unitImageObject.SetActive(false);

    }
    public void SetTextChange(TextMeshProUGUI UI, string changeString)
    {
        UI.text = changeString;
    }
    public void SetHealthUI()
    {
        healthText.text = (GameMgr.Instance._playerHealth).ToString();
    }
    public void SetGoldUI()
    {
        goldText.text = (GameMgr.Instance._playerGold).ToString();
    }
    public void SetMonsterCountUI()
    {
        System.Text.StringBuilder monsterCount = new System.Text.StringBuilder(MonsterSpawner.Instance._remainingMonsterCount.ToString());
        monsterCount.Append(" / ");
        monsterCount.Append(WaveManager.Instance._thisWaveMonsterCount.ToString());
        MonsterCountText.text = monsterCount.ToString();
        
    }
}
