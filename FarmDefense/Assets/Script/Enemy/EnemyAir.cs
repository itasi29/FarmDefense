using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyAir : EnemyBase
{
    /* 定数 */
    private const int kAirWaitRate = 5;
    private const float kAngleSpeed = 180 / Mathf.PI * 0.02f;    // 回転速度
    private Quaternion kCircleMotionRot = Quaternion.AngleAxis(kAngleSpeed, Vector3.up);    // 旋回のクオータニオン

    /* 変数 */
    [SerializeField] private float _enemyAirPosY; // 飛ぶY座標
    private int _airWaitFrame;
    private int _airWaitInterval;
    private bool _isInFarm;
    private bool _isCircleMotion;
    private bool _isApproachFarm;
    private bool _isAirReturn;
    private Vector3 _approachPos;

    private void Start()
    {
        Init(transform.position, 1);
    }

    public override void Init(Vector3 pos, int enemyNo)
    {
        pos.y = _enemyAirPosY;
        base.Init(pos, enemyNo);
        FindFarm(false);

        // 空中待機時間は攻撃時間のkAirWaitRate倍する
        _airWaitInterval = _status.attackInterval * kAirWaitRate;
        // 初期化
        _airWaitFrame = 0;
        _isInFarm = false;
        _isCircleMotion = false;
        _isApproachFarm = false;
        _isAirReturn = false;
    }

    private void FixedUpdate()
    {
        PlayerFindInfo();

        // 農場が破壊されたら次の農場へ
        if (_farmScript.IsBreak)
        {
            FindFarm(false);
        }

        // 攻撃待機処理
        base.AttackInterval();

        if (_isStopMove)
        {
            _rb.velocity = Vector3.zero;
            return;
        }

        // プレイヤー発見時
        if (_isFindPlayer)
        {
            base.MoveToPlayer();
        }
        // プレイヤー非発見時
        else
        {
            // 農場に向かい中
            if (!_isInFarm)
            {
                this.MoveToFarm();
            }
            // 旋回行動中
            else if (_isCircleMotion)
            {
                MoveCircleMotion();

                --_airWaitFrame;
                if (_airWaitFrame < 0)
                {
                    _isCircleMotion = false;
                    _isApproachFarm = true;
                    _approachPos = transform.position;
                }
            }
            // 農場接近中
            else if (_isApproachFarm)
            {
                base.MoveToFarm();
            }
            // 空中帰還中
            else if (_isAirReturn)
            {
                ReturnToAir();

                // 空中まで戻ったら旋回行動開始
                if (transform.position.y >= _enemyAirPosY)
                {
                    _isAirReturn = false;
                    _isCircleMotion = true;

                    _airWaitFrame = _airWaitInterval;
                    _rb.velocity = Vector3.zero;

                    Vector3 pos = transform.position;
                    pos.y = _enemyAirPosY;
                    transform.position = pos;
                }
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Farm")
        {
            _isApproachFarm = false;
            _isAirReturn = true;
            AttackFarm();
        }
        else if (collision.gameObject.tag == "Player")
        {
            AttackPlayer();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        // まだ農場へ移動中に攻撃対象の農場に当たったら旋回へ
        if (!_isInFarm && collider.gameObject.name == _farm.name)
        {
            _isInFarm = true;
            _isCircleMotion = true;

            _airWaitFrame = _airWaitInterval;
            _rb.velocity = Vector3.zero;
        }
    }

    private void PlayerFindInfo()
    {
        // 非発見→発見
        if (!_isFindPlayer && _farmScript.IsInPlayer)
        {
            _approachPos = _player.transform.position;
            _approachPos.y = _enemyAirPosY;
            
        }
        // 発見→非発見
        if (_isFindPlayer && !_farmScript.IsInPlayer)
        {
            _isCircleMotion = false;
            _isApproachFarm = false;
            _isAirReturn = true;
        }

        _isFindPlayer = _farmScript.IsInPlayer;
    }

    /// <summary>
    /// 空中での農場に向かっての移動
    /// </summary>
    private new void MoveToFarm()
    {
        Vector3 pos = transform.position;
        Vector3 farmPos = _farm.transform.position;
        farmPos.y = _enemyAirPosY;

        Vector3 velocity = (farmPos - pos).normalized * _status.speed;
        _rb.velocity = velocity;
    }

    /// <summary>
    /// 旋回行動
    /// </summary>
    private void MoveCircleMotion()
    {
        Vector3 pos = transform.position;
        Vector3 center = _farm.transform.position;

        pos -= center;
        pos = kCircleMotionRot * pos;
        pos += center;

        transform.position = pos;
    }

    /// <summary>
    /// 空中への帰還行動
    /// </summary>
    private void ReturnToAir()
    {
        Vector3 pos = transform.position;

        Vector3 velocity = (_approachPos - pos).normalized * _status.speed;

        _rb.velocity = velocity;
    }
}
