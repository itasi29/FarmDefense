using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour
{
    struct SwordStatus
    {
        public int attack;
        public int interval;
        public float range;
    }
    struct BulletStatus
    {
        public int attack;
        public int interval;
        public float speed;
    }
    public enum WeaponType
    {
        kNear,
        kFar
    }
    protected enum AnimParm
    {
        kAttack,
        kStAttack,
        kJump,
        kJumpEnd,
        kHit,
        kWin,
        kMove,
        kDash,
        kJumpAir,
        kExist,
        kNowPlaying,
    };

    /* 定数 */
    private const int kDecreaseHpSpeed = 1;   // _deltaHpの減少速度
    private Vector3 kInitPos = Vector3.zero;
    // アニメーションの動きを制御
    private Dictionary<AnimParm, string> kAnimParmInfo = new Dictionary<AnimParm, string>()
    {
        { AnimParm.kAttack,     "Attack" },
        { AnimParm.kStAttack,   "StrongAttack" },
        { AnimParm.kJump,       "Jump" },
        { AnimParm.kJumpEnd,    "JumpEnd" },
        { AnimParm.kHit,        "Hit" },
        { AnimParm.kWin,        "Win" },
        { AnimParm.kMove,       "IsMove" },
        { AnimParm.kDash,       "IsDash" },
        { AnimParm.kJumpAir,    "IsJumpAir" },
        { AnimParm.kExist,      "IsExist" },
        { AnimParm.kNowPlaying, "IsNowPlaying" },
    };
    /* ステータス系 */
    private const int kMaxHp = 100;             // 最大HP
    private const int kMaxStamina = 500;        // 最大スタミナ
    private const float kSpeed = 16.0f;         // 通常スピード
    private const float kDashSpeed = 30.0f;     // ダッシュスピード
    private const float kDownSpeed = 5.0f;      // 低速時スピード
    private const float kJumpPower = 0.8f;      // ジャンプ力
    private const float kFallSpeed = -0.04f;    // 落下スピード
    /* コスト系 */
    private const int kDashCost = 5;
    private const int kStAttackCost = 50;
    /* 時間系 */
    private const int kStanTime = 30;       // スタン
    private const int kHitSafeTime = 40;    // ヒット時無敵
    private const int kRevivalSafeTime = 60;    // 復活時無敵
    /* その他 */
    private const int kRecoveryStaminaSpeed = 2;     // スタミナ回復速度
    private const float kMaxFallSpeed = -0.1f;  // 最大落下速度
    private const int kAddStrongAttack = 10;    // 基礎強攻撃追加ダメージ量
    private const float kRateStrongAttackInterval = 1.25f;  // 強攻撃追加フレーム割合

    /* 変数 */
    // プレイヤー情報
    private Animator _anim;
    private Rigidbody _rb;
    private CameraControl _camera;
    private int _hp;                    // 現在のHP
    private int _deltaHp;               // 減少HP
    private int _stamina;               // 現在のスタミナ
    private int _waitAttackTime;       // 攻撃停止時間
    private int _stanTime;              // スタン時間
    private int _safeTime;              // 無敵時間
    private string _stopAnimName;       // 停止確認アニメ名
    private bool _isNowAnimCheckName;   // 停止アニメ名がプレイされたか
    private bool _isCheckAnimEnd;       // アニメ停止確認するか
    private bool _isCanAttack;          // 攻撃可能
    private bool _isStopMove;           // 移動停止
    private bool _isDash;               // ダッシュ
    private bool _isTired;              // 疲れ
    private bool _isJump;               // ジャンプ
    private bool _isStan;               // スタン
    private bool _isSafe;               // 無敵
    private bool _isDeltaHp;            // 減少
    private Vector3 _jumpVelocity;      // ジャンプ力
    private Vector3 _velocity;          // 移動力
    // 武器情報
    private SwordStatus _swordStatus;
    private BulletStatus _bulletStatus;
    private WeaponType _nowWeaponType;
    private GameObject _weapon;
    [SerializeField] private GameObject _weaponArm;
    [SerializeField] private GameObject _sword;
    [SerializeField] private GameObject _gun;
    [SerializeField] private GameObject _bullet;

    /* プロパティ */
    public WeaponType NowWeaponType { get { return _nowWeaponType; } }
    
    void Start()
    {
        // 各種取得
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        _camera = GameObject.Find("Main Camera").GetComponent<CameraControl>();
        var dataMgr = GameObject.Find("DataManager").GetComponent<DataManager>();
        var user = dataMgr.User;
        var weapon = dataMgr.Weapon;
        _swordStatus = new SwordStatus();
        _bulletStatus = new BulletStatus();
        _swordStatus.attack    = weapon.GetStatus("W_0", user.GetWeaponLv("W_0"));
        _swordStatus.interval  = weapon.GetStatus("W_1", user.GetWeaponLv("W_1"));
        _swordStatus.range     = weapon.GetStatus("W_2", user.GetWeaponLv("W_2"));
        _bulletStatus.attack   = weapon.GetStatus("W_3", user.GetWeaponLv("W_3"));
        _bulletStatus.interval = weapon.GetStatus("W_4", user.GetWeaponLv("W_4"));
        _bulletStatus.speed    = weapon.GetStatus("W_5", user.GetWeaponLv("W_5"));
        _nowWeaponType = WeaponType.kNear;
        _weapon = Instantiate(_sword, _weaponArm.transform);

        // 初期化
        Init();
    }

    private void Update()
    {
        // スタン時は行動不可
        if (_isStan) return;

        // 移動
        Move();
        // ジャンプ
        if (Input.GetButtonDown("A"))
        {
            OnJump();
        }
        // アイテム使用
        if (Input.GetButtonDown("B"))
        {
            UseItem();
        }
        // 武器切り替え
        else if (Input.GetButtonDown("RB"))
        {
            ChangeWeapon();
        }
        // 通常攻撃
        if (Input.GetButtonDown("X") && _isCanAttack)
        {
            OnAttack();
        }
        // 強攻撃
        else if (Input.GetButtonDown("Y") && _isCanAttack)
        {
            OnStrongAttack();
        }
    }

    private void FixedUpdate()
    {
        // ストップ終了確認処理
        if (CheckStopAnimEnd())
        {
            if (_isStopMove)
            {
                _isStopMove = false;
                if (_nowWeaponType == WeaponType.kNear)
                {
                    _weapon.GetComponent<Sword>().OffAttack();
                }
            }
        }

        // ジャンプ処理
        Jump();
        // スタン処理
        StanTime();
        // スタミナ処理
        Stamina();
        // 攻撃待機処理
        AttackWaitTime();
        // 無敵処理
        SafeTime();
        // DeltaHp処理
        ReduceDeltaHp();

        // 移動速度適用
        _rb.velocity = _velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Ground")
        {
            _isJump = false;
            _anim.SetTrigger(kAnimParmInfo[AnimParm.kJumpEnd]);
            _anim.SetBool(kAnimParmInfo[AnimParm.kJumpAir], false);
            SetStopAnimInfo(kAnimParmInfo[AnimParm.kJumpEnd]);
            _isStopMove = true;
            Vector3 pos = transform.position;
            pos.y = collision.transform.position.y + 0.5f;
            transform.position = pos;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            EnemyBase enemy = other.gameObject.GetComponent<EnemyBase>();
            enemy.IsFindPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            EnemyBase enemy = other.gameObject.GetComponent<EnemyBase>();
            enemy.IsFindPlayer = false;
        }
    }

    public void OnDamage(int damage)
    {
        // 無敵状態なら攻撃受けない
        if (_isSafe) return;

        //体力を減らす
        _hp -= damage;
        _isDeltaHp = true;

        if (_hp < 0)
        {
            Init();
            return;
        }

        // スタン状態に
        _isStan = true;
        // 無敵状態に
        _isSafe = true;
        // スタン・無敵時間の設定
        _stanTime = kStanTime;
        _safeTime = kHitSafeTime;
    }

    private void Init()
    {
        this.transform.position = kInitPos;
        _hp = kMaxHp;
        _deltaHp = _hp;
        _stamina = kMaxStamina;
        _stanTime = 0;
        _safeTime = kRevivalSafeTime;
        _waitAttackTime = 0;
        _isCanAttack = true;
        _isDash = false;
        _isTired = false;
        _isJump = false;
        _isStan = false;
        _isSafe = true;
        _jumpVelocity = Vector3.zero;
        _velocity = Vector3.zero;
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        // カメラの方向を取得
        Vector3 cameraRight = _camera.GetRight();
        Vector3 cameraFront = _camera.GetFront();
        cameraFront.y = 0;
        cameraFront.Normalize();
        // スティックの移動量を取得
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        // カメラの向きに合わせて移動するように
        Vector3 velocity = cameraFront * z + cameraRight * x;

        // 動いて無ければ速度0にして処理終了
        if (velocity.sqrMagnitude == 0.0f)
        {
            _velocity = Vector3.zero;
            _anim.SetBool(kAnimParmInfo[AnimParm.kMove], false);
            return;
        }

        velocity.Normalize();
        // 方向変換
        transform.rotation = Quaternion.LookRotation(velocity);

        // 移動停止中なら
        if (_isStopMove)
        {
            // ジャンプしている
            if (_isJump)
            {
                // 速度を通常の速度に
                _velocity = velocity * kSpeed;
            }
            // ジャンプしていない
            else
            {
                // 移動停止
                _velocity = Vector3.zero;
            }
            // 移動処理終了
            return;
        }

        // 移動モーション有効化
        _anim.SetBool(kAnimParmInfo[AnimParm.kMove], true);
        // ダッシュ
        _isDash = Input.GetAxis("RT") > 0.0f && !_isTired;
        if (_nowWeaponType == WeaponType.kNear)
        {
            _anim.SetBool(kAnimParmInfo[AnimParm.kDash], _isDash);
        }


        // 速度調整
        // ダッシュ時
        if (_isDash)
        {
            velocity *= kDashSpeed;
        }
        // 疲れているとき
        else if (_isTired)
        {
            velocity *= kDownSpeed;
        }
        // 通常時
        else
        {
            velocity *= kSpeed;
        }

        _velocity = velocity;
    }

    /// <summary>
    /// 通常攻撃
    /// </summary>
    private void OnAttack()
    {
        _anim.SetTrigger(kAnimParmInfo[AnimParm.kAttack]);

        // TODO: 通常攻撃
        if (_nowWeaponType == WeaponType.kNear)
        {
            // 待機時間適用
            _waitAttackTime = _swordStatus.interval;
            // 攻撃力適用
            _weapon.GetComponent<Sword>().OnAttack(_swordStatus.attack);
        }
        else if (_nowWeaponType == WeaponType.kFar)
        {
            // 待機時間適用
            _waitAttackTime = _bulletStatus.interval;
            // 弾の生成
            var bullet = Instantiate(_bullet, _weapon.transform.position, Quaternion.identity);
            // 方向・速度適用
            Vector3 velocity = transform.forward;
            velocity.y = _camera.GetFront().y;
            velocity = velocity.normalized * _bulletStatus.speed;
            // 弾に設定
            bullet.GetComponent<Bullet>().Init(_bulletStatus.attack, velocity);
        }
        _isCanAttack = false;

        StopMove(kAnimParmInfo[AnimParm.kAttack]);
    }

    /// <summary>
    /// 強攻撃
    /// </summary>
    private void OnStrongAttack()
    {
        // 遠距離攻撃には強攻撃なし
        if (_nowWeaponType == WeaponType.kFar) return;

        // 消費後のスタミナ確認
        int temp = _stamina - kStAttackCost;
        // 実行できるか
        bool isDo = temp >= 0;
        // 実行
        if (isDo)
        {
            // アニメ再生
            StopMove(kAnimParmInfo[AnimParm.kStAttack]);
            _anim.SetTrigger(kAnimParmInfo[AnimParm.kStAttack]);
            // スタミナ適用
            _stamina = temp;
            // 待機時間適用
            _isCanAttack = false;
            _waitAttackTime = (int)(_swordStatus.interval * kRateStrongAttackInterval);
            // 攻撃力適用
            int attack = _swordStatus.attack + kAddStrongAttack;
            _weapon.GetComponent<Sword>().OnAttack(attack);
        }
        else
        {
            // TODO: 余裕があれば「足りない」というテキストを出すように
        }
    }

    private void StopMove(string animName)
    {
        _anim.SetBool(kAnimParmInfo[AnimParm.kMove], false);
        _isStopMove = true;
        _isDash = false;
        SetStopAnimInfo(animName);
    }

    private void SetStopAnimInfo(string animName)
    {
        _stopAnimName = animName;
        _isNowAnimCheckName = false;
        _isCheckAnimEnd = true;
    }

    /// <summary>
    /// 武器の変更
    /// </summary>
    private void ChangeWeapon()
    {
        // 現在所持している武器を削除
        Destroy(_weapon);

        // 現在近接武器の場合
        if (_nowWeaponType == WeaponType.kNear)
        {
            _anim.SetBool("IsSword", false);
            _anim.SetBool("IsGun", true);
            _nowWeaponType = WeaponType.kFar;
            _weapon = Instantiate(_gun, _weaponArm.transform);
        }
        // 現在遠距離武器の場合
        else if (_nowWeaponType == WeaponType.kFar)
        {
            _anim.SetBool("IsGun", false);
            _anim.SetBool("IsSword", true);
            _nowWeaponType = WeaponType.kNear;
            _weapon = Instantiate(_sword, _weaponArm.transform);
        }
    }

    /// <summary>
    /// ジャンプ状態遷移
    /// </summary>
    private void OnJump()
    {
        // ジャンプ中は無視
        if (_isJump) return;

        _anim.SetTrigger(kAnimParmInfo[AnimParm.kJump]);
        _anim.SetBool(kAnimParmInfo[AnimParm.kJumpAir], true);
        _isJump = true;
        _jumpVelocity.y = kJumpPower;
    }

    /// <summary>
    /// アイテムの使用
    /// </summary>
    private void UseItem()
    {
        // TODO: アイテム使用
    }

    private bool CheckStopAnimEnd()
    {
        if (!_isCheckAnimEnd) return false;

        if (_anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == _stopAnimName)
        {
            _isNowAnimCheckName = true;
        }
        else if (_isNowAnimCheckName)
        {
            _isCheckAnimEnd = false;
        }

        // MEMO: 反転したものを返すことで終了時の1フレームだけtrueにする
        return !_isCheckAnimEnd;
    }

    private void Jump()
    {
        // ジャンプ中でないなら無視
        if (!_isJump) return;

        transform.position += _jumpVelocity;
        _jumpVelocity.y += kFallSpeed;
        // 最低速度未満になったら補正
        if (_jumpVelocity.y < kMaxFallSpeed)
        {
            _jumpVelocity.y = kMaxFallSpeed;
        }
    }

    private void Stamina()
    {
        // 疲れていないとき
        if (!_isTired)
        {
            // ダッシュ時スタミナ減らす
            if (_isDash)
            {
                _stamina -= kDashCost;
            }
            // スタミナが0未満になったら
            if (_stamina <= 0)
            {
                _stamina = 0;
                _isTired = true;
                if (_isDash)
                {
                    _isDash = false;
                    _anim.SetBool(kAnimParmInfo[AnimParm.kDash], false);
                }
                return;
            }
            // スタミナが最大でない場合
            if (_stamina < kMaxStamina)
            {
                // スタミナ回復
                _stamina += kRecoveryStaminaSpeed;
                // 最大値を超えたら補正
                _stamina = Mathf.Min(_stamina, kMaxStamina);
            }
        }
        // 疲れているとき
        else
        {
            // 通常時の2倍の速度でスタミナ回復
            _stamina += kRecoveryStaminaSpeed * 2;
            // 最大値まで回復したら通常状態へ
            if (_stamina >= kMaxStamina)
            {
                _stamina = kMaxStamina;
                _isTired = false;
            }
        }
    }

    private void StanTime()
    {
        // スタン状態でないなら無視
        if (!_isStan) return;

        Debug.Log(_stanTime);
        --_stanTime;

        if (_stanTime < 0)
        {
            _isStan = false;
        }
    }

    private void AttackWaitTime()
    {
        // 攻撃可能状態なら無視
        if (_isCanAttack) return;

        --_waitAttackTime;

        if (_waitAttackTime < 0)
        {
            _isCanAttack = true;
        }
    }

    private void SafeTime()
    {
        // 無敵状態でないなら無視
        if (!_isSafe) return;

        --_safeTime;

        if (_safeTime < 0)
        {
            _isSafe = false;
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
}
