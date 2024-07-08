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

    /* �萔 */
    private const int kDecreaseHpSpeed = 1;   // _deltaHp�̌������x
    private Vector3 kInitPos = Vector3.zero;
    // �A�j���[�V�����̓����𐧌�
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
    /* �X�e�[�^�X�n */
    private const int kMaxHp = 100;             // �ő�HP
    private const int kMaxStamina = 500;        // �ő�X�^�~�i
    private const float kSpeed = 16.0f;         // �ʏ�X�s�[�h
    private const float kDashSpeed = 30.0f;     // �_�b�V���X�s�[�h
    private const float kDownSpeed = 5.0f;      // �ᑬ���X�s�[�h
    private const float kJumpPower = 0.8f;      // �W�����v��
    private const float kFallSpeed = -0.04f;    // �����X�s�[�h
    /* �R�X�g�n */
    private const int kDashCost = 5;
    private const int kStAttackCost = 50;
    /* ���Ԍn */
    private const int kStanTime = 30;       // �X�^��
    private const int kHitSafeTime = 40;    // �q�b�g�����G
    private const int kRevivalSafeTime = 60;    // ���������G
    /* ���̑� */
    private const int kRecoveryStaminaSpeed = 2;     // �X�^�~�i�񕜑��x
    private const float kMaxFallSpeed = -0.1f;  // �ő嗎�����x
    private const int kAddStrongAttack = 10;    // ��b���U���ǉ��_���[�W��
    private const float kRateStrongAttackInterval = 1.25f;  // ���U���ǉ��t���[������

    /* �ϐ� */
    // �v���C���[���
    private Animator _anim;
    private Rigidbody _rb;
    private CameraControl _camera;
    private int _hp;                    // ���݂�HP
    private int _deltaHp;               // ����HP
    private int _stamina;               // ���݂̃X�^�~�i
    private int _waitAttackTime;       // �U����~����
    private int _stanTime;              // �X�^������
    private int _safeTime;              // ���G����
    private string _stopAnimName;       // ��~�m�F�A�j����
    private bool _isNowAnimCheckName;   // ��~�A�j�������v���C���ꂽ��
    private bool _isCheckAnimEnd;       // �A�j����~�m�F���邩
    private bool _isCanAttack;          // �U���\
    private bool _isStopMove;           // �ړ���~
    private bool _isDash;               // �_�b�V��
    private bool _isTired;              // ���
    private bool _isJump;               // �W�����v
    private bool _isStan;               // �X�^��
    private bool _isSafe;               // ���G
    private bool _isDeltaHp;            // ����
    private Vector3 _jumpVelocity;      // �W�����v��
    private Vector3 _velocity;          // �ړ���
    // ������
    private SwordStatus _swordStatus;
    private BulletStatus _bulletStatus;
    private WeaponType _nowWeaponType;
    private GameObject _weapon;
    [SerializeField] private GameObject _weaponArm;
    [SerializeField] private GameObject _sword;
    [SerializeField] private GameObject _gun;
    [SerializeField] private GameObject _bullet;

    /* �v���p�e�B */
    public WeaponType NowWeaponType { get { return _nowWeaponType; } }
    
    void Start()
    {
        // �e��擾
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

        // ������
        Init();
    }

    private void Update()
    {
        // �X�^�����͍s���s��
        if (_isStan) return;

        // �ړ�
        Move();
        // �W�����v
        if (Input.GetButtonDown("A"))
        {
            OnJump();
        }
        // �A�C�e���g�p
        if (Input.GetButtonDown("B"))
        {
            UseItem();
        }
        // ����؂�ւ�
        else if (Input.GetButtonDown("RB"))
        {
            ChangeWeapon();
        }
        // �ʏ�U��
        if (Input.GetButtonDown("X") && _isCanAttack)
        {
            OnAttack();
        }
        // ���U��
        else if (Input.GetButtonDown("Y") && _isCanAttack)
        {
            OnStrongAttack();
        }
    }

    private void FixedUpdate()
    {
        // �X�g�b�v�I���m�F����
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

        // �W�����v����
        Jump();
        // �X�^������
        StanTime();
        // �X�^�~�i����
        Stamina();
        // �U���ҋ@����
        AttackWaitTime();
        // ���G����
        SafeTime();
        // DeltaHp����
        ReduceDeltaHp();

        // �ړ����x�K�p
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
        // ���G��ԂȂ�U���󂯂Ȃ�
        if (_isSafe) return;

        //�̗͂����炷
        _hp -= damage;
        _isDeltaHp = true;

        if (_hp < 0)
        {
            Init();
            return;
        }

        // �X�^����Ԃ�
        _isStan = true;
        // ���G��Ԃ�
        _isSafe = true;
        // �X�^���E���G���Ԃ̐ݒ�
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
    /// �ړ�
    /// </summary>
    private void Move()
    {
        // �J�����̕������擾
        Vector3 cameraRight = _camera.GetRight();
        Vector3 cameraFront = _camera.GetFront();
        cameraFront.y = 0;
        cameraFront.Normalize();
        // �X�e�B�b�N�̈ړ��ʂ��擾
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        // �J�����̌����ɍ��킹�Ĉړ�����悤��
        Vector3 velocity = cameraFront * z + cameraRight * x;

        // �����Ė�����Α��x0�ɂ��ď����I��
        if (velocity.sqrMagnitude == 0.0f)
        {
            _velocity = Vector3.zero;
            _anim.SetBool(kAnimParmInfo[AnimParm.kMove], false);
            return;
        }

        velocity.Normalize();
        // �����ϊ�
        transform.rotation = Quaternion.LookRotation(velocity);

        // �ړ���~���Ȃ�
        if (_isStopMove)
        {
            // �W�����v���Ă���
            if (_isJump)
            {
                // ���x��ʏ�̑��x��
                _velocity = velocity * kSpeed;
            }
            // �W�����v���Ă��Ȃ�
            else
            {
                // �ړ���~
                _velocity = Vector3.zero;
            }
            // �ړ������I��
            return;
        }

        // �ړ����[�V�����L����
        _anim.SetBool(kAnimParmInfo[AnimParm.kMove], true);
        // �_�b�V��
        _isDash = Input.GetAxis("RT") > 0.0f && !_isTired;
        if (_nowWeaponType == WeaponType.kNear)
        {
            _anim.SetBool(kAnimParmInfo[AnimParm.kDash], _isDash);
        }


        // ���x����
        // �_�b�V����
        if (_isDash)
        {
            velocity *= kDashSpeed;
        }
        // ���Ă���Ƃ�
        else if (_isTired)
        {
            velocity *= kDownSpeed;
        }
        // �ʏ펞
        else
        {
            velocity *= kSpeed;
        }

        _velocity = velocity;
    }

    /// <summary>
    /// �ʏ�U��
    /// </summary>
    private void OnAttack()
    {
        _anim.SetTrigger(kAnimParmInfo[AnimParm.kAttack]);

        // TODO: �ʏ�U��
        if (_nowWeaponType == WeaponType.kNear)
        {
            // �ҋ@���ԓK�p
            _waitAttackTime = _swordStatus.interval;
            // �U���͓K�p
            _weapon.GetComponent<Sword>().OnAttack(_swordStatus.attack);
        }
        else if (_nowWeaponType == WeaponType.kFar)
        {
            // �ҋ@���ԓK�p
            _waitAttackTime = _bulletStatus.interval;
            // �e�̐���
            var bullet = Instantiate(_bullet, _weapon.transform.position, Quaternion.identity);
            // �����E���x�K�p
            Vector3 velocity = transform.forward;
            velocity.y = _camera.GetFront().y;
            velocity = velocity.normalized * _bulletStatus.speed;
            // �e�ɐݒ�
            bullet.GetComponent<Bullet>().Init(_bulletStatus.attack, velocity);
        }
        _isCanAttack = false;

        StopMove(kAnimParmInfo[AnimParm.kAttack]);
    }

    /// <summary>
    /// ���U��
    /// </summary>
    private void OnStrongAttack()
    {
        // �������U���ɂ͋��U���Ȃ�
        if (_nowWeaponType == WeaponType.kFar) return;

        // �����̃X�^�~�i�m�F
        int temp = _stamina - kStAttackCost;
        // ���s�ł��邩
        bool isDo = temp >= 0;
        // ���s
        if (isDo)
        {
            // �A�j���Đ�
            StopMove(kAnimParmInfo[AnimParm.kStAttack]);
            _anim.SetTrigger(kAnimParmInfo[AnimParm.kStAttack]);
            // �X�^�~�i�K�p
            _stamina = temp;
            // �ҋ@���ԓK�p
            _isCanAttack = false;
            _waitAttackTime = (int)(_swordStatus.interval * kRateStrongAttackInterval);
            // �U���͓K�p
            int attack = _swordStatus.attack + kAddStrongAttack;
            _weapon.GetComponent<Sword>().OnAttack(attack);
        }
        else
        {
            // TODO: �]�T������΁u����Ȃ��v�Ƃ����e�L�X�g���o���悤��
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
    /// ����̕ύX
    /// </summary>
    private void ChangeWeapon()
    {
        // ���ݏ������Ă��镐����폜
        Destroy(_weapon);

        // ���݋ߐڕ���̏ꍇ
        if (_nowWeaponType == WeaponType.kNear)
        {
            _anim.SetBool("IsSword", false);
            _anim.SetBool("IsGun", true);
            _nowWeaponType = WeaponType.kFar;
            _weapon = Instantiate(_gun, _weaponArm.transform);
        }
        // ���݉���������̏ꍇ
        else if (_nowWeaponType == WeaponType.kFar)
        {
            _anim.SetBool("IsGun", false);
            _anim.SetBool("IsSword", true);
            _nowWeaponType = WeaponType.kNear;
            _weapon = Instantiate(_sword, _weaponArm.transform);
        }
    }

    /// <summary>
    /// �W�����v��ԑJ��
    /// </summary>
    private void OnJump()
    {
        // �W�����v���͖���
        if (_isJump) return;

        _anim.SetTrigger(kAnimParmInfo[AnimParm.kJump]);
        _anim.SetBool(kAnimParmInfo[AnimParm.kJumpAir], true);
        _isJump = true;
        _jumpVelocity.y = kJumpPower;
    }

    /// <summary>
    /// �A�C�e���̎g�p
    /// </summary>
    private void UseItem()
    {
        // TODO: �A�C�e���g�p
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

        // MEMO: ���]�������̂�Ԃ����ƂŏI������1�t���[������true�ɂ���
        return !_isCheckAnimEnd;
    }

    private void Jump()
    {
        // �W�����v���łȂ��Ȃ疳��
        if (!_isJump) return;

        transform.position += _jumpVelocity;
        _jumpVelocity.y += kFallSpeed;
        // �Œᑬ�x�����ɂȂ�����␳
        if (_jumpVelocity.y < kMaxFallSpeed)
        {
            _jumpVelocity.y = kMaxFallSpeed;
        }
    }

    private void Stamina()
    {
        // ���Ă��Ȃ��Ƃ�
        if (!_isTired)
        {
            // �_�b�V�����X�^�~�i���炷
            if (_isDash)
            {
                _stamina -= kDashCost;
            }
            // �X�^�~�i��0�����ɂȂ�����
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
            // �X�^�~�i���ő�łȂ��ꍇ
            if (_stamina < kMaxStamina)
            {
                // �X�^�~�i��
                _stamina += kRecoveryStaminaSpeed;
                // �ő�l�𒴂�����␳
                _stamina = Mathf.Min(_stamina, kMaxStamina);
            }
        }
        // ���Ă���Ƃ�
        else
        {
            // �ʏ펞��2�{�̑��x�ŃX�^�~�i��
            _stamina += kRecoveryStaminaSpeed * 2;
            // �ő�l�܂ŉ񕜂�����ʏ��Ԃ�
            if (_stamina >= kMaxStamina)
            {
                _stamina = kMaxStamina;
                _isTired = false;
            }
        }
    }

    private void StanTime()
    {
        // �X�^����ԂłȂ��Ȃ疳��
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
        // �U���\��ԂȂ疳��
        if (_isCanAttack) return;

        --_waitAttackTime;

        if (_waitAttackTime < 0)
        {
            _isCanAttack = true;
        }
    }

    private void SafeTime()
    {
        // ���G��ԂłȂ��Ȃ疳��
        if (!_isSafe) return;

        --_safeTime;

        if (_safeTime < 0)
        {
            _isSafe = false;
        }
    }

    /// <summary>
    /// _deltaHp��_hp�܂Ō��炷����
    /// </summary>
    protected void ReduceDeltaHp()
    {
        // �������łȂ��Ȃ�I��
        if (!_isDeltaHp) return;

        // ����
        _deltaHp -= kDecreaseHpSpeed;
        // ���݂�HP�����ɂȂ�����I��
        if (_deltaHp < _hp)
        {
            _deltaHp = _hp;
            _isDeltaHp = false;
        }
    }
}
