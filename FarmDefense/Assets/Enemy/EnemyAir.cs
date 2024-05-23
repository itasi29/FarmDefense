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
    private bool _airRetrun;  //敵が上空に戻るフラグ
    private float _airAttakTime; //敵が攻撃するまでの間隔
    float x;  //X軸
    float z;  //Z軸
    //private Vector3 defPosition;  //Vector3で位置情報を取得
    private Vector3 pos;

    /// <summary>
    /// スタート関数
    /// </summary>
    void Start()
    {
        m_enemyRadius = 4;
        m_enemyRadiusSpeed = 2;
        _airAttakTime = 0.0f;
        _enemyAirMove = false;
        _airAttak = false;
        _circularmotion = false;
        m_enemyAirPosY = 9;
        _airRetrun = false;
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
        Vector3 start = transform.position;
        Vector3 air = new Vector3(target.transform.position.x, m_enemyAirPosY, target.transform.position.z);
        float timer = 0;
        timer += Time.deltaTime;

        if (_circularmotion == false && _enemyAirMove == false)    //_circularmotionがfalseなら農場へ向かう行動(最初だけ)
        {
            Transform trans = this.transform;

            pos = transform.position;  //ポジションをVector3で定義する

            pos.y = m_enemyAirPosY;   //Y座標を設定

            transform.position = pos;

            transform.position = Vector3.Lerp(start, air, timer);  //農場の上空に行く
        }

        if (_circularmotion == true)   //_circularmotionがtrueなら旋回行動
        {

            _airAttakTime += Time.deltaTime;  //攻撃までの間隔を進める

            x = m_enemyRadius * Mathf.Sin(_airAttakTime * m_enemyRadiusSpeed);
            z = m_enemyRadius * Mathf.Cos(_airAttakTime * m_enemyRadiusSpeed);

            transform.position = new Vector3(x, pos.y, z);

            if (_airAttakTime >= 15.0f)
            {
                _airAttak = true;  //攻撃フラグを可能にする

                _circularmotion = false;  //旋回行動をやめる

                _airAttakTime = 0.0f;  //初期化する

            }
        }

        if (_airAttak == true) //攻撃フラグがtrueなら
        {
            base.FixedUpdate();  //農場にむかう行動
        }

        if (_airRetrun == true)  //_airRetrunがtrueなら
        {
            
            transform.position = Vector3.Lerp(start, air, timer);  //農場の上空に行く

            if (start.y >= 8.8f)  //一定の高さに到達したら
            {
                _circularmotion = true;  //_circularmotionをtrueにする

                _airRetrun = false;  //_airRetrunをfalseにする
            }
        }

    }

    /// <summary>
    /// 農場の当たり判定
    /// </summary>
    /// <param name="collision"></param>
    public override void OnCollisionStay(Collision collision)
    {
        if(_airAttak == true)  //攻撃フラグがtrueなら
        {
            base.OnCollisionStay(collision);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Farm")  //Farmに触れたら
        {
            _airAttak = false;  //攻撃をしたらfalseに戻す

            _airRetrun = true; //_airRetrunをtrueにする
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.name == "Farm")  //物体に当たったら
        {
            if(_circularmotion == false　&& _airAttak == false) //_circularmotionがfalseで_airAttackがfalseなら
            {
                _circularmotion = true;  //_circularmotionをtrueにする

                _enemyAirMove = true;  //農場に到達した
            }
        }
    }

}
