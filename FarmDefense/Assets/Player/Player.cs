using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;

//�v���C���[�̏����܂Ƃ߂��
public class Player : MonoBehaviour
{
    private const float kDownSpeed = 0.1f;//�X�^�~�i�؂ꎞ�̈ړ����x

    private const float kSpeed = 0.2f;//��{�I�Ȉړ����x

    private const float kDashMaxSpeed = 0.5f;//�_�b�V�����̈ړ����x

    private const float kDashMaxSpeedTime = 0.05f;//�ō����x�ɒB����܂ł̎���

    private const float kDashAddSpeed = (kDashMaxSpeed - kSpeed) * kDashMaxSpeedTime;

    private const float kJumpPower = 30.0f;//�W�����v��

    private const int kStaminaMax = 500;//�X�^�~�i�̍ő�l

    private const int kHpMax = 100;//�̗͂̍ő�l

    private const int kHitStanTime = 30;//�G�l�~�[�ɂԂ������Ƃ��̃X�^������

    private const int kHitSafeTime = 40;//�G�l�~�[�ɂԂ������Ƃ��̖��G����

    private const int kReviveSafeTime = 60;//�G�l�~�[�ɂԂ������Ƃ��̖��G����

    private const int kHeavyAttackNeedStamina = 100;//���U�����ɕK�v�ɂȂ�X�^�~�i

    private Vector3 kInitPos = new Vector3(0, 0, 0);

    private Vector3 _dirVec;
    private Vector3 _moveVec;
    private Rigidbody _rigidBody;

    private float _speed;

    private int _stamina;

    private bool _isDash;

    private bool _isJump;

    private int _hp;

    private int _hitStanTime;

    private bool _isStan;

    private bool _isSafe;

    private int _safeTime;

    private bool _isUseNearWeapon;

    private bool _isTired;

    private NearWeapon _nearWeapon;
    private FarWeapon _farWeapon;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _stamina = kStaminaMax;
        _hp = kHpMax;
        _speed = kSpeed;

        _isJump = false;

        _isStan = false;

        _isTired = false;

        _isDash = false;

        _nearWeapon = null;

        _farWeapon = null;

        _isSafe = false;

        _isUseNearWeapon = true;

        //TODO:�ق��̂��������̏����ɕς�����.�擾�͂ł��Ă�
        GameObject weapon = GameObject.Find("scop");

        _nearWeapon = weapon.GetComponent<NearWeapon>();

        weapon = null;

        weapon = GameObject.Find("gun");

        _farWeapon = weapon.GetComponent<FarWeapon>();


    }

    void FixedUpdate()
    {
 //       Debug.Log(_stamina);
        //�X�^�����Ă��Ȃ��Ƃ��̏���
        if (!_isStan)
        {
            this.transform.position += _moveVec;

            //�_�b�V�����̏���
            if (_isDash)
            {
                _stamina--;
                //�_�b�V���̑��x���ō����x����Ȃ�������
                if (_speed < kDashMaxSpeed)
                {
                    //�ړ����x�����X�ɏグ�Ă���
                    _speed += kDashAddSpeed;
                }
            }
            //�X�^�~�i���ő�l�������Ȃ�������
            if (_stamina < kStaminaMax && !_isDash)
            {
                //�X�^�~�i���񕜂���
                _stamina++;
            }
            //�X�^�~�i���ő�l�ɂȂ�����
            if (_stamina >= kStaminaMax)
            {
                _stamina = kStaminaMax;
                _isTired = false;
            }
            //�X�^�~�i��0�ȉ��ɂȂ����Ƃ��̏���
            else if (_stamina < 0)
            {
                _stamina = 0;
                _isDash = false;
                _isTired = true;
            }

        }
        else
        {
            //�d�����Ԃ����炷
            _hitStanTime--;

            if (_hitStanTime <= 0)
            {
                _isStan = false;
            }
        }
        //���G���Ԃ�����Ƃ�
        if (_safeTime >= 0)
        {
            Debug.Log("�ނĂ�����");
            _isSafe = true;
            _safeTime--;
        }
        else
        {
            _isSafe = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

        //�U���{�^�����������Ƃ�
        if (Input.GetButtonDown("X"))
        {
            Debug.Log("������");
            //�ߋ�������������Ă��邩�ǂ���
            if (_isUseNearWeapon)
            {
                _nearWeapon.Attack();
            }
            else
            {
                _farWeapon.Attack(_dirVec);
            }
        }
        //���U�����g�����Ƃ�(�{�^���킩��Ȃ�)
        if (Input.GetButtonDown("Y") && !_isTired)
        {
            _stamina -= kHeavyAttackNeedStamina;
        }
        //����؂�ւ�
        if (Input.GetButtonDown("RB"))
        {
            _isUseNearWeapon = !_isUseNearWeapon;
        }
        //�ړ�����(��)
        Move();
        if (!_isStan)
        {

            //�W�����v�{�^�����������Ƃ�
            if (Input.GetButtonDown("A") && !_isJump)
            {
                Jump();
            }

            //�A�C�e���g�p����(��)
            if (Input.GetButtonDown("B"))
            {
                RecoveryHp(10);
                Debug.Log(_hp);
            }
        }
        //�_���[�W��H������Ƃ�
        //if���̓f�o�b�O�p
        if (Input.GetButtonDown("StickPushRight") && !_isSafe)
        {
            OnDamage(10);
            Debug.Log(_hp);
        }
    }
    private void Move()
    {


        _moveVec = new Vector3(0, 0, 0);
        Vector3 dirVec = new Vector3(0, 0, 0);


        dirVec.x = Input.GetAxis("Horizontal");
        dirVec.z = Input.GetAxis("Vertical");

        dirVec.Normalize();

        if(dirVec.sqrMagnitude != 0)
        {
            _dirVec = dirVec;
        }

        //�_�b�V���{�^���������Ă��Ĕ���Ԃ���Ȃ�������
        if (Input.GetButton("X") && !_isTired)
        {
            //�X�^�~�i�������Ĉړ����͂�����Ă�����
            if (_stamina > 0 && dirVec != Vector3.zero)
            {
                //�_�b�V���t���O�𗧂Ă�
                _isDash = true;
            }
        }
        //�����Ă��Ȃ��ꍇ
        else
        {
            //�_�b�V���t���O���Ȃ���
            _isDash = false;
            //�ʏ�̃X�s�[�h
            _speed = kSpeed;
        }
        //����Ԃ������ꍇ
        if (_isTired)
        {
            _speed = kDownSpeed;
        }
        _moveVec = dirVec * _speed;
    }

    private void Jump()
    {
        _rigidBody.velocity = new Vector3(0, 0, 0);
        _rigidBody.AddForce(new Vector3(0, kJumpPower, 0), ForceMode.Impulse);
        _isJump = true;
    }

    public void OnDamage(int damage)
    {
        //�̗͂����炷
        _hp -= damage;
        if (_hp < 0)
        {
            _hp = kHpMax;
            //TODO ���S�������̏������쐬����
            this.transform.position = kInitPos;
            _safeTime = kReviveSafeTime;
        }

        //�q�b�g���̍d������
        _isStan = true;
        _hitStanTime = kHitStanTime;

        _safeTime = kHitSafeTime;
    }

    public void RecoveryHp(int heal)
    {
        _hp += heal;
        if (_hp > kHpMax)
        {
            _hp = kHpMax;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Debug.Log("��������");
            _isJump = false;
        }
    }
}

