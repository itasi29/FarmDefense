using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class EnemyAir : EnemyBase
{
    public int m_enemyAirPosY;  //空のエネミーのY座標のポジション
    private float m_enemyRadius;  //旋回の円運動の半径
    private float m_enemyRadiusSpeed; //旋回するときの速さ
    private float m_enemyRadiusTime;  //旋回する時間
    private bool _circularmotion;  //円運動をするフラグ
    float x;  //X軸
    float z;  //Z軸
    //private Vector3 defPosition;  //Vector3で位置情報を取得
    Transform trans;
    private Vector3 pos;

    /// <summary>
    /// スタート関数
    /// </summary>
    private void Start()
    {
        m_enemyRadius = 1.2f;
        m_enemyRadiusSpeed = 1.0f;
        m_enemyRadiusTime = 0.0f;

        _circularmotion = false;
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    private new void Update()
    {
        

        if (_circularmotion == true)   //_circularmotionがtrueなら旋回行動
        {
            m_enemyRadiusTime = Time.time;

            x = m_enemyRadius * Mathf.Sin(m_enemyRadiusTime * m_enemyRadiusSpeed);
            z = m_enemyRadius * Mathf.Cos(m_enemyRadiusTime * m_enemyRadiusSpeed);

            m_enemyAirPosY = 5;

            transform.position = new Vector3(x + pos.x, pos.y + m_enemyAirPosY, z + pos.z);
        }

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
        trans = this.transform;

        pos = transform.position;  //ポジションをVector3で定義する

        pos.y = m_enemyAirPosY;   //Y座標を設定

        transform.position = pos;

        if (_circularmotion == false)    //_circularmotionがfalseなら農場へ向かう行動
        {
            base.FixedUpdate();   
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

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.name == "Farm")  //物体に当たったら
        {
            if(_circularmotion == false) //_circularmotionがfalseなら
            {

                m_enemyRadiusTime = m_enemyRadiusTime - Time.time;

                _circularmotion = true;  //_circularmotionをtrueにする
            }
        }
    }

}
