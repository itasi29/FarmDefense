/* 外部ファイルの構造体 */

// ステージ
using JetBrains.Annotations;

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

[System.Serializable]
public class PlayerCSV
{
    public int MaxHp;
    public int MaxStamina;
    public int RecoveryStamina;
    public float Speed;
    public float DashSpeed;
    public float DownSpeed;
    public float JumpPower;
    public float FallPower;
    public int DashCost;
    public int StrongAttackCost;
    public int StanTime;
    public int HitSafeTime;
    public int RevivalSafeTime;
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
    public string ID;
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

// アイテム
public class ItemCSV
{
    public string ID;
    public int Lv;
    public int Effect1;
}

// ショップ
public class ShopCSV

{
    public string ID;
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

[System.Serializable]
public class SoundCSV
{
    public string ID;
    public float Volume;
    public string Type;
    public string Path;
}