using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class EnemyAir : EnemyBase
{
    public int m_enemyAirPosY;  //空のエネミーのY座標のポジション
    private int m_enemyRadius;  //旋回の円運動の半径
    private bool _circularmotion;  //円運動をするフラグ
    float x;  //X軸
    float z;  //Z軸
    private Vector3 defPosition;  //Vector3で位置情報を取得

    /// <summary>
    /// スタート関数
    /// </summary>
    private void Start()
    {
        m_enemyRadius = 2;

        defPosition = transform.position;
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    private new void Update()
    {
        x = m_enemyRadius * Mathf.Sin(Time.time * m_enemySpeed);
        z = m_enemyRadius * Mathf.Cos(Time.time * m_enemySpeed);

        transform.position = new Vector3(x + defPosition.x, defPosition.y, z + defPosition.z);
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    //public override void Update()
    //{
    //   //if(_circularmotion == true)
    //   // {
    //   //     x = m_enemyRadius * Mathf.Sin(Time.time * m_enemySpeed);
    //   //     z = m_enemyRadius * Mathf.Cos(Time.time * m_enemySpeed);

    //   //     transform.position = new Vector3(x + defPosition.x, defPosition.y, z + defPosition.z);
    //   // }
    //}

    /// <summary>
    /// 初期化処理
    /// </summary>
    public override void Init()
    {
        base.Init();

        m_enemyAirPosY = 5;
        //m_enemyRadius = 3;
        //_circularmotion = true;

        //defPosition = transform.position;
    }

    /// <summary>
    /// 物理挙動の更新処理
    /// </summary>
    public override void FixedUpdate()
    {
        //Transform transform = this.transform;

        //x = m_enemyRadius * Mathf.Sin(Time.time * m_enemySpeed);
        //z = m_enemyRadius * Mathf.Cos(Time.time * m_enemySpeed);

        //transform.position = new Vector3(x + defPosition.x, defPosition.y, z + defPosition.z);
        //Vector3 pos = transform.position;  //ポジションをVector3で定義する

        //pos.y = m_enemyAirPosY;  //Y座標を設定

        //transform.position = pos;

        
        //if(_circularmotion == false)  //円運動するフラグがfalseなら
        //{
        //    base.FixedUpdate();
        //}
        
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
