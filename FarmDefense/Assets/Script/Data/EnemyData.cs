using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EnemyStatus
{
    public int maxHp;            // 最大HP
    public float speed;            // 移動速度
    public int attack;           // 攻撃力
    public int attackInterval;   // 攻撃間隔
};

public class EnemyData
{
    private Dictionary<string, EnemyStatus> _data = new Dictionary<string, EnemyStatus>();

    public void Load()
    {
        // csvファイルの読み込み
        TextAsset csv = Resources.Load("Csv/EnemyManager") as TextAsset;
        // データ読み込み
        EnemyCSV[] items = CSVSerializer.Deserialize<EnemyCSV>(csv.text);

        foreach (var item in items)
        {
            EnemyStatus status = new EnemyStatus();
            status.maxHp = item.MaxHp;
            status.speed = item.Speed;
            status.attack = item.Attack;
            status.attackInterval = item.AttackInterval;

            _data[item.ID] = status;
        }
    }

    public EnemyStatus GetStatus(string id)
    {
        EnemyStatus status = new EnemyStatus();
        status.maxHp = 100;
        status.speed = 15.0f;
        status.attack = 2;
        status.attackInterval = 60;
        return status;
        return _data[id];
    }

    public List<string> GetID()
    {
        List<string> result = new List<string>();

        foreach (var item in _data)
        {
            result.Add(item.Key);
        }

        return result;
    }
}
