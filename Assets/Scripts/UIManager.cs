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
        ground.ShowPlaceAble();
        UnitSpawner.Instance.SpawnNewUnit();
        GameMgr.Instance._click_Type = GameMgr.CLICK_TYPE.PLACING_UNIT;
    }
    public void ClickCompositionUnitBtn()
    {
        GameMgr.Instance._click_Type = GameMgr.CLICK_TYPE.COMPOSITION_UNIT;
        Debug.Log(GameMgr.Instance._click_Type);
    }

    public void UnitSpawnWaitImageSet(int index)
    {
        Debug.Log(index);
        unitImageObject.SetActive(true);
        //unitImageObject.GetComponent<Image>().sprite = arrUnitSprite[index];
    }
    public void UnitSpawnWaitImageDelete()
    {
        unitImageObject.SetActive(false);
    }
}
