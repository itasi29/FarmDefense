using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmBase : MonoBehaviour
{
    public int m_farmHp = 100;  //農場の体力

    public Vector3 posFarm;

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
}
