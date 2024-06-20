/* 外部ファイルの構造体 */

// ステージ
[System.Serializable]
public class StageCSV
{
    public int StageNo;
    public string StageName;
    public float EnemyBoostRate;
    public int StaticMoney;
    public int DynamicMoney;
}

// パターン(敵生成)
[System.Serializable]
public class PatternCSV
{
    public int WaveNo;
    public string EnemyNo;
    public float CreateSec;
    public int CreatePosNo;
}

// 敵
[System.Serializable]
public class EnemyCSV
{
    public string ID;
    public int MaxHp;
    public float Speed;
    public int Attack;
    public int AttackInterval;
}

// 武器
[System.Serializable]
public class WeaponCSV
{
    public int Lv;
    public int W_0;
    public int W_1;
    public float W_2;
    public int W_3;
    public int W_4;
    public float W_5;
}

// アイテム
public class ItemCSV
{
    public string ID;
    public string Type;
    public float Effect1;
}

// ショップ
public class ShopCSV

{
    public string id;
    public int Lv1;
    public int Lv2;
    public int Lv3;
    public int Lv4;
    public int Lv5;
    public int Lv6;
    public int Lv7;
    public int Lv8;
    public int Lv9;
    public int Lv10;
}