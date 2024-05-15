using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAir : EnemyBase
{
    public int m_enemyAirPosY;  //空のエネミーのY座標のポジション

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
    }

    /// <summary>
    /// 物理挙動の更新処理
    /// </summary>
    public override void FixedUpdate()
    {
        base.FixedUpdate();

        Transform transform = this.transform; //オブジェクトを取得

        Vector3 pos = transform.position;  //Transformの情報からVector3の位置情報を取得

        pos.x += m_enemySpeed;  //X座標にスピードを加算する
        pos.z += m_enemySpeed;  //Z座標にスピードを加算する
        pos.y = m_enemyAirPosY;  //Y座標を設定

        transform.position = pos;  //自分の座標に値を設定
    }

}
