/* �O���t�@�C���̍\���� */

// �X�e�[�W
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

// �p�^�[��(�G����)
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

// �G
[System.Serializable]
public class EnemyCSV
{
    public string ID;
    public int MaxHp;
    public float Speed;
    public int Attack;
    public int AttackInterval;
}

// ����
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

// �A�C�e��
public class ItemCSV
{
    public string ID;
    public int Lv;
    public int Effect1;
}

// �V���b�v
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