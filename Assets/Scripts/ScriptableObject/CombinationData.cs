using UnityEngine;

[CreateAssetMenu(fileName = "Combination Data", menuName ="Scriptable Object/Combination Data",order = 1)]
public class CombinationData : ScriptableObject
{
    [SerializeField] private UnitType firstMaterialUnit;
    [SerializeField] private UnitType secondMaterialUnit;
    [SerializeField] private UnitType resultUnit;

    public UnitType _firstMaterialUnit => firstMaterialUnit;
    public UnitType _secondMaterialUnit => secondMaterialUnit;
    public UnitType _resultUnit => resultUnit;
}
