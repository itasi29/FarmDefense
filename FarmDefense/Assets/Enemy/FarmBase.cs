using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmBase : MonoBehaviour
{
    public int m_farmHp = 100;  //_๊ฬฬอ

    public Vector3 posFarm;

    public EnemyBase enemy;  //enemyฬNXฤัoต

    /// <summary>
    /// XV
    /// </summary>
    public virtual void Update()
    {

    }

    /// <summary>
    /// ๚ป
    /// </summary>
    public virtual void Init()
    {
        posFarm = this.transform.position;
    }

    /// <summary>
    /// จฎฬXV
    /// </summary>
    public virtual void FixedUpdate()
    {

    }

    /// <summary>
    /// EnemyAirฬ๕Gออภ
    /// </summary>
    /// <param name="collision"></param>
    public void OnTriggerEnter(Collider collision)
    {
        enemy.OnTriggerEnter(collision);
    }

    /// <summary>
    /// EnemyAirฬ๕Gออภ
    /// </summary>
    /// <param name="collision"></param>
    public void OnTriggerExit(Collider collision)
    {
        enemy.OnTriggerExit(collision);
    }
}
