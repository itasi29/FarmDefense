using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class EnemyAir : EnemyBase
{
    public int m_enemyAirPosY;  //��̃G�l�~�[��Y���W�̃|�W�V����
    private int m_enemyRadius;  //����̉~�^���̔��a
    private bool _circularmotion;  //�~�^��������t���O
    float x;  //X��
    float z;  //Z��
    private Vector3 defPosition;  //Vector3�ňʒu�����擾

    /// <summary>
    /// �X�^�[�g�֐�
    /// </summary>
    private void Start()
    {
        m_enemyRadius = 2;

        defPosition = transform.position;
    }

    /// <summary>
    /// �X�V����
    /// </summary>
    private new void Update()
    {
        x = m_enemyRadius * Mathf.Sin(Time.time * m_enemySpeed);
        z = m_enemyRadius * Mathf.Cos(Time.time * m_enemySpeed);

        transform.position = new Vector3(x + defPosition.x, defPosition.y, z + defPosition.z);
    }

    /// <summary>
    /// �X�V����
    /// </summary>
    //public override void Update()
    //{
    //   //if(_circularmotion == true)
    //   // {
    //   //     x = m_enemyRadius * Mathf.Sin(Time.time * m_enemySpeed);
    //   //     z = m_enemyRadius * Mathf.Cos(Time.time * m_enemySpeed);

    //   //     transform.position = new Vector3(x + defPosition.x, defPosition.y, z + defPosition.z);
    //   // }
    //}

    /// <summary>
    /// ����������
    /// </summary>
    public override void Init()
    {
        base.Init();

        m_enemyAirPosY = 5;
        //m_enemyRadius = 3;
        //_circularmotion = true;

        //defPosition = transform.position;
    }

    /// <summary>
    /// ���������̍X�V����
    /// </summary>
    public override void FixedUpdate()
    {
        //Transform transform = this.transform;

        //x = m_enemyRadius * Mathf.Sin(Time.time * m_enemySpeed);
        //z = m_enemyRadius * Mathf.Cos(Time.time * m_enemySpeed);

        //transform.position = new Vector3(x + defPosition.x, defPosition.y, z + defPosition.z);
        //Vector3 pos = transform.position;  //�|�W�V������Vector3�Œ�`����

        //pos.y = m_enemyAirPosY;  //Y���W��ݒ�

        //transform.position = pos;

        
        //if(_circularmotion == false)  //�~�^������t���O��false�Ȃ�
        //{
        //    base.FixedUpdate();
        //}
        
    }

    /// <summary>
    /// �_��̓����蔻��
    /// </summary>
    /// <param name="collision"></param>
    public override void OnCollisionStay(Collision collision)
    {
        base.OnCollisionStay(collision);
    }

}
