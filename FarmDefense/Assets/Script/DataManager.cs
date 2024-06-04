/* データ系を一括に管理する */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private SpawnerData _spawner;

    void Start()
    {
        // 自信を非破壊オブジェクトにする
        DontDestroyOnLoad(this.gameObject);

        // 各種データ系を生成
        _spawner = new SpawnerData();

        // データの読み込み
        _spawner.Load();
    }

    // 各プロパティ
    public SpawnerData Spawner { get { return _spawner; } }
}
