using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class EnemyAir : EnemyBase
{
    public int m_enemyAirPosY;  //空のエネミーのY座標のポジション
    private int m_enemyRadius;  //旋回の円運動の半径
    private int m_enemyRadiusSpeed; //旋回するときの速さ
    private bool _enemyAirMove;  //農場にむかう行動フラグ
    private bool _circularmotion;  //円運動をするフラグ
    private bool _airAttak;  //敵が攻撃するフラグ
    private float _airAttakTime; //敵が攻撃するまでの間隔
    private float _airMoveX;  //敵のX軸行動
    private float _airMoveY;  //敵のY軸行動
    private float _airMoveZ;  //敵のZ軸行動
    float x;  //X軸
    float z;  //Z軸
    //private Vector3 defPosition;  //Vector3で位置情報を取得
    private Vector3 pos;

    /// <summary>
    /// スタート関数
    /// </summary>
    void Start()
    {
        m_enemyRadius = 3;
        m_enemyRadiusSpeed = 1;
        _airAttakTime = 0.0f;
        _enemyAirMove = false;
        _airAttak = false;
        _circularmotion = false;
        m_enemyAirPosY = 5;

        _airMoveX = 0.0f;
        _airMoveY = 0.0f;
        _airMoveZ = 0.0f;
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    new void Update()
    {
        if (_circularmotion == true)   //_circularmotionがtrueなら旋回行動
        {

            _airAttakTime = Time.time;  //攻撃までの間隔を進める

            x = m_enemyRadius * Mathf.Sin(Time.time * m_enemyRadiusSpeed);
            z = m_enemyRadius * Mathf.Cos(Time.time * m_enemyRadiusSpeed);

            transform.position = new Vector3(x + pos.x, pos.y + m_enemyAirPosY, z + pos.z);

            if (_airAttakTime >= 10.0f)
            {
                _airAttak = true;  //攻撃フラグを可能にする

                _circularmotion = false;  //旋回行動をやめる

            }
        }
        else if(_airAttak == true) //攻撃フラグがtrueなら
        {
            float sb,sbx, sby, sbz; //敵の移動速度を設定する

            sbx = base.target.transform.position.x - pos.x;
            sby = base.target.transform.position.y - pos.y;
            sbz = base.target.transform.position.z - pos.z;

            sb = Mathf.Sqrt(sbx * sbx + sby * sby + sbz * sbz);

            _airMoveX = sbx / sb * m_enemySpeed;
            _airMoveY = sby / sb * m_enemySpeed;
            _airMoveZ = sbz / sb * m_enemySpeed;

            transform.position = new Vector3(pos.x + _airMoveX, pos.y + _airMoveY, pos.z + _airMoveZ);
            
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
        if (_circularmotion == false && _enemyAirMove == false)    //_circularmotionがfalseなら農場へ向かう行動(最初だけ)
        {
            Transform trans = this.transform;

            pos = transform.position;  //ポジションをVector3で定義する

            pos.y = m_enemyAirPosY;   //Y座標を設定

            transform.position = pos;

            base.FixedUpdate();
        }

    }

    /// <summary>
    /// 農場の当たり判定
    /// </summary>
    /// <param name="collision"></param>
    public override void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.name == "Farm")
        {
            base.OnCollisionStay(collision);

            _airAttak = false;  //攻撃をしたらfalseに戻す

            _airAttakTime = _airAttakTime - Time.time;  //初期に戻す
        }
    }

    private new void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.name == "Farm")  //物体に当たったら
        {
            if(_circularmotion == false　&& _airAttak == false) //_circularmotionがfalseで_airAttackがfalseなら
            {
                _circularmotion = true;  //_circularmotionをtrueにする

                _enemyAirMove = true;  //農場に到達した
            }
        }

        farm.OnTriggerEnter(collision);  //農場の索敵範囲
    }

    /// <summary>
    /// 農場の索敵範囲
    /// </summary>
    /// <param name="collision"></param>
    private new void OnTriggerExit(Collider collision)
    {
        farm.OnTriggerExit(collision);  //農場の索敵範囲
    }

}
