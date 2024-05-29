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
    private bool _airReturn;  //�G���󒆂ɖ߂�t���O
    private float _airAttakTime; //�G���U������܂ł̊Ԋu
    float x;  //X��
    float z;  //Z��
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

        _airReturn = false;
    }

    /// <summary>
    /// �X�V����
    /// </summary>
    new void Update()
    {
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
        if (_circularmotion == false && _enemyAirMove == false && m_player == false)    //_circularmotion��false�A_enemyAirMove��false�Am_player��false�Ȃ�_��֌������s��(�ŏ�����)
        {
            pos = transform.position;

            Vector3 farm = new Vector3(target.transform.position.x, m_enemyAirPosY, target.transform.position.z);

            pos.y = m_enemyAirPosY;   //Y���W��ݒ�

            float timer = 0;

            timer += Time.deltaTime;

            transform.position = Vector3.Lerp(pos, farm, timer);
         
        }

        if (_circularmotion == true && m_player == false)   //_circularmotion��true�Am_player��false�Ȃ����s��
        {

            _airAttakTime += Time.deltaTime;  //�U���܂ł̊Ԋu��i�߂�

            x = m_enemyRadius * Mathf.Sin(_airAttakTime * m_enemyRadiusSpeed);
            z = m_enemyRadius * Mathf.Cos(_airAttakTime * m_enemyRadiusSpeed);

            transform.position = new Vector3(x + pos.x, pos.y, z + pos.z);

            if (_airAttakTime >= 10.0f)
            {
                _airAttak = true;  //�U���t���O���\�ɂ���

                _circularmotion = false;  //����s������߂�

            }
        }
        else if (_airAttak == true && m_player == false) //�U���t���O��true�Am_player��false�Ȃ�
        {
            base.FixedUpdate();   //�_��ɍU������
        }
        else if(_airReturn == true && m_player == false)  //�󒆂ɖ߂�t���O��true�Am_player��false�Ȃ�
        {
            pos = transform.position;

            Vector3 farm = new Vector3(target.transform.position.x, m_enemyAirPosY, target.transform.position.z);

            float timer = 0;

            timer += Time.deltaTime;

            transform.position = Vector3.Lerp(pos, farm, timer);

            if(pos.y >= 4.9f)
            {
                _circularmotion = true;  //����s����������
            }
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

            _airReturn = true;  //�󒆂ɖ߂�

            _airAttakTime = 0;  //�����ɖ߂�
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
