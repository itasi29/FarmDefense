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
    private bool _airRetrun;  //�G�����ɖ߂�t���O
    private float _airAttakTime; //�G���U������܂ł̊Ԋu
    float x;  //X��
    float z;  //Z��
    //private Vector3 defPosition;  //Vector3�ňʒu�����擾
    private Vector3 pos;

    /// <summary>
    /// �X�^�[�g�֐�
    /// </summary>
    void Start()
    {
        m_enemyRadius = 4;
        m_enemyRadiusSpeed = 2;
        _airAttakTime = 0.0f;
        _enemyAirMove = false;
        _airAttak = false;
        _circularmotion = false;
        m_enemyAirPosY = 9;
        _airRetrun = false;
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
        Vector3 start = transform.position;
        Vector3 air = new Vector3(target.transform.position.x, m_enemyAirPosY, target.transform.position.z);
        float timer = 0;
        timer += Time.deltaTime;

        if (_circularmotion == false && _enemyAirMove == false)    //_circularmotion��false�Ȃ�_��֌������s��(�ŏ�����)
        {
            Transform trans = this.transform;

            pos = transform.position;  //�|�W�V������Vector3�Œ�`����

            pos.y = m_enemyAirPosY;   //Y���W��ݒ�

            transform.position = pos;

            transform.position = Vector3.Lerp(start, air, timer);  //�_��̏��ɍs��
        }

        if (_circularmotion == true)   //_circularmotion��true�Ȃ����s��
        {

            _airAttakTime += Time.deltaTime;  //�U���܂ł̊Ԋu��i�߂�

            x = m_enemyRadius * Mathf.Sin(_airAttakTime * m_enemyRadiusSpeed);
            z = m_enemyRadius * Mathf.Cos(_airAttakTime * m_enemyRadiusSpeed);

            transform.position = new Vector3(x, pos.y, z);

            if (_airAttakTime >= 15.0f)
            {
                _airAttak = true;  //�U���t���O���\�ɂ���

                _circularmotion = false;  //����s������߂�

                _airAttakTime = 0.0f;  //����������

            }
        }

        if (_airAttak == true) //�U���t���O��true�Ȃ�
        {
            base.FixedUpdate();  //�_��ɂނ����s��
        }

        if (_airRetrun == true)  //_airRetrun��true�Ȃ�
        {
            
            transform.position = Vector3.Lerp(start, air, timer);  //�_��̏��ɍs��

            if (start.y >= 8.8f)  //���̍����ɓ��B������
            {
                _circularmotion = true;  //_circularmotion��true�ɂ���

                _airRetrun = false;  //_airRetrun��false�ɂ���
            }
        }

    }

    /// <summary>
    /// �_��̓����蔻��
    /// </summary>
    /// <param name="collision"></param>
    public override void OnCollisionStay(Collision collision)
    {
        if(_airAttak == true)  //�U���t���O��true�Ȃ�
        {
            base.OnCollisionStay(collision);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Farm")  //Farm�ɐG�ꂽ��
        {
            _airAttak = false;  //�U����������false�ɖ߂�

            _airRetrun = true; //_airRetrun��true�ɂ���
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.name == "Farm")  //���̂ɓ���������
        {
            if(_circularmotion == false�@&& _airAttak == false) //_circularmotion��false��_airAttack��false�Ȃ�
            {
                _circularmotion = true;  //_circularmotion��true�ɂ���

                _enemyAirMove = true;  //�_��ɓ��B����
            }
        }
    }

}
