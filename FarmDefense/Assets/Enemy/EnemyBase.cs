using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// �G�l�~�[�̐e
/// </summary>
public class EnemyBase : MonoBehaviour
{  
    public int m_enemyHp;  //�G��HP

    public float m_enemySpeed;  //�G�̃X�s�[�h

    public int m_enemyAttack;  //�G�̍U����

    public float m_attackTime;  //�G�̍U�����ԊԊu

    public bool m_attackinterval;  //�U���������̃t���O

    [SerializeField] protected GameObject target; //�^�[�Q�b�g�̃I�u�W�F�N�g�l��

    public FarmBase farm;  //�_��̃X�N���v�g�Ăяo��

    /// <summary>
    /// �X�V����
    /// </summary>
    public virtual void Update()
    {
    }

    /// <summary>
    /// ����������
    /// </summary>
    public virtual void Init()
    {
        m_enemyHp = 0;
        m_enemySpeed = 0;
        m_enemyAttack = 0;
        m_attackTime = 0;

        m_attackinterval = false;
    }

    /// <summary>
    /// ���������̍X�V����
    /// </summary>
    public virtual void FixedUpdate()
    {
        Transform transform = this.transform; //�I�u�W�F�N�g���擾

        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, m_enemySpeed * Time.deltaTime);  //�^�[�Q�b�g�̃I�u�W�F�N�g�Ɍ�����
    }

    /// <summary>
    /// �_��Ƃ̓����蔻��
    /// </summary>
    public virtual void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.name == "Farm")
        {
            if(m_attackinterval == false)    //�t���O��false�Ȃ�U���J�n
            {
                farm.m_farmHp -= m_enemyAttack; //�������Ă�Ƃ�HP�����炷

                Debug.Log(farm.m_farmHp -= (int)m_enemyAttack);

                m_attackinterval = true;  //1�񂾂��U���\
            }
            else if(m_attackinterval == true)  //�t���O��true�Ȃ�U�����~
            {
                m_attackTime++;  //���Ԍo��

                if(m_attackTime >= 60.0f)  //���Ԃ����Ă�
                {
                    m_attackTime = 0;   //0�b�ɖ߂�

                    m_attackinterval = false; //�t���O��false�ɖ߂�
                }
            }
            

        }
    }
}
