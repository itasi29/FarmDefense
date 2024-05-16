using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAir : EnemyBase
{
    public int m_enemyAirPosY;  //空のエネミーのY座標のポジション
    private int m_enemyRadius;  //旋回の円運動の半径
    private float _x;  //エネミーのX座標移動変数
    private float _z;  //エネミーのZ座標移動変数

    /// <summary>
    /// 更新処理
    /// </summary>
    public override void Update()
    {
        base.Update();
    }

    /// <summary>
    /// 初期化処理
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
    /// 物理挙動の更新処理
    /// </summary>
    public override void FixedUpdate()
    {
        base.FixedUpdate();

        Vector3 pos = transform.position;  //Transformの情報からVector3の位置情報を取得
       
        pos.y = m_enemyAirPosY;  //Y座標を設定

        transform.position = pos;  //自分の座標に値を設定

        if(pos.x == farm.posFarm.x && pos.z == farm.posFarm.z)  //農場と同じX座標とZ座標になったら
        {
            
        }
    }

    /// <summary>
    /// 農場の当たり判定
    /// </summary>
    /// <param name="collision"></param>
    public override void OnCollisionStay(Collision collision)
    {
        base.OnCollisionStay(collision);
    }

}
