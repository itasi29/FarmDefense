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

    /* �ϐ� */
    // �v���C���[���
    private Animator _anim;
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
    private string _stopAnimName;
    private bool _isNowAnimCheckName;
    private bool _isCheckAnimEnd;
    private bool _isStopMove;
    private bool _isDash;
    private bool _isTired;
    private bool _isJump;
    private bool _isStan;
    private bool _isSafe;
    private bool _isDeltaHp;
    private Vector3 _jumpVelocity;
    private Vector3 _velocity;
    // ������
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
        // �e��擾
        _anim = GetComponent<Animator>();
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
        if (Input.GetButtonDown("X"))
        {
            OnAttack();
        }
        // ���U��
        else if (Input.GetButtonDown("Y"))
        {
            OnStrongAttack();
        }
    }

    private void FixedUpdate()
    {
        // �X�g�b�v�I���m�F����
        if (CheckStopAnimEnd())
        {
            _isStopMove = false;
        }

        // �W�����v����
        Jump();
        // �X�^����
        if (_isStan)
        {
            // �X�^�~�i����
            Stamina();
        }
        // ��X�^����
        else
        {
            // �X�^������
            StanTime();
        }
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
                _velocity = velocity * _status.speed;
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
            velocity *= _status.dashSpeed;
        }
        // ���Ă���Ƃ�
        else if (_isTired)
        {
            velocity *= _status.downSpeed;
        }
        // �ʏ펞
        else
        {
            velocity *= _status.speed;
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
        }
        else if (_nowWeaponType == WeaponType.kFar)
        {
            // �e�̐���
            var bullet = Instantiate(_bullet, _weapon.transform.position, Quaternion.identity);

            Vector3 velocity = transform.forward;
            velocity.y = _camera.GetFront().y;
            velocity = velocity.normalized * _bulletStatus.speed;

            bullet.GetComponent<Bullet>().Init(_bulletStatus.attack, velocity);
        }

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
        int temp = _stamina - _cost.strongAttack;
        // ���s�ł��邩
        bool isDo = temp >= 0;
        // ���s
        if (isDo)
        {
            StopMove(kAnimParmInfo[AnimParm.kStAttack]);
            _anim.SetTrigger(kAnimParmInfo[AnimParm.kStAttack]);
            _stamina = temp;
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
        _jumpVelocity.y = _status.jumpPower;
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


        //Debug.Log(_anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
        //if (_anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        //{
        //    _isCheckAnimEnd = false;
        //}
        Debug.Log(_anim.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        if (_anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == _stopAnimName)
        {
            _isNowAnimCheckName = true;
        }
        else if (_isNowAnimCheckName)
        {
            _isCheckAnimEnd = false;
        }

#if false
        if (_anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != _stopAnimName)
        {
            _isNowAnimCheckName = true;
        }
        else if (_isNowAnimCheckName)
        {
            _isCheckAnimEnd = false;
        }
#endif

        // MEMO: ���]�������̂�Ԃ����ƂŏI������1�t���[������true�ɂ���
        return !_isCheckAnimEnd;
    }

    private void Jump()
    {
        // �W�����v���łȂ��Ȃ疳��
        if (!_isJump) return;

        transform.position += _jumpVelocity;
        _jumpVelocity.y += _status.fallPower;
    }

    private void Stamina()
    {
        // ���Ă��Ȃ��Ƃ�
        if (!_isTired)
        {
            // �_�b�V�����X�^�~�i���炷
            if (_isDash)
            {
                _stamina -= _cost.dash;
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
            if (_stamina < _status.maxStamina)
            {
                // �X�^�~�i��
                _stamina += _status.recoveryStamina;
                // �ő�l�𒴂�����␳
                _stamina = Mathf.Min(_stamina, _status.maxStamina);
            }
        }
        // ���Ă���Ƃ�
        else
        {
            // �ʏ펞��2�{�̑��x�ŃX�^�~�i��
            _stamina += _status.recoveryStamina * 2;
            // �ő�l�܂ŉ񕜂�����ʏ��Ԃ�
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
