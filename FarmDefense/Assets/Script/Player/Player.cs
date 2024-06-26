using System.Collections;
using System.Collections.Generic;
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
    enum WeaponType
    {
        kNear,
        kFar
    }

    /* 定数 */
    private const int kDecreaseHpSpeed = 1;   // _deltaHpの減少速度
    private Vector3 kInitPos = Vector3.zero;

    /* 変数 */
    // プレイヤー情報
    private Rigidbody _rb;
    private CameraControl _camera;
    private PlayerStatus _status;
    private StaminaCost _cost;
    private PlayerTime _time;
    private int _hp;
    private int _deltaHp;
    private int _stamina;
    private int _stanTime;
    private int _safeTime;
    private bool _isDash;
    private bool _isTired;
    private bool _isJump;
    private bool _isStan;
    private bool _isSafe;
    private bool _isDeltaHp;
    private Vector3 _jumpVelocity;
    private Vector3 _velocity;
    // 武器情報
    private SwordStatus _swordStatus;
    private BulletStatus _bulletStatus;
    private WeaponType _nowWeaponType;
    private GameObject _weapon;
    [SerializeField] private GameObject _weaponArm;
    [SerializeField] private GameObject _sword;
    [SerializeField] private GameObject _gun;
    [SerializeField] private GameObject _bullet;
    
    void Start()
    {
        // 各種取得
        _rb = GetComponent<Rigidbody>();
        _camera = GameObject.Find("Main Camera").GetComponent<CameraControl>();
        var dataMgr = GameObject.Find("DataManager").GetComponent<DataManager>();
        var player = dataMgr.Player;
        _status = player.Status;
        _cost = player.Cost;
        _time = player.Time;
        var user = dataMgr.User;
        var weapon = dataMgr.Weapon;
        _swordStatus = new SwordStatus();
        _bulletStatus = new BulletStatus();
        _swordStatus.attack    = weapon.GetStatus("W_0", user.GetWeaponLv(0, "W_0"));
        _swordStatus.interval  = weapon.GetStatus("W_1", user.GetWeaponLv(0, "W_1"));
        _swordStatus.range     = weapon.GetStatus("W_2", user.GetWeaponLv(0, "W_2"));
        _bulletStatus.attack   = weapon.GetStatus("W_3", user.GetWeaponLv(0, "W_3"));
        _bulletStatus.interval = weapon.GetStatus("W_4", user.GetWeaponLv(0, "W_4"));
        _bulletStatus.speed    = weapon.GetStatus("W_5", user.GetWeaponLv(0, "W_5"));
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
        if (Input.GetButtonDown("X"))
        {
            OnAttack();
        }
        // 強攻撃
        else if (Input.GetButtonDown("Y"))
        {
            OnStrongAttack();
        }
    }

    private void FixedUpdate()
    {
        // ジャンプ処理
        Jump();
        // スタン時
        if (_isStan)
        {
            // スタミナ処理
            Stamina();
        }
        // 非スタン時
        else
        {
            // スタン処理
            StanTime();
        }
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
            Vector3 pos = transform.position;
            pos.y = collision.transform.position.y + 1;
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
        _stanTime = _time.stan;
        _safeTime = _time.hitSafe;
    }

    private void Init()
    {
        this.transform.position = kInitPos;
        _hp = _status.maxHp;
        _deltaHp = _hp;
        _stamina = _status.maxStamina;
        _stanTime = 0;
        _safeTime = _time.revivalSafe;
        _isDash = false;
        _isTired = false;
        _isJump = false;
        _isStan = false;
        _isSafe = true;
        _jumpVelocity = Vector3.zero;
        _velocity = Vector3.zero;
    }

    /// <summary>
    /// 通常攻撃
    /// </summary>
    private void OnAttack()
    {
        // TODO: 通常攻撃
        if (_nowWeaponType == WeaponType.kNear)
        {

        }
        else if (_nowWeaponType == WeaponType.kFar)
        {
            // 弾の生成
            var bullet = Instantiate(_bullet, _weapon.transform.position, Quaternion.identity);

            Vector3 velocity = transform.forward;
            velocity.y = _camera.GetFront().y;
            velocity = velocity.normalized * _bulletStatus.speed;

            bullet.GetComponent<Bullet>().Init(_bulletStatus.attack, velocity);
        }
    }

        /// <summary>
        /// 強攻撃
        /// </summary>
        private void OnStrongAttack()
    {
        // 消費後のスタミナ確認
        int temp = _stamina - _cost.strongAttack;
        // 実行できるか
        bool isDo = temp >= 0;
        // 実行
        if (isDo)
        {
            _stamina = temp;
            // TODO: 強攻撃
        }
        else
        {
            // TODO: 余裕があれば「足りない」というテキストを出すように
        }
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
            _nowWeaponType = WeaponType.kFar;
            _weapon = Instantiate(_gun, _weaponArm.transform);
        }
        // 現在遠距離武器の場合
        else if (_nowWeaponType == WeaponType.kFar)
        {
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

        _isJump = true;
        _jumpVelocity.y = _status.jumpPower;
    }

    /// <summary>
    /// アイテムの使用
    /// </summary>
    private void UseItem()
    {
        // TODO: アイテム使用
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
            return;
        }
        
        // ダッシュ
        _isDash = false;
        if (Input.GetAxis("RT") > 0.0f)
        {
            OnDash();
        }

        velocity.Normalize();
        // 方向変換
        transform.rotation = Quaternion.LookRotation(velocity);

        // 速度調整
        // ダッシュ時
        if (_isDash)
        {
            velocity *= _status.dashSpeed;
        }
        // 疲れているとき
        else if (_isTired)
        {
            velocity *= _status.downSpeed;
        }
        // 通常時
        else
        {
            velocity *= _status.speed;
        }

        _velocity = velocity;
    }

    /// <summary>
    /// ダッシュ
    /// </summary>
    private void OnDash()
    {
        // 疲れていなければダッシュ状態に
        if (!_isTired)
        {
            _isDash = true;
        }
    }

    private void Jump()
    {
        // ジャンプ中でないなら無視
        if (!_isJump) return;

        transform.position += _jumpVelocity;
        _jumpVelocity.y += _status.fallPower;
    }

    private void Stamina()
    {
        // 疲れていないとき
        if (!_isTired)
        {
            // ダッシュ時スタミナ減らす
            if (_isDash)
            {
                _stamina -= _cost.dash;
            }
            // スタミナが0未満になったら
            if (_stamina <= 0)
            {
                _stamina = 0;
                _isTired = true;
                _isDash = false;
                return;
            }
            // スタミナが最大でない場合
            if (_stamina < _status.maxStamina)
            {
                // スタミナ回復
                _stamina += _status.recoveryStamina;
                // 最大値を超えたら補正
                _stamina = Mathf.Min(_stamina, _status.maxStamina);
            }
        }
        // 疲れているとき
        else
        {
            // 通常時の2倍の速度でスタミナ回復
            _stamina += _status.recoveryStamina * 2;
            // 最大値まで回復したら通常状態へ
            if (_stamina >= _status.maxStamina)
            {
                _stamina = _status.maxStamina;
                _isTired = false;
            }
        }
    }

    private void StanTime()
    {
        --_stanTime;

        if (_stanTime < 0)
        {
            _isStan = false;
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
