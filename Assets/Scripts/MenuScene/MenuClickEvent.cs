
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuClickEvent : MonoBehaviour
{
    public static MenuClickEvent Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    [SerializeField] private Sprite[] helpPage;
    private int pageIndex=0;
   
    [SerializeField] private GameObject blinkText;
    [SerializeField] private GameObject helpPanel;
    public void ClickShowPannelButton(GameObject pannel)
    {
        SoundManager.Instance.PlayAudioClipOneShot(Sound_Type.Sound_SFX,(int)SFX_Num.Click_Button);
        pannel.SetActive(true);
    }
    public void ClickClosePannelButton(GameObject pannel)
    {
        SoundManager.Instance.PlayAudioClipOneShot(Sound_Type.Sound_SFX, (int)SFX_Num.Click_Button);
        pannel.SetActive(false);
    }
    public void ClickGameOptionButton(GameObject combinationListPannel)
    {
        SoundManager.Instance.PlayAudioClipOneShot(Sound_Type.Sound_SFX, (int)SFX_Num.Click_Button);
    }
    public void ClickStageButton(int stage)
    {
        SoundManager.Instance.PlayAudioClipOneShot(Sound_Type.Sound_SFX, (int)SFX_Num.Click_Button);
        if (PlayerPrefs.GetInt("SelectStage") == -1)
        {
            ClickShowPannelButton(helpPanel);
        }
        else
        {
            PlayerPrefs.SetInt("SelectStage", stage);
            SceneManager.LoadScene("PlayScene");
        }
    }

    public void ClickHelpBtn(GameObject combinationListPannel)
    {
        if (GameMgr.Instance._click_Type == CLICK_TYPE.IDLE)
        {
            SoundManager.Instance.PlayAudioClipOneShot(Sound_Type.Sound_SFX, (int)SFX_Num.Click_Button);
            ClickShowPannelButton(combinationListPannel);

            combinationListPannel.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = helpPage[pageIndex];
        }
    }
    public void ClickHelpNextPageBtn(GameObject combinationListPannel)
    {

        if (pageIndex < helpPage.Length - 1)
        {
            SoundManager.Instance.PlayAudioClipOneShot(Sound_Type.Sound_SFX, (int)SFX_Num.Click_Button);
            pageIndex++;
            combinationListPannel.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = helpPage[pageIndex];
            if (PlayerPrefs.GetInt("SelectStage") == -1 && pageIndex == helpPage.Length - 2)
            {
                blinkText.SetActive(true);
                PlayerPrefs.SetInt("SelectStage", 0);
            }
        }
    }
    public void ClickHelpPreviousPageBtn(GameObject combinationListPannel)
    {
        if (pageIndex > 0)
        {
            SoundManager.Instance.PlayAudioClipOneShot(Sound_Type.Sound_SFX, (int)SFX_Num.Click_Button);
            pageIndex--;
            combinationListPannel.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = helpPage[pageIndex];
        }
    }
}
