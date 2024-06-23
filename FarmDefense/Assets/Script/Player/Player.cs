using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /* �萔 */
    private const int kDecreaseHpSpeed = 1;   // _deltaHp�̌������x
    private Vector3 kInitPos = Vector3.zero;

    /* �ϐ� */
    private Rigidbody _rb;
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
    private CameraControl _camera;
    
    void Start()
    {
        // �e��擾
        _rb = GetComponent<Rigidbody>();
        _camera = GameObject.Find("Main Camera").GetComponent<CameraControl>();
        var dataMgr = GameObject.Find("DataManager").GetComponent<DataManager>();
        var data = dataMgr.Player;
        _status = data.Status;
        _cost = data.Cost;
        _time = data.Time;

        // ������
        Init();
    }

    private void Update()
    {
        // �X�^�����͍s���s��
        if (_isStan) return;

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
        // ����؂�ւ�
        else if (Input.GetButtonDown("RB"))
        {
            ChangeWeapon();
        }
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
        // �ړ�
        Move();
    }

    private void FixedUpdate()
    {
        _rb.velocity = _velocity;
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
    }

    private void OnCollisionEnter(Collision collision)
    {
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
    /// �ʏ�U��
    /// </summary>
    private void OnAttack()
    {
        // TODO: �ʏ�U��
    }

    /// <summary>
    /// ���U��
    /// </summary>
    private void OnStrongAttack()
    {
        // �����̃X�^�~�i�m�F
        int temp = _stamina - _cost.strongAttack;
        // ���s�ł��邩
        bool isDo = temp >= 0;
        // ���s
        if (isDo)
        {
            _stamina = temp;
            // TODO: ���U��
        }
        else
        {
            // TODO: �]�T������΁u����Ȃ��v�Ƃ����e�L�X�g���o���悤��
        }
    }

    /// <summary>
    /// ����̕ύX
    /// </summary>
    private void ChangeWeapon()
    {
        // TODO: ����؂�ւ�
    }

    /// <summary>
    /// �W�����v��ԑJ��
    /// </summary>
    private void OnJump()
    {
        // �W�����v���͖���
        if (_isJump) return;

        _isJump = true;
    }

    /// <summary>
    /// �A�C�e���̎g�p
    /// </summary>
    private void UseItem()
    {
        // TODO: �A�C�e���g�p
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
            return;
        }
        
        // �_�b�V��
        _isDash = false;
        if (Input.GetAxis("RT") > 0.0f)
        {
            OnDash();
        }

        velocity.Normalize();
        // �����ϊ�
        transform.rotation = Quaternion.LookRotation(velocity);

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
    /// �_�b�V��
    /// </summary>
    private void OnDash()
    {
        // ���Ă��Ȃ���΃_�b�V����Ԃ�
        if (!_isTired)
        {
            _isDash = true;
        }
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
                _isDash = false;
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
