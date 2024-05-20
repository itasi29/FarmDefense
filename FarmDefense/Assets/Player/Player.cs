using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//�v���C���[�̏����܂Ƃ߂��
public class Player : MonoBehaviour
{
    private const float kDownSpeed = 0.1f;//�X�^�~�i�؂ꎞ�̈ړ����x

    private const float kSpeed = 0.2f;//��{�I�Ȉړ����x

    private const float kDashMaxSpeed = 0.5f;//�_�b�V�����̈ړ����x

    private const float kDashMaxSpeedTime = 0.05f;//�ō����x�ɒB����܂ł̎���

    private const float kDashAddSpeed = (kDashMaxSpeed - kSpeed) * kDashMaxSpeedTime;

    private const float kJumpPower = 30.0f;//�W�����v��

    private const int kStaminaMax = 1200;//�X�^�~�i�̍ő�l

    private Vector3 _moveVec;
    private Rigidbody _rigidBody;

    private float _speed;

    private int _stamina;

    private bool _isDash;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _stamina = kStaminaMax;
        _speed = kSpeed;
    }

    void FixedUpdate()
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
        else if (_stamina < kStaminaMax)
        {
            //�X�^�~�i���񕜂���
            _stamina++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        //�W�����v�{�^�����������Ƃ�
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }
    private void Move()
    {

        _moveVec = new Vector3(0, 0, 0);
        Vector3 dirVec = new Vector3(0, 0, 0);


        dirVec.x = Input.GetAxis("Horizontal");
        dirVec.z = Input.GetAxis("Vertical");

        dirVec.Normalize();

        //�_�b�V���{�^���������Ă�����
        if (Input.GetButton("Fire1"))
        {
            //�X�^�~�i�������Ĉړ����͂�����Ă�����
            if (_stamina > 0 && dirVec != Vector3.zero)
            {
                //�_�b�V���t���O�𗧂Ă�
                _isDash = true;
            }
            //�X�^�~�i���Ȃ���Ԃ�������
            else
            {
                //�ʏ��菭���x���X�s�[�h
                _speed = kDownSpeed;
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
        _moveVec = dirVec * _speed;
        Debug.Log(_stamina);
    }

    private void Jump()
    {
        _rigidBody.velocity = new Vector3(0, 0, 0);
        _rigidBody.AddForce(new Vector3(0, kJumpPower, 0), ForceMode.Impulse);
    }

}

