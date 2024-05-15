using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G�l�~�[�̐e
/// </summary>
public class EnemyBase : MonoBehaviour
{  
    public int m_enemyHp;  //�G��HP

    public float m_enemySpeed;  //�G�̃X�s�[�h

    public float m_enemyAttack;  //�G�̍U����



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
    }

    /// <summary>
    /// ���������̍X�V����
    /// </summary>
    public virtual void FixedUpdate()
    {
        
    }
}
