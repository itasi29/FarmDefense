using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAir : EnemyBase
{
    public int m_enemyAirPosY;  //��̃G�l�~�[��Y���W�̃|�W�V����

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
    }

    /// <summary>
    /// ���������̍X�V����
    /// </summary>
    public override void FixedUpdate()
    {
        base.FixedUpdate();

        Transform transform = this.transform; //�I�u�W�F�N�g���擾

        Vector3 pos = transform.position;  //Transform�̏�񂩂�Vector3�̈ʒu�����擾

        pos.x += m_enemySpeed;  //X���W�ɃX�s�[�h�����Z����
        pos.z += m_enemySpeed;  //Z���W�ɃX�s�[�h�����Z����
        pos.y = m_enemyAirPosY;  //Y���W��ݒ�

        transform.position = pos;  //�����̍��W�ɒl��ݒ�
    }

}
