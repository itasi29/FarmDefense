using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    /* 型 */
    protected enum AnimParm
    {
        kAttack,
        kHit,
        kDeath,
        kMove,
    };

    /* 定数 */
    private const int kDecreaseHpSpeed = 1;   // _deltaHpの減少速度
    // アニメーションの動きを制御
    private Dictionary<AnimParm, string> kAnimParmInfo = new Dictionary<AnimParm, string>()
    {
        { AnimParm.kAttack, "Attack" },
        { AnimParm.kHit,    "Hit" },
        { AnimParm.kDeath,  "Death" },
        { AnimParm.kMove,   "IsMove" },
    };

    /* 変数 */
    [SerializeField] protected EnemyStatus _status;    // ステータス
    private int _hp;                // 現在のHP
    private int _deltaHp;           // 減少を分かりやすくするための変数
    private int _watiAttackFrame;   // 攻撃待機フレーム用
    private bool _isExist;          // 生存フラグ
    private bool _isDeltaHp;        // 減少フラグ
    protected bool _isStopAttack;     // 攻撃停止フラグ
    protected bool _isFindPlayer;     // プレイヤー発見フラグ
    protected bool _isStopMove;       // 停止フラグ
    protected bool _isColPlayer;
    protected bool _isColFarm;
    private bool _isDeathAnim;
    protected Rigidbody _rb;
    private GameObject _farmBase;   // 農場全部を持っている親オブジェクト
    protected GameObject _farm;     // 攻撃する農場
    protected Farm _farmScript;     // 上ののスクリプト
    protected GameObject _player;   // プレイヤー
    private CameraControl _camera;  // カメラ情報
    private Minimap _minimap;       // ミニマップ
    private SpawnerManager _spawnerMgr; // スポナーマネージャー
    protected Animator _anim;

    private SoundManager _soundMgr;

    /* プロパティ */
    public int Hp { get { return _hp; } }
    public int DeltaHp { get { return _deltaHp; } }
    public int MaxHp { get { return _status.maxHp; } }
    public bool IsExist { get { return  _isExist; } }
    public bool IsFindPlayer { get { return _isFindPlayer; } set { _isFindPlayer = value; } }

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="pos">初期位置</param>
    /// <param name="enemyNo">敵のID</param>
    public virtual void Init(Vector3 pos, string enemyID)
    {
        _rb = GetComponent<Rigidbody>();

        _farmBase = GameObject.Find("Farm").gameObject;
        _player = GameObject.Find("Player").gameObject;
        _camera = GameObject.Find("Main Camera").GetComponent<CameraControl>();
        var stageMgr = GameObject.Find("StageManager");
        _minimap = stageMgr.GetComponent<Minimap>();
        _spawnerMgr = stageMgr.GetComponent<SpawnerManager>();
        _anim = GetComponent<Animator>();
        _anim.speed = 0.2f;
        _anim.SetBool(kAnimParmInfo[AnimParm.kMove], true);

        // ステータス取得
        var director = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        _soundMgr = director.SoundMgr;
        EnemyData data = director.DataMgr.Enemy;
        _status = data.GetStatus(enemyID);

        // 各種初期化
        _hp = _status.maxHp;
        _deltaHp = _status.maxHp;
        _watiAttackFrame = 0;
        _isExist = true;
        _isDeltaHp = false;
        _isStopAttack = false;
        _isFindPlayer = false;
        _isStopMove = false;
        _isColPlayer = false;
        _isColFarm = false;
        _isDeathAnim = false;
        transform.position = pos;
    }

    protected void OnCollisionEnter(Collision collision)
    {
        bool isPlayer = collision.gameObject.tag == "Player";
        bool isFarm = collision.gameObject.tag == "Farm";

        if (isPlayer || isFarm)
        {
            // どちらとも当たったことがなければ移動アニメーション終了
            if (!_isColPlayer && !_isColFarm)
            {
                _anim.SetBool(kAnimParmInfo[AnimParm.kMove], false);
            }

            if (isPlayer)
            {
                _isStopMove = true;
                _isColPlayer = true;
            }
            else if (isFarm)
            {
                _isColFarm = true;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        bool isPlayer = collision.gameObject.tag == "Player";
        bool isFarm = collision.gameObject.tag == "Farm";

        if (isPlayer || isFarm) 
        {
            if (isPlayer)
            {
                _isStopMove = false;
                _isColPlayer = false;
            }
            else if (isFarm)
            {
                _isColFarm = false;
            }

            // どちらとも離れたら移動を開始させる
            if (!_isColPlayer && !_isColFarm)
            {
                _anim.SetBool(kAnimParmInfo[AnimParm.kMove], true);
            }
        }
    }

    protected void FrontUpdate()
    {
        if (_rb.velocity.sqrMagnitude > 0.0f)
        {
            Vector3 dir = _rb.velocity.normalized;

            transform.forward = dir;
        }
    }

    protected bool DeathAfterUpdate()
    {
        // 生存時は無視
        if (_isExist) return false;

        if (IsNowPlayClipName("Death"))
        {
            _isDeathAnim = true;
        }
        else if (_isDeathAnim)
        {
            // カメラに死亡したことを伝える
            _camera.SubHpBarInfo(this.gameObject);
            // スポナーマネージャーに死亡したことを伝える
            _spawnerMgr.AddKilledEnemy();
            //ミニマップに死亡したことを伝える
            _minimap.EntryDeathEnemyList(this.gameObject);
            // 自身を破壊
            Destroy(this.gameObject);
        }

        return true;
    }

    protected bool IsNowPlayClipName(string clipName)
    {
        return _anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == clipName;
    }

    /// <summary>
    /// ダメージ処理
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    public void OnDamage(int damage)
    {
        _hp -= damage;
        Debug.Log("hp" + _hp);
        _isDeltaHp = true;
        _anim.SetTrigger(kAnimParmInfo[AnimParm.kHit]);

        _soundMgr.PlaySe("SE_10");

        // HPが0以下になったら死亡処理
        if (_hp <= 0)
        {
            _hp = 0;
            _isExist = false;
            _anim.SetTrigger(kAnimParmInfo[AnimParm.kDeath]);
            _anim.SetBool("Is" + kAnimParmInfo[AnimParm.kDeath], true);
        }
    }

    /// <summary>
    /// 農場に向かっての移動
    /// </summary>
    protected void MoveToFarm()
    {
        Vector3 pos = transform.position;
        Vector3 farmPos = _farm.transform.position;

        Vector3 velocity = (farmPos - pos).normalized * _status.speed;

        _rb.velocity = velocity;
    }

    /// <summary>
    /// プレイヤーに向かっての移動
    /// </summary>
    protected void MoveToPlayer()
    {
        Vector3 pos = transform.position;
        Vector3 playerPos = _player.transform.position;

        Vector3 velocity = (playerPos - pos).normalized * _status.speed;

        _rb.velocity = velocity;
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

        _soundMgr.PlaySe("SE_9");
        _farmScript.OnDamage(_status.attack);
        _isStopAttack = true;
        _watiAttackFrame = _status.attackInterval;
        _anim.SetTrigger(kAnimParmInfo[AnimParm.kAttack]);
    }

    protected void AttackPlayer()
    {
        // 攻撃待機状態ならしない
        if (_isStopAttack) return;
        // プレイヤー非発見時なら攻撃しない
        if (!_isFindPlayer) return;

        _soundMgr.PlaySe("SE_9");
        _player.GetComponent<Player>().OnDamage(_status.attack);
        _isStopAttack = true;
        _watiAttackFrame = _status.attackInterval;
        _anim.SetTrigger(kAnimParmInfo[AnimParm.kAttack]);
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
