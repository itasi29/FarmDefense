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

    private Vector3 _center;  //���S�_

    private Vector3 _axis = Vector3.up;   //��]��

    private float _period = 5; //�~�^������

    private bool _updateRotation = true;  //�������X�V

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
        if(m_player == true)   //m_player��true�Ȃ�v���C���[�֌�����
        {
            Transform transform = this.transform;  //�I�u�W�F�N�g���擾

            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, m_enemySpeed * Time.deltaTime);  //�v���C���[�Ɍ�����
        }
        else if(m_player == false)   //m_player��false�Ȃ�
        {
            if (_circularmotion == false && _enemyAirMove == false)    //_circularmotion��false�A_enemyAirMove��false�Ȃ�_��֌������s��(�ŏ�����)
            {
                pos = transform.position;

                Vector3 farm = new Vector3(target.transform.position.x, m_enemyAirPosY, target.transform.position.z);

                float timer = 0;

                timer += Time.deltaTime;

                transform.position = Vector3.Lerp(pos, farm, timer);

            }
            if (_airReturn == true)  //�󒆂ɖ߂�t���O��true�Ȃ�
            {
                pos = transform.position;

                Vector3 farm = new Vector3(target.transform.position.x - 2.0f, m_enemyAirPosY, target.transform.position.z);

                float timer = 0;

                timer += Time.deltaTime;

                transform.position = Vector3.Lerp(pos, farm, timer);

                if (pos.y >= 4.9f)
                {
                    _circularmotion = true;  //����s����������

                    _airReturn = false; //�󒆂ɖ߂��Ă���
                }
            }
            else if (_circularmotion == true)   //_circularmotion��true�Ȃ����s��
            {

                _airAttakTime += Time.deltaTime;  //�U���܂ł̊Ԋu��i�߂�


                _center = target.transform.position;  //���S�_���^�[�Q�b�g�̍��W�ɂ���

                var tr = transform;

                var angleAxis = Quaternion.AngleAxis(360 / _period * Time.deltaTime, _axis);  //��]�̃N�H�[�^�j�I���쐬

                var circlepos = tr.position;  //�~�^���̈ʒu�v�Z

                circlepos -= _center;
                circlepos = angleAxis * circlepos;
                circlepos += _center;

                tr.position = circlepos;


                if (_airAttakTime >= 10.0f)
                {
                    _airAttak = true;  //�U���t���O���\�ɂ���

                    _circularmotion = false;  //����s������߂�

                }
            }
            else if (_airAttak == true) //�U���t���O��true�Ȃ�
            {
                base.FixedUpdate();   //�_��ɍU������
            }
        }

        
        
    }

    /// <summary>
    /// �_��̓����蔻��
    /// </summary>
    /// <param name="collision"></param>
    public override void OnCollisionStay(Collision collision)
    {
        base.OnCollisionStay(collision);

        if (collision.gameObject.name == "Farm")
        {
            _airAttak = false;  //�U����������false�ɖ߂�

            _airReturn = true;  //�󒆂ɖ߂�

            _airAttakTime = 0;  //�����ɖ߂�
        }
        else if(collision.gameObject.name == "Player")  //Player�ɓ���������
        {
            _airReturn = true;  //�󒆂ɖ߂�
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
