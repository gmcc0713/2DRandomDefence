using UnityEngine;

[CreateAssetMenu(fileName = "Wave Data", menuName = "Scriptable Object/Wave Data", order = 1)]


public class WaveData : ScriptableObject
{
    [SerializeField] Monster_Data[] monArr;
    [SerializeField] Stage_Data stageData;

    public Monster_Data[] _monArr => monArr;
    public Stage_Data _stageData => stageData;
}

[System.Serializable]       //�ν�����â�� ����ü�� ���̰� �ϱ� ���� ����ȭ
public struct Monster_Data 
{
    public int monsterTypeIndex;        //���� ���� �ε���
    public int monsterCount;            //�ش���� ����
}
[System.Serializable]       //�ν�����â�� ����ü�� ���̰� �ϱ� ���� ����ȭ
public struct Stage_Data
{
    public int stage;
    public int wave;
    public float monSpeedBuf;        //�ش� ���������� �̵��ӵ� ����
    public float monHealthBuf;       //�ش� ���������� ü�� ����
}
