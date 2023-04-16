using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //======================ΩÃ±€≈Ê=================================
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

    public Sprite[] arrUnitSprite;
    

    public void ClickBuyUnitBtn()
    {
        ground.ShowGroundSetUnit();
        UnitSpawner.Instance.SpawnNewUnit();
        GameMgr.Instance._click_Type = CLICK_TYPE.PLACING_UNIT;
    }
    public void ClickSellUnitBtn()
    {
        ground.ShowUnitGround();
        GameMgr.Instance._click_Type = CLICK_TYPE.Sell_Unit;
    }
    public void ClickCompositionUnitBtn()
    {
        ground.ShowUnitGround();
        GameMgr.Instance._click_Type = CLICK_TYPE.COMPOSITION_UNIT;
    }

    public void UnitSpawnWaitImageSet(int index)
    {
        unitImageObject.SetActive(true);
        //unitImageObject.GetComponent<Image>().sprite = arrUnitSprite[index];
    }
    public void UnitSpawnWaitImageDelete()
    {
        unitImageObject.SetActive(false);
    }
}
