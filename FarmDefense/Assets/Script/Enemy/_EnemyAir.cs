using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class _EnemyAir : _EnemyBase
{
    [SerializeField] private const float kAngleSpeed = 180 / Mathf.PI * 0.2f;

    public int m_enemyAirPosY;  //空のエネミーのY座標のポジション
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

    /// <summary>
    /// スタート関数
    /// </summary>
    void Start()
    {
#if false
        targetBase = GameObject.Find("Farm").gameObject;
        player = GameObject.Find("Player").gameObject;

        _airAttakTime = 0.0f;
        _enemyAirMove = false;
        _airAttak = false;
        _circularmotion = false;
        m_enemyAirPosY = 5;

        _airReturn = false;

        FindFarm();
#endif
        Init(this.transform.position);
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
    public override void Init(Vector3 pos)
    {
        targetBase = GameObject.Find("Farm").gameObject;
        player = GameObject.Find("Player").gameObject;

        _airAttakTime = 0.0f;
        _enemyAirMove = false;
        _airAttak = false;
        _circularmotion = false;
        m_enemyAirPosY = 5;

        _airReturn = false;

        base.Init(pos);
        FindFarm();
    }

    protected override void FindFarm()
    {
        int childIdx = 0;

        bool isFirst = true;
        float dis = 0.0f;

        Vector3 pos = this.transform.position;

        for (int i = 0; i < FarmManager.kFarmNum; ++i)
        {
            Farm tempFarm = targetBase.transform.GetChild(i).gameObject.GetComponent<Farm>();
            if (tempFarm.IsBreak) continue;

            if (isFirst)
            {
                Vector3 childPos = targetBase.transform.GetChild(i).transform.position;
                dis = (pos - childPos).sqrMagnitude;
                childIdx = i;

                isFirst = false;
            }
            else
            {
                Vector3 childPos = targetBase.transform.GetChild(i).transform.position;
                var childSqrLen = (pos - childPos).sqrMagnitude;

                if (dis < childSqrLen)
                {
                    dis = childSqrLen;
                    childIdx = i;
                }
            }

        }

        target = targetBase.transform.GetChild(childIdx).gameObject;
        farm = target.GetComponent<Farm>();
    }

    /// <summary>
    /// 物理挙動の更新処理
    /// </summary>
    public override void FixedUpdate()
    {
        if (farm.IsBreak)
        {
            FindFarm();
            _circularmotion = false;
            _enemyAirMove = false;
        }

        _isFindPlayer = farm.IsInPlayer;

        if (_isFindPlayer == true)   //m_playerがtrueならプレイヤーへ向かう
        {
            Transform transform = this.transform;  //オブジェクトを取得

            //            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, _speed * Time.deltaTime);  //プレイヤーに向かう
            Vector3 pos = transform.position;
            Vector3 tarPos = player.transform.position;

            Vector3 move = (tarPos - pos).normalized * _speed;

            transform.position = pos + move;
        }
        else if(_isFindPlayer == false)   //m_playerがfalseなら
        {
            if (_circularmotion == false && _enemyAirMove == false)    //_circularmotionがfalse、_enemyAirMoveもfalseなら農場へ向かう行動(最初だけ)
            {
                pos = transform.position;

                Vector3 farm = new Vector3(target.transform.position.x, m_enemyAirPosY, target.transform.position.z);

                float timer = 0;

                timer += Time.deltaTime;

                Vector3 move = (farm - pos).normalized  * _speed;

                transform.position = pos + move;

            }
            if (_airReturn == true)  //空中に戻るフラグがtrueなら
            {
                pos = transform.position;

                Vector3 farm = new Vector3(target.transform.position.x - 2.0f, m_enemyAirPosY, target.transform.position.z);

                float timer = 0;

                timer += Time.deltaTime;

                Vector3 move = (farm - pos).normalized * _speed;

                transform.position = pos + move;

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

                var angleAxis = Quaternion.AngleAxis(kAngleSpeed * _period * Time.deltaTime, _axis);  //回転のクォータニオン作成

                var circlepos = tr.position;  //円運動の位置計算

                Debug.Log("center" + _center);
                Debug.Log("befor" + circlepos);
                circlepos -= _center;
                Debug.Log("-" + circlepos);
                circlepos = angleAxis * circlepos;
                Debug.Log("q" + circlepos);
                circlepos += _center;
                Debug.Log("+" + circlepos);

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

        if (collision.gameObject.tag == "Farm")
        {
            _airAttak = false;  //攻撃をしたらfalseに戻す

            _airReturn = true;  //空中に戻す

            _airAttakTime = 0;  //初期に戻す
        }
        else if(collision.gameObject.tag == "Player")  //Playerに当たったら
        {
            _airReturn = true;  //空中に戻す
        }
    }

    private new void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == target.name)  //物体に当たったら
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
