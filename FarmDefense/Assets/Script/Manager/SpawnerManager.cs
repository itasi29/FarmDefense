using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class SpawnerManager : MonoBehaviour
{
    /* 定数 */
    // 生成場所の数
    [SerializeField] private const int kCreatePosNum = 6;

    private string kResultSceneName = "ResultScene";

    // 敵の数
    // FIXME: まだ敵の種類が完成しきっていないため、出来次第増やしていく
    [SerializeField] private const int kEnemyTypeNum = 2;


    /* 変数 */
    // カメラ
    private CameraControl _camera;
    // 生成場所の取得
    private GameObject[] createPos = new GameObject[kCreatePosNum];
    // プレハブデータ
    private Dictionary<string, GameObject> enemyPrefab = new Dictionary<string, GameObject>();
    // ステージ名
    [SerializeField] private int _stageNo;
    //ミニマップ
    private Minimap _miniMap;
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

    private UserData _user;
    private Fade _fade;
    private StageRusultData _resultData;

    public int WaveEnemyNum { get { return _waveData[_nowWaveNo].createNum; } }

    public int KillEnemyNum { get { return _killedEnemyNum; } }
    void Start()
    {
        _miniMap = GetComponent<Minimap>();
        _fade = GetComponent<Fade>();

        _camera = GameObject.Find("Main Camera").GetComponent<CameraControl>();
        GameObject parent = GameObject.Find("CreatePos");

        // ステージデータの取得
        GameDirector director = GameObject.Find("GameDirector").GetComponent <GameDirector>();
        DataManager dataMgr = director.DataMgr;
        _user = dataMgr.User;
        _waveData = dataMgr.Spawner.GetWaveData(_stageNo);
        _resultData = director.ResultData;
        _resultData.StageNo = _stageNo;

        // 生成場所の取得
        for (int i = 0; i < kCreatePosNum; ++i)
        {
            createPos[i] = parent.transform.GetChild(i).gameObject;
        }
        // プレハブデータの取得
        List<string> enemyIDList = dataMgr.Enemy.GetID();
        foreach (var id in enemyIDList)
        {
            enemyPrefab[id] = (GameObject)Resources.Load("Enemy/" + id);
        }


        // 各パラメータの初期化
        _nowWaveNo = 0;
        _elapsFrame = 0;
        _createNo = 0;
        _killedEnemyNum = 0;
        _isStopCreate = false;
        _isAllCreate = false;
    }

    
    void FixedUpdate()
    {
        IsKillAllEnemys();
        if (_isAllCreate) return;

        _user.AddTime();
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
        if (_isStopCreate)
        {
            return;
        }

        // 生成フレームの増加
        ++_elapsFrame;

        // 一時変数
        WaveData wave = _waveData[_nowWaveNo];
        PatternData pattern = wave.patternDatas[_createNo];

        // 生成フレームを超えていなければ終了
        if (_elapsFrame < pattern.createFrame) return;

        // 種類・場所に合わせて生成
        CreateEnemy(pattern.enemyNo, pattern.createPosNo);

        // 生成した数の増加
        ++_createNo;
        // そのwaveの生成数を超えていれば
        if (_createNo >= wave.createNum)
        {
            // 生成停止
            _isStopCreate = true;

            // 現在が最終waveになっていれば
            if (_nowWaveNo == 2)
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
    /// 種類と場所に合わせて敵を生成する
    /// </summary>
    /// <param name="enemyNo">敵のID</param>
    /// <param name="posNo">生成場所の番号</param>
    private void CreateEnemy(string enemyID, int posNo)
    {
        GameObject enemy;
        enemy = Instantiate(enemyPrefab[enemyID]);

        _miniMap.EntryMiniMapEnemy(enemy);

        // TODO: 敵の位置初期化できたらそれに変更
        enemy.GetComponent<EnemyBase>().Init(createPos[posNo].transform.position, enemyID);
        _camera.AddHpBarInfo(enemy);
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
            // 全ての敵生成した上での倒してであったらリザルトへ
            if (_isAllCreate)
            {
                _resultData.IsCrear = true;
                _fade.StartFadeOut(kResultSceneName);
                return false;
            }

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
