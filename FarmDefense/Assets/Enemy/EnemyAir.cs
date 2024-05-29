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
    private bool _airReturn;  //敵が空中に戻るフラグ
    private float _airAttakTime; //敵が攻撃するまでの間隔
    float x;  //X軸
    float z;  //Z軸
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

        _airReturn = false;
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    new void Update()
    {
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
        if (_circularmotion == false && _enemyAirMove == false && m_player == false)    //_circularmotionがfalse、_enemyAirMoveもfalse、m_playerもfalseなら農場へ向かう行動(最初だけ)
        {
            pos = transform.position;

            Vector3 farm = new Vector3(target.transform.position.x, m_enemyAirPosY, target.transform.position.z);

            pos.y = m_enemyAirPosY;   //Y座標を設定

            float timer = 0;

            timer += Time.deltaTime;

            transform.position = Vector3.Lerp(pos, farm, timer);
         
        }

        if (_circularmotion == true && m_player == false)   //_circularmotionがtrue、m_playerがfalseなら旋回行動
        {

            _airAttakTime += Time.deltaTime;  //攻撃までの間隔を進める

            x = m_enemyRadius * Mathf.Sin(_airAttakTime * m_enemyRadiusSpeed);
            z = m_enemyRadius * Mathf.Cos(_airAttakTime * m_enemyRadiusSpeed);

            transform.position = new Vector3(x + pos.x, pos.y, z + pos.z);

            if (_airAttakTime >= 10.0f)
            {
                _airAttak = true;  //攻撃フラグを可能にする

                _circularmotion = false;  //旋回行動をやめる

            }
        }
        else if (_airAttak == true && m_player == false) //攻撃フラグがtrue、m_playerがfalseなら
        {
            base.FixedUpdate();   //農場に攻撃する
        }
        else if(_airReturn == true && m_player == false)  //空中に戻るフラグがtrue、m_playerがfalseなら
        {
            pos = transform.position;

            Vector3 farm = new Vector3(target.transform.position.x, m_enemyAirPosY, target.transform.position.z);

            float timer = 0;

            timer += Time.deltaTime;

            transform.position = Vector3.Lerp(pos, farm, timer);

            if(pos.y >= 4.9f)
            {
                _circularmotion = true;  //旋回行動をさせる
            }
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

            _airReturn = true;  //空中に戻す

            _airAttakTime = 0;  //初期に戻す
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
