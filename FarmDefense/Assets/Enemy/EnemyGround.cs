using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGround : EnemyBase
{

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

        transform.position = pos;  //自分の座標に値を設定
    }

}
