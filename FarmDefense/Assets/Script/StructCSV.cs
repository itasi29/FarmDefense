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
    public int Lv;
    public int W_0;
    public int W_1;
    public float W_2;
    public int W_3;
    public int W_4;
    public float W_5;
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
    public int Lv;
    public int I_0;
    public int I_1;
    public int I_2;
    public int I_3;
    public int I_4;
    public int I_5;
}