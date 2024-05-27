using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class EnemyAir : EnemyBase
{
    public int m_enemyAirPosY;  //��̃G�l�~�[��Y���W�̃|�W�V����
    private int m_enemyRadius;  //����̉~�^���̔��a
    private int m_enemyRadiusSpeed; //���񂷂�Ƃ��̑���
    private bool _enemyAirMove;  //�_��ɂނ����s���t���O
    private bool _circularmotion;  //�~�^��������t���O
    private bool _airAttak;  //�G���U������t���O
    private float _airAttakTime; //�G���U������܂ł̊Ԋu
    private float _airMoveX;  //�G��X���s��
    private float _airMoveY;  //�G��Y���s��
    private float _airMoveZ;  //�G��Z���s��
    float x;  //X��
    float z;  //Z��
    //private Vector3 defPosition;  //Vector3�ňʒu�����擾
    private Vector3 pos;

    /// <summary>
    /// �X�^�[�g�֐�
    /// </summary>
    void Start()
    {
        m_enemyRadius = 3;
        m_enemyRadiusSpeed = 1;
        _airAttakTime = 0.0f;
        _enemyAirMove = false;
        _airAttak = false;
        _circularmotion = false;
        m_enemyAirPosY = 5;

        _airMoveX = 0.0f;
        _airMoveY = 0.0f;
        _airMoveZ = 0.0f;
    }

    /// <summary>
    /// �X�V����
    /// </summary>
    new void Update()
    {
        if (_circularmotion == true)   //_circularmotion��true�Ȃ����s��
        {

            _airAttakTime = Time.time;  //�U���܂ł̊Ԋu��i�߂�

            x = m_enemyRadius * Mathf.Sin(Time.time * m_enemyRadiusSpeed);
            z = m_enemyRadius * Mathf.Cos(Time.time * m_enemyRadiusSpeed);

            transform.position = new Vector3(x + pos.x, pos.y + m_enemyAirPosY, z + pos.z);

            if (_airAttakTime >= 10.0f)
            {
                _airAttak = true;  //�U���t���O���\�ɂ���

                _circularmotion = false;  //����s������߂�

            }
        }
        else if(_airAttak == true) //�U���t���O��true�Ȃ�
        {
            float sb,sbx, sby, sbz; //�G�̈ړ����x��ݒ肷��

            sbx = base.target.transform.position.x - pos.x;
            sby = base.target.transform.position.y - pos.y;
            sbz = base.target.transform.position.z - pos.z;

            sb = Mathf.Sqrt(sbx * sbx + sby * sby + sbz * sbz);

            _airMoveX = sbx / sb * m_enemySpeed;
            _airMoveY = sby / sb * m_enemySpeed;
            _airMoveZ = sbz / sb * m_enemySpeed;

            transform.position = new Vector3(pos.x + _airMoveX, pos.y + _airMoveY, pos.z + _airMoveZ);
            
        }

    }

    /// <summary>
    /// ����������
    /// </summary>
    public override void Init()
    {
        base.Init();
    }

    /// <summary>
    /// ���������̍X�V����
    /// </summary>
    public override void FixedUpdate()
    {
        if (_circularmotion == false && _enemyAirMove == false)    //_circularmotion��false�Ȃ�_��֌������s��(�ŏ�����)
        {
            Transform trans = this.transform;

            pos = transform.position;  //�|�W�V������Vector3�Œ�`����

            pos.y = m_enemyAirPosY;   //Y���W��ݒ�

            transform.position = pos;

            base.FixedUpdate();
        }

    }

    /// <summary>
    /// �_��̓����蔻��
    /// </summary>
    /// <param name="collision"></param>
    public override void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.name == "Farm")
        {
            base.OnCollisionStay(collision);

            _airAttak = false;  //�U����������false�ɖ߂�

            _airAttakTime = _airAttakTime - Time.time;  //�����ɖ߂�
        }
    }

    private new void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.name == "Farm")  //���̂ɓ���������
        {
            if(_circularmotion == false�@&& _airAttak == false) //_circularmotion��false��_airAttack��false�Ȃ�
            {
                _circularmotion = true;  //_circularmotion��true�ɂ���

                _enemyAirMove = true;  //�_��ɓ��B����
            }
        }

        farm.OnTriggerEnter(collision);  //�_��̍��G�͈�
    }

    /// <summary>
    /// �_��̍��G�͈�
    /// </summary>
    /// <param name="collision"></param>
    private new void OnTriggerExit(Collider collision)
    {
        farm.OnTriggerExit(collision);  //�_��̍��G�͈�
    }

}
