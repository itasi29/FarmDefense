/* �O���t�@�C���̍\���� */

// �X�e�[�W
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
    public float Lv1; 
    public float Lv2; 
    public float Lv3; 
    public float Lv4; 
    public float Lv5; 
    public float Lv6;
    public float Lv7;
    public float Lv8;
    public float Lv9;
    public float Lv10;
}

// �A�C�e��
public class ItemCSV
{
    public string ID;
    public string Type;
    public float Effect1;
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