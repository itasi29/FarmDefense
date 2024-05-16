using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAir : EnemyBase
{
    public int m_enemyAirPosY;  //��̃G�l�~�[��Y���W�̃|�W�V����
    private int m_enemyRadius;  //����̉~�^���̔��a
    private float _x;  //�G�l�~�[��X���W�ړ��ϐ�
    private float _z;  //�G�l�~�[��Z���W�ړ��ϐ�

    /// <summary>
    /// �X�V����
    /// </summary>
    public override void Update()
    {
        base.Update();
    }

    /// <summary>
    /// ����������
    /// </summary>
    public override void Init()
    {
        base.Init();

        m_enemyAirPosY = 0;
        m_enemyRadius = 5;
        _x = 0.0f;
        _z = 0.0f;
    }

    /// <summary>
    /// ���������̍X�V����
    /// </summary>
    public override void FixedUpdate()
    {
        base.FixedUpdate();

        Vector3 pos = transform.position;  //Transform�̏�񂩂�Vector3�̈ʒu�����擾
       
        pos.y = m_enemyAirPosY;  //Y���W��ݒ�

        transform.position = pos;  //�����̍��W�ɒl��ݒ�

        if(pos.x == farm.posFarm.x && pos.z == farm.posFarm.z)  //�_��Ɠ���X���W��Z���W�ɂȂ�����
        {
            
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

}
