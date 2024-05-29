using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    /* 変数 */
    // ステージ名
    [SerializeField] private int _stageNo;
    // ステージ生成データ
    private List<WaveData> _waveData;
    // 現在のwave番号
    private int _nowWaveNo;
    // 生成番号
    private int _createNo;
    // wave開始からの経過フレーム
    private int _elapsFrame;
    // 倒した敵の数
    private int _killedEnemyNum;
    // 生成を停止するか
    private bool _isStopCreate;
    // 全waveの敵を生成したか
    private bool _isAllCreate;

    void Start()
    {
        // ステージデータの取得
        _waveData = GameObject.Find("DataManager").GetComponent<SpawnerData>().GetWaveData(_stageNo);

        // 各パラメータの初期化
        _nowWaveNo = 1;
        _elapsFrame = 0;
        _createNo = 0;
        _killedEnemyNum = 0;
        _isStopCreate = false;
        _isAllCreate = false;
    }

    
    void FixedUpdate()
    {
        if (_isAllCreate) return;

        IsKillAllEnemys();
        Create();
    }

    /// <summary>
    /// 殺した敵の数を増やす
    /// </summary>
    public void AddKilledEnemy()
    {
        ++_killedEnemyNum;
    }

    private void Create()
    {
        // 生成終了していれば何もしない
        if (_isStopCreate) return;

        // 生成フレームの増加
        ++_elapsFrame;

        // 一時変数
        WaveData wave = _waveData[_nowWaveNo];
        PatternData pattern = wave.patternDatas[_createNo];

        // 生成フレームを超えていなければ終了
        if (_elapsFrame < pattern.createFrame) return;
        
        // TODD:敵の種類に合わせての生成

        // 生成した数の増加
        ++_createNo;
        // そのwaveの生成数を超えていれば
        if (_createNo >= wave.createNum)
        {
            // 生成停止
            _isStopCreate = true;

            // 現在のwaveが3になっていれば
            if (_nowWaveNo == 3)
            {
                // 全て生成したことに
                _isAllCreate = true;
            }

            // この時点で関数は終了させる
            return;
        }

        // 同時タイミングでの生成ようにフレームを減らして再帰する
        --_elapsFrame;
        Create();
    }

    /// <summary>
    /// 現在のwaveの敵を全て倒した
    /// </summary>
    /// <returns>true : 倒した / false : 倒していない</returns>
    private bool IsKillAllEnemys()
    {
        bool isAllKill = false;

        // そのwaveの全ての敵を倒したら
        if (_killedEnemyNum >= _waveData[_nowWaveNo].createNum)
        {
            // 現在のwave数を増やす
            ++_nowWaveNo;
            // 各生成用変数の初期化
            _createNo = 0;
            _elapsFrame = 0;
            _killedEnemyNum = 0;
            _isStopCreate = false;

            // 倒したことに
            isAllKill = true;
        }

        return isAllKill;
    }

}
