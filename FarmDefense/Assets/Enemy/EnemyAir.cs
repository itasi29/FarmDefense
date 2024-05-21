using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class EnemyAir : EnemyBase
{
    public int m_enemyAirPosY;  //��̃G�l�~�[��Y���W�̃|�W�V����
    private float m_enemyRadius;  //����̉~�^���̔��a
    private float m_enemyRadiusSpeed; //���񂷂�Ƃ��̑���
    private float m_enemyRadiusTime;  //���񂷂鎞��
    private bool _circularmotion;  //�~�^��������t���O
    float x;  //X��
    float z;  //Z��
    //private Vector3 defPosition;  //Vector3�ňʒu�����擾
    Transform trans;
    private Vector3 pos;

    /// <summary>
    /// �X�^�[�g�֐�
    /// </summary>
    private void Start()
    {
        m_enemyRadius = 1.2f;
        m_enemyRadiusSpeed = 1.0f;
        m_enemyRadiusTime = 0.0f;

        _circularmotion = false;
    }

    /// <summary>
    /// �X�V����
    /// </summary>
    private new void Update()
    {
        

        if (_circularmotion == true)   //_circularmotion��true�Ȃ����s��
        {
            m_enemyRadiusTime = Time.time;

            x = m_enemyRadius * Mathf.Sin(m_enemyRadiusTime * m_enemyRadiusSpeed);
            z = m_enemyRadius * Mathf.Cos(m_enemyRadiusTime * m_enemyRadiusSpeed);

            m_enemyAirPosY = 5;

            transform.position = new Vector3(x + pos.x, pos.y + m_enemyAirPosY, z + pos.z);
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
        trans = this.transform;

        pos = transform.position;  //�|�W�V������Vector3�Œ�`����

        pos.y = m_enemyAirPosY;   //Y���W��ݒ�

        transform.position = pos;

        if (_circularmotion == false)    //_circularmotion��false�Ȃ�_��֌������s��
        {
            base.FixedUpdate();   
        }
        
    }

    /// <summary>
    /// �_��̓����蔻��
    /// </summary>
    /// <param name="collision"></param>
    public override void OnCollisionStay(Collision collision)
    {
        base.OnCollisionStay(collision);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.name == "Farm")  //���̂ɓ���������
        {
            if(_circularmotion == false) //_circularmotion��false�Ȃ�
            {

                m_enemyRadiusTime = m_enemyRadiusTime - Time.time;

                _circularmotion = true;  //_circularmotion��true�ɂ���
            }
        }
    }

}
