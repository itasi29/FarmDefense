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
    public override void Init(Vector3 pos)
    {
        base.Init(pos);
    }

    /// <summary>
    /// 物理挙動の更新処理
    /// </summary>
    public override void FixedUpdate()
    {
        if(_isFindPlayer == false)  //Playerを発見していない状況
        {
            base.FixedUpdate();
        }
        else if(_isFindPlayer == true)  //Playerを発見している状況
        {
            Transform transform = this.transform;  //オブジェクトを取得

            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, _speed * Time.deltaTime);  //playerへ向かう
        }
       
       
    }

    /// <summary>
    /// 農場との当たり判定
    /// </summary>
    /// <param name="collision"></param>
    public override void OnCollisionStay(Collision collision)
    {
        base.OnCollisionStay(collision);
    }

    /// <summary>
    /// プレイヤーが索敵範囲に入ったら
    /// </summary>
    /// <param name="collision"></param>
    public override void OnTriggerEnter(Collider collision)
    {
        base.OnTriggerEnter(collision);
    }

    /// <summary>
    /// プレイヤーが索敵範囲を出たら
    /// </summary>
    /// <param name="collision"></param>
    public override void OnTriggerExit(Collider collision)
    {
        base.OnTriggerExit(collision);
    }
}
