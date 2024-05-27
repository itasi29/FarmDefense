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

    public bool m_player;  //�v���C���[�𔭌��������ǂ����̃t���O

    [SerializeField] protected GameObject target; //�^�[�Q�b�g�̃I�u�W�F�N�g�l��

    [SerializeField] protected GameObject player; //Player�̃I�u�W�F�N�g�l��

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
        m_player = false;
    }

    /// <summary>
    /// ���������̍X�V����
    /// </summary>
    public virtual void FixedUpdate()
    {
        Transform transform = this.transform; //�I�u�W�F�N�g���擾

        if(m_player == false)  //Player���������ĂȂ�������_��Ɍ�����
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, m_enemySpeed * Time.deltaTime);  //�^�[�Q�b�g�̃I�u�W�F�N�g�Ɍ�����
        }
        else if(m_player == true)  //Player�������ł�����Player�Ɍ�����
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, m_enemySpeed * Time.deltaTime);  //Player�̃I�u�W�F�N�g�Ɍ�����
        }
    }

    /// <summary>
    /// �_��Ƃ̓����蔻��
    /// </summary>
    public virtual void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.name == "Farm") //Farm�ɓ���������U��
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
        else if(collision.gameObject.name == "Player")  //Plyaer�ɓ���������U��
        {
            if (m_attackinterval == false)    //�t���O��false�Ȃ�U���J�n
            {
                farm.m_farmHp -= m_enemyAttack; //�������Ă�Ƃ�HP�����炷

                Debug.Log(farm.m_farmHp -= (int)m_enemyAttack);

                m_attackinterval = true;  //1�񂾂��U���\
            }
            else if (m_attackinterval == true)  //�t���O��true�Ȃ�U�����~
            {
                m_attackTime++;  //���Ԍo��

                if (m_attackTime >= 60.0f)  //���Ԃ����Ă�
                {
                    m_attackTime = 0;   //0�b�ɖ߂�

                    m_attackinterval = false; //�t���O��false�ɖ߂�
                }
            }
        }
    }

    /// <summary>
    /// �v���C���[���U��������
    /// </summary>
    /// <param name="collision"></param>
    public virtual void OnCollisionEnter(Collider collision)
    {
        //�v���C���[�ɍU�����ꂽ��_���[�W���󂯂�
    }

    /// <summary>
    /// �v���C���[�����G�͈͂ɓ��������ǂ���
    /// </summary>
    /// <param name="collision"></param>
    public virtual void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.name == "Player")  //Player�����G�͈͂ɓ�������Player��ǂ�������
        {
            m_player = true;  //m_player��true�ɂ���

            Debug.Log("������");
        }
    }

    /// <summary>
    /// �v���C���[�����G�͈͂��ł����ǂ���
    /// </summary>
    /// <param name="collision"></param>
    public virtual void OnTriggerExit(Collider collision)
    {
        if(collision.gameObject.name == "Player")
        {
            m_player = false; //m_player��false�ɂ���

            Debug.Log("������");

        }
    }
}
