
using System.Collections.Generic;
using UnityEngine;
static class Constants
{
    public const int MAX_RANDOM_INDEX = 6;
}

public enum UnitType { Unit_Hammer = 0,Unit_AxeMan, Unit_Wizard, Unit_Archer,Unit_Rogue, Unit_Warrior,
                        Unit_Pistol, Unit_Priest, Unit_Spear, Unit_Reaper,
                        Unit_Sniper, Unit_Paladin,Unit_MasicWarrior,Unit_BlackMage, Unit_Non}

public class UnitSpawner : MonoBehaviour
{
    
    //======================�̱���=================================
    public static UnitSpawner Instance { get; private set; }
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
    //===============================================================================
    [SerializeField] private GameObject []unitPrefab;

    private int randomIndex;
    private int maxIndex;
    private bool IsUnitSpawnReady =false;
    [SerializeField] private Ground ground;
    [SerializeField] private List<CombinationData> combinationDatas;
    [SerializeField] private ObjectPool<Unit>[] unitPool;
    public GameObject[] _unitPrefabs => unitPrefab;

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
            for(int i =0;i<2;i++)
            {
                unitPool[(int)compositionUnit[i].type].PutInPool(compositionUnit[i]);
                compositionUnit[i] = null;
            }

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
        GameMgr.Instance.GetGold(sellUnit._unitGold);

    }
    public UnitType SpawnNewUnit()              //���Ź�ư�� ��������
    {
        if(!IsUnitSpawnReady)
        {
            randomIndex = UnityEngine.Random.Range(0, maxIndex);
            IsUnitSpawnReady = true;
            UIManager.Instance.UnitSpawnWaitImageSet(randomIndex);
            return (UnitType)randomIndex;
        }
        return UnitType.Unit_Non;
    }
    public bool SetNewUnitInField(Transform spawnTransform)     //���� ��ġ �����Ѱ��� ��������
    {
        if (IsUnitSpawnReady)
        {
            Unit cloneUnit;
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

