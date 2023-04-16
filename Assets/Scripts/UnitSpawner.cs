
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GameMgr;
static class Constants
{
    public const int MAX_RANDOM_INDEX = 6;
}

public enum UnitType { Unit_Hammer = 0,Unit_AxeMan, Unit_Wizard, Unit_Archer,Unit_Rogue, Unit_Warrior,
                        Unit_Pistol, Unit_Priest, Unit_Spear, Unit_Reaper,
                        Unit_Sniper, Unit_Paladin,Unit_MasicWarrior,Unit_BlackMage, Unit_Non}

public class UnitSpawner : MonoBehaviour
{
    
    //======================½Ì±ÛÅæ=================================
    public static UnitSpawner Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance =this;
        }
    }
    //===============================================================================
    [SerializeField] public GameObject []UnitPrefab;
    private int randomIndex;
    private int maxIndex;
    private bool IsUnitSpawnReady =false;
    [SerializeField] private Ground ground;
    [SerializeField] private List<CombinationData> combinationDatas;
    [SerializeField] private ObjectPool<Unit>[] unitPool;


    void Start()
    {
        maxIndex = Constants.MAX_RANDOM_INDEX;
        randomIndex = 0;
        for (int i = 0; i < unitPool.Length; i++)
        {
            unitPool[i].Initialize();

        }
    }
    public bool CompositionUnit(Unit[] compositionUnit)
    {
        UnitType findType = FindCompositionUnit(compositionUnit);
        if (findType != UnitType.Unit_Non)
        {

            Vector2 spawnPos = compositionUnit[1].transform.position;
            ground.ChangeGroundTag(compositionUnit[0].transform.position);
            unitPool[(int)compositionUnit[0].type].PutInPool(compositionUnit[0]);
            unitPool[(int)compositionUnit[1].type].PutInPool(compositionUnit[1]);
            compositionUnit[0] = null;
            compositionUnit[1] = null;
            Unit cloneUnit;
            if(unitPool[(int)findType].GetObject(out cloneUnit))
            {
                cloneUnit.SetPosition(spawnPos);
            }

            return true;
        }
        return false;
    }
    public UnitType FindCompositionUnit(Unit[] compositionUnit)
    {
        UnitType firstunitType = compositionUnit[0].type;
        UnitType secondunitType = compositionUnit[1].type;
        CombinationData result;
        result = combinationDatas.Find(type => (type._firstMaterialUnit == firstunitType&& type._secondMaterialUnit == secondunitType)
                                            || (type._firstMaterialUnit == secondunitType&& type._secondMaterialUnit == firstunitType));

        if(result==null)
        {
            return UnitType.Unit_Non;
        }
        return result._resultUnit;
    }
    public void SellUnit(Unit sellUnit)
    {
        ground.ChangeGroundTag(sellUnit.transform.position);
        unitPool[(int)sellUnit.type].PutInPool(sellUnit);

    }
    public void SpawnNewUnit()              //±¸¸Å¹öÆ°À» ´­·¶À»¶§
    {
        if(!IsUnitSpawnReady)
        {
            randomIndex = UnityEngine.Random.Range(0, maxIndex);
            Debug.Log(randomIndex);
            IsUnitSpawnReady = true;
            UIManager.Instance.UnitSpawnWaitImageSet(randomIndex);
        }
    }
    public bool SetNewUnitInField(Transform spawnTransform)     //À¯´Ö ¼³Ä¡ °¡´ÉÇÑ°÷À» ´­·¶À»¶§
    {
        if (IsUnitSpawnReady)
        {
            Unit cloneUnit;
            Debug.Log(randomIndex);
            if (unitPool[randomIndex].GetObject(out cloneUnit))
            {
                cloneUnit.SetPosition(spawnTransform.position);
            }

            IsUnitSpawnReady = false;
            return true;
        }
        return false;
    }

}


