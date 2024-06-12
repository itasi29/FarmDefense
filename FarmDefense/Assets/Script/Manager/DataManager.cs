using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private SpawnerData _spawner;
    private EnemyData _enemy;

    void Start()
    {
        // 自信を非破壊オブジェクトにする
        DontDestroyOnLoad(this.gameObject);

        // 各種データ系を生成
        _spawner = new SpawnerData();
        _enemy = new EnemyData();

        // データの読み込み
        _spawner.Load();
        _enemy.Load();
    }

    // 各プロパティ
    public SpawnerData Spawner { get { return _spawner; } }
    public EnemyData Enemy { get { return _enemy; } }
}
