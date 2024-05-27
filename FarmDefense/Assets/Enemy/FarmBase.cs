using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmBase : MonoBehaviour
{
    public int m_farmHp = 100;  //農場の体力

    public Vector3 posFarm;

    public EnemyBase enemy;  //enemyのクラス呼び出し

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
        posFarm = this.transform.position;
    }

    /// <summary>
    /// 物理挙動の更新処理
    /// </summary>
    public virtual void FixedUpdate()
    {

    }

    /// <summary>
    /// EnemyAirの索敵範囲実装
    /// </summary>
    /// <param name="collision"></param>
    public void OnTriggerEnter(Collider collision)
    {
        enemy.OnTriggerEnter(collision);
    }

    /// <summary>
    /// EnemyAirの索敵範囲実装
    /// </summary>
    /// <param name="collision"></param>
    public void OnTriggerExit(Collider collision)
    {
        enemy.OnTriggerExit(collision);
    }
}
