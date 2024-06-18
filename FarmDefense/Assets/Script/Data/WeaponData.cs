using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct WeaponStatus
{
    public int nearAttack;
    public int nearAttackInterval;
    public float nearRange;
    public int farAttack;
    public int farAttackInterval;
    public float bulletSpeed;
}

public class WeaponData
{

    private Dictionary<int, WeaponStatus> _data = new Dictionary<int, WeaponStatus>();

    public void Load()
    {
        // csvファイルの読み込み
        TextAsset csv = Resources.Load("Csv/WeaponManager") as TextAsset;
        // データ読み込み
        WeaponCSV[] items = CSVSerializer.Deserialize<WeaponCSV>(csv.text);

        for (int i = 0; i < items.Length; ++i)
        {
            WeaponStatus status = new WeaponStatus();
            status.nearAttack = items[i].W_0;
            status.nearAttackInterval = items[i].W_1;
            status.nearRange = items[i].W_2;
            status.farAttack = items[i].W_3;
            status.farAttackInterval = items[i].W_4;
            status.bulletSpeed = items[i].W_5;

            _data[i] = status;
        }
    }

    public int GetNearAttack(int lv)
    {
        return _data[lv].nearAttack;
    }
    public int GetNearAttackInterval(int lv)
    {
        return _data[lv].nearAttackInterval;
    }
    public float GetNearRange(int lv)
    {
        return _data[lv].nearRange;
    }
    public int GetFarAttack(int lv) 
    {
        return _data[lv].farAttack;
    }
    public int GetFarAttackInterval(int lv)
    {
        return _data[lv].farAttackInterval;
    }
    public float GetBulletSpeed(int lv)
    {
        return _data[lv].bulletSpeed;
    }
}
