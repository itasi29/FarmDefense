/*  */
[System.Serializable]
public class PatternCSV
{
    public int WaveNo;
    public int EnemyNo;
    public float CreateSec;
    public int CreatePosNo;
}

[System.Serializable]
public class StageCSV
{
    public int StageNo;
    public string StageName;
    public float EnemyBoostRate;
    public int StaticMoney;
    public int DynamicMoney;
}
