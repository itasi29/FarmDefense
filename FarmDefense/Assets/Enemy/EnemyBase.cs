using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// エネミーの親
/// </summary>
public class EnemyBase : MonoBehaviour
{  
    public int m_enemyHp;  //敵のHP

    public float m_enemySpeed;  //敵のスピード

    public int m_enemyAttack;  //敵の攻撃力

    public float m_attackTime;  //敵の攻撃時間間隔

    public bool m_attackinterval;  //攻撃したかのフラグ

    [SerializeField] protected GameObject target; //ターゲットのオブジェクト獲得

    public FarmBase farm;  //農場のスクリプト呼び出し

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
        m_attackTime = 0;

        m_attackinterval = false;
    }

    /// <summary>
    /// 物理挙動の更新処理
    /// </summary>
    public virtual void FixedUpdate()
    {
        Transform transform = this.transform; //オブジェクトを取得

        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, m_enemySpeed * Time.deltaTime);  //ターゲットのオブジェクトに向かう
    }

    /// <summary>
    /// 農場との当たり判定
    /// </summary>
    public virtual void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.name == "Farm")
        {
            if(m_attackinterval == false)    //フラグがfalseなら攻撃開始
            {
                farm.m_farmHp -= m_enemyAttack; //当たってるときHPを減らす

                Debug.Log(farm.m_farmHp -= (int)m_enemyAttack);

                m_attackinterval = true;  //1回だけ攻撃可能
            }
            else if(m_attackinterval == true)  //フラグがtrueなら攻撃中止
            {
                m_attackTime++;  //時間経過

                if(m_attackTime >= 60.0f)  //時間がたてば
                {
                    m_attackTime = 0;   //0秒に戻す

                    m_attackinterval = false; //フラグをfalseに戻す
                }
            }
            

        }
    }
}
