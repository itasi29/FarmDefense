using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public abstract class EnemyBase : MonoBehaviour
{
    /* 定数 */
    private const int kDecreaseHpSpeed = 1;   // _deltaHpの減少速度

    /* 変数 */
    [SerializeField] private EnemyStatus _status;    // ステータス
    private int _hp;                // 現在のHP
    private int _deltaHp;           // 減少を分かりやすくするための変数
    private int _watiAttackFrame;   // 攻撃待機フレーム用
    private bool _isExist;          // 生存フラグ
    private bool _isDeltaHp;        // 減少フラグ
    protected bool _isStopAttack;     // 攻撃停止フラグ
    protected bool _isFindPlayer;     // プレイヤー発見フラグ
    private GameObject _farmBase;   // 農場全部を持っている親オブジェクト
    protected GameObject _farm;     // 攻撃する農場
    protected Farm _farmScript;     // 上ののスクリプト
    protected GameObject _player;   // プレイヤー
    private CameraControl _camera;  // カメラ情報
    private SpawnerManager _spawnerMgr; // スポナーマネージャー

    /* プロパティ */
    public int Hp { get { return _hp; } }
    public int DeltaHp { get { return _deltaHp; } }
    public int MaxHp { get { return _status.maxHp; } }
    public bool IsExist { get { return  IsExist; } }

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="pos">初期位置</param>
    /// <param name="enemyNo">敵の番号</param>
    public virtual void Init(Vector3 pos, int enemyNo)
    {
        _farmBase = GameObject.Find("Farm").gameObject;
        _player = GameObject.Find("Player").gameObject;
        _camera = GameObject.Find("Main Camera").GetComponent<CameraControl>();
        _spawnerMgr = GameObject.Find("SpawnerManager").GetComponent<SpawnerManager>();

        // ステータス取得
        EnemyData data = GameObject.Find("DataManager").GetComponent<EnemyData>();
        _status = data.GetStatus(enemyNo);

        // 各種初期化
        _hp = _status.maxHp;
        _deltaHp = _status.maxHp;
        _watiAttackFrame = 0;
        _isExist = true;
        _isDeltaHp = false;
        _isStopAttack = false;
        _isFindPlayer = false;
        transform.position = pos;
    }

    /// <summary>
    /// ダメージ処理
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    public void OnDamage(int damage)
    {
        _hp -= damage;
        _isDeltaHp = true;

        // HPが0以下になったら死亡処理
        if (_hp <= 0)
        {
            _hp = 0;
            _isExist = false;
            // スポナーマネージャーに死亡したことを伝える
            _spawnerMgr.AddKilledEnemy();
            // カメラに死亡したことを伝える
            _camera.SubHpBarInfo(this.gameObject);
            // 破壊
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// 農場に向かっての移動
    /// </summary>
    protected void MoeveToFarm()
    {
        Vector3 pos = transform.position;
        Vector3 farmPos = _farm.transform.position;

        Vector3 velocity = (farmPos - pos).normalized * _status.speed;

        transform.position = pos + velocity;
    }

    /// <summary>
    /// プレイヤーに向かっての移動
    /// </summary>
    protected void MoveToPlayer()
    {
        Vector3 pos = transform.position;
        Vector3 playerPos = _player.transform.position;

        Vector3 velocity = (playerPos - pos).normalized * _status.speed;

        transform.position = pos + velocity;
    }

    /// <summary>
    /// 農場への攻撃処理
    /// </summary>
    protected void AttackFarm()
    {
        // 攻撃待機状態ならしない
        if (_isStopAttack) return;
        // プレイヤー発見時なら農場を攻撃しない
        if (_isFindPlayer) return;

        _farmScript.OnDamage(_status.attack);
        _isStopAttack = true;
    }

    protected void AttackPlayer()
    {
        // 攻撃待機状態ならしない
        if (_isStopAttack) return;
        // プレイヤー非発見時なら攻撃しない
        if (!_isFindPlayer) return;

        _player.GetComponent<Player>().OnDamage(_status.attack);
        _isStopAttack = true;
    }

    /// <summary>
    /// 攻撃待機処理
    /// </summary>
    protected void AttackInterval()
    {
        // 攻撃停止してないなら終了
        if (!_isStopAttack) return;

        --_watiAttackFrame;
        // 攻撃待機時間が終了したら再び攻撃できるように
        if (_watiAttackFrame < 0)
        {
            _isStopAttack = false;
        }
    }

    /// <summary>
    /// _deltaHpを_hpまで減らす処理
    /// </summary>
    protected void ReduceDeltaHp()
    {
        // 減少中でないなら終了
        if (!_isDeltaHp) return;

        // 減少
        _deltaHp -= kDecreaseHpSpeed;
        // 現在のHP未満になったら終了
        if (_deltaHp < _hp)
        {
            _deltaHp = _hp;
            _isDeltaHp = false;
        }
    }

    /// <summary>
    /// 攻撃農場の設定
    /// </summary>
    /// <param name="isNear">true : 近くの農場を攻撃対象に / false : 遠くの農場を攻撃対象に</param>
    protected void FindFarm(bool isNear)
    {
        int childIdx = 0;

        bool isFirst = true;
        float dis = 0.0f;

        Vector3 pos = this.transform.position;

        for (int i = 0; i < FarmManager.kFarmNum; ++i)
        {
            Farm tempFarm = _farmBase.transform.GetChild(i).gameObject.GetComponent<Farm>();
            if (tempFarm.IsBreak) continue;

            if (isFirst)
            {
                Vector3 childPos = _farmBase.transform.GetChild(i).transform.position;
                dis = (pos - childPos).sqrMagnitude;
                childIdx = i;

                isFirst = false;
            }
            else
            {
                Vector3 childPos = _farmBase.transform.GetChild(i).transform.position;
                var childSqrLen = (pos - childPos).sqrMagnitude;

                bool isUpdate = false;
                if (isNear)
                {
                    isUpdate = (dis > childSqrLen);
                }
                else
                {
                    isUpdate = (dis < childSqrLen);
                }

                if (isUpdate)
                {
                    dis = childSqrLen;
                    childIdx = i;
                }
            }
        }

        _farm = _farmBase.transform.GetChild(childIdx).gameObject;
        _farmScript = _farm.GetComponent<Farm>();
    }
}
