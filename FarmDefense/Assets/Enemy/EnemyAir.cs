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

    private Vector3 _center;  //中心点

    private Vector3 _axis = Vector3.up;   //回転軸

    private float _period = 5; //円運動周期

    private bool _updateRotation = true;  //向きを更新

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
        if(m_player == true)   //m_playerがtrueならプレイヤーへ向かう
        {
            Transform transform = this.transform;  //オブジェクトを取得

            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, m_enemySpeed * Time.deltaTime);  //プレイヤーに向かう
        }
        else if(m_player == false)   //m_playerがfalseなら
        {
            if (_circularmotion == false && _enemyAirMove == false)    //_circularmotionがfalse、_enemyAirMoveもfalseなら農場へ向かう行動(最初だけ)
            {
                pos = transform.position;

                Vector3 farm = new Vector3(target.transform.position.x, m_enemyAirPosY, target.transform.position.z);

                float timer = 0;

                timer += Time.deltaTime;

                transform.position = Vector3.Lerp(pos, farm, timer);

            }
            if (_airReturn == true)  //空中に戻るフラグがtrueなら
            {
                pos = transform.position;

                Vector3 farm = new Vector3(target.transform.position.x - 2.0f, m_enemyAirPosY, target.transform.position.z);

                float timer = 0;

                timer += Time.deltaTime;

                transform.position = Vector3.Lerp(pos, farm, timer);

                if (pos.y >= 4.9f)
                {
                    _circularmotion = true;  //旋回行動をさせる

                    _airReturn = false; //空中に戻ってきた
                }
            }
            else if (_circularmotion == true)   //_circularmotionがtrueなら旋回行動
            {

                _airAttakTime += Time.deltaTime;  //攻撃までの間隔を進める


                _center = target.transform.position;  //中心点をターゲットの座標にする

                var tr = transform;

                var angleAxis = Quaternion.AngleAxis(360 / _period * Time.deltaTime, _axis);  //回転のクォータニオン作成

                var circlepos = tr.position;  //円運動の位置計算

                circlepos -= _center;
                circlepos = angleAxis * circlepos;
                circlepos += _center;

                tr.position = circlepos;


                if (_airAttakTime >= 10.0f)
                {
                    _airAttak = true;  //攻撃フラグを可能にする

                    _circularmotion = false;  //旋回行動をやめる

                }
            }
            else if (_airAttak == true) //攻撃フラグがtrueなら
            {
                base.FixedUpdate();   //農場に攻撃する
            }
        }

        
        
    }

    /// <summary>
    /// 農場の当たり判定
    /// </summary>
    /// <param name="collision"></param>
    public override void OnCollisionStay(Collision collision)
    {
        base.OnCollisionStay(collision);

        if (collision.gameObject.name == "Farm")
        {
            _airAttak = false;  //攻撃をしたらfalseに戻す

            _airReturn = true;  //空中に戻す

            _airAttakTime = 0;  //初期に戻す
        }
        else if(collision.gameObject.name == "Player")  //Playerに当たったら
        {
            _airReturn = true;  //空中に戻す
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
