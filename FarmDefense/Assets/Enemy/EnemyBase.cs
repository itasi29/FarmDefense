using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エネミーの親
/// </summary>
public class EnemyBase : MonoBehaviour
{  
    public int m_enemyHp;  //敵のHP

    public float m_enemySpeed;  //敵のスピード

    public float m_enemyAttack;  //敵の攻撃力



    /// <summary>
    /// 更新処理
    /// </summary>
    public virtual void Update()
    {
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    public virtual void Init()
    {
        m_enemyHp = 0;
        m_enemySpeed = 0;
        m_enemyAttack = 0;
    }

    /// <summary>
    /// 物理挙動の更新処理
    /// </summary>
    public virtual void FixedUpdate()
    {
        
    }
}
