using UnityEngine;

[CreateAssetMenu(fileName = "Wave Data", menuName = "Scriptable Object/Wave Data", order = 1)]


public class WaveData : ScriptableObject
{
    [SerializeField] Monster_Data[] monArr;
    [SerializeField] Stage_Data stageData;

    public Monster_Data[] _monArr => monArr;
    public Stage_Data _stageData => stageData;
}

[System.Serializable]       //인스펙터창에 구조체가 보이게 하기 위한 직렬화
public struct Monster_Data 
{
    public int monsterTypeIndex;        //몬스터 종류 인덱스
    public int monsterCount;            //해당몬스터 갯수
}
[System.Serializable]       //인스펙터창에 구조체가 보이게 하기 위한 직렬화
public struct Stage_Data
{
    public int stage;
    public int wave;
    public float monSpeedBuf;        //해당 스테이지의 이동속도 버프
    public float monHealthBuf;       //해당 스테이지의 체력 버프
}
