using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GameMgr;

public enum UnitType { Unit_Pistol = 0,Unit_Miner, Unit_Wizard, Unit_Archer,Unit_Rogue, Unit_Warrior,
                        Unit_Flamethrower, Unit_Priest, Unit_Spear, Unit_Crossbow,
                        Unit_Sniper, Unit_Paladin,Unit_DragonTamer,Unit_BlackMage, Unit_Non}

public class UnitSpawner : MonoBehaviour
{
    
    //======================싱글톤=================================
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
    private List<Unit> unitList = new List<Unit>();

    void Start()
    {
        maxIndex = 4;
        randomIndex = 0;
    }
    public bool CompositionUnit(Unit[] compositionUnit)
    {
        UnitType findType = FindCompositionUnit(compositionUnit);
        if (findType != UnitType.Unit_Non)
        {

            Vector2 spawnPos = compositionUnit[1].transform.position;
            ground.ChangeGroundTag(compositionUnit[0].transform.position);
            Destroy(compositionUnit[0].gameObject);
            Destroy(compositionUnit[1].gameObject);

            Instantiate(UnitPrefab[(int)findType], spawnPos, Quaternion.identity);
            return true;
        }
        return false;
    }
    public UnitType FindCompositionUnit(Unit[] compositionUnit)
    {
        Debug.Log("조합 시작");
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
    public void SpawnNewUnit()              //구매버튼을 눌렀을때
    {
        if(!IsUnitSpawnReady)
        {
            randomIndex = UnityEngine.Random.Range(0, maxIndex);
            IsUnitSpawnReady = true;
            UIManager.Instance.UnitSpawnWaitImageSet(randomIndex);
        }
    }
    public bool SetNewUnitInField(Transform spawnTransform)     //유닛 설치 가능한곳을 눌렀을때
    {
        if(IsUnitSpawnReady)
        {
            GameObject clone = Instantiate(UnitPrefab[randomIndex], spawnTransform.position, Quaternion.identity);
            Unit unit = clone.GetComponent<Unit>();
            unit.type = (UnitType)randomIndex;

            unitList.Add(unit);
            IsUnitSpawnReady = false;
            return true;
        }
        return false;
      
    }
}


