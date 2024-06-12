using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyAir : EnemyBase
{
    /* 定数 */
    private const int kAirWaitRate = 3;
    private const float kAngleSpeed = 180 / Mathf.PI * 0.2f;    // 回転速度
    private Quaternion kCircleMotionRot = Quaternion.AngleAxis(kAngleSpeed, Vector3.up);    // 旋回のクオータニオン

    /* 変数 */
    [SerializeField] private int _enemyAirPosY; // 飛ぶY座標
    private int _airWaitFrame;
    private int _airWaitInterval;
    private bool _isMoveFarm;
    private bool _isCircleMotion;
    private bool _isApproachFarm;
    private bool _isAirReturn;
    private Vector3 _center;
    private Vector3 _approachPos;

    public override void Init(Vector3 pos, int enemyNo)
    {
        base.Init(pos, enemyNo);
        FindFarm(false);

        // 狙う農場の座標を中心に
        _center = _farm.transform.position;
        // 空中待機時間は攻撃時間のkAirWaitRate倍する
        _airWaitInterval = _status.attackInterval * kAirWaitRate;
        // 初期化
        _airWaitFrame = 0;
        _isMoveFarm = false;
        _isCircleMotion = false;
        _isApproachFarm = false;
        _isAirReturn = false;
    }

    private void FixedUpdate()
    {
        // 農場が破壊されたら次の農場へ
        if (_farmScript.IsBreak)
        {
            FindFarm(false);
            _center = _farm.transform.position;
        }


        // 攻撃待機処理
        base.AttackInterval();

        // プレイヤー発見時
        if (_isFindPlayer)
        {
            base.MoveToPlayer();
        }
        // プレイヤー非発見時
        else
        {
            // 農場に向かい中
            if (_isMoveFarm)
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
        if (_isMoveFarm && collider.gameObject.name == _farm.name)
        {
            _isMoveFarm = false;
            _isCircleMotion = true;

            _airWaitFrame = _airWaitInterval;
        }
    }

    /// <summary>
    /// 空中での農場に向かっての移動
    /// </summary>
    private new void MoveToFarm()
    {
        Vector3 pos = transform.position;
        Vector3 farmPos = _farm.transform.position;
        // Y座標だけ上空に変更
        farmPos.y = _enemyAirPosY;

        Vector3 velocity = (farmPos - pos).normalized * _status.speed;

        transform.position = pos + velocity;
    }

    /// <summary>
    /// 旋回行動
    /// </summary>
    private void MoveCircleMotion()
    {
        Vector3 pos = transform.position;

        pos -= _center;
        pos = kCircleMotionRot * pos;
        pos += _center;

        transform.position = pos;
    }

    /// <summary>
    /// 空中への帰還行動
    /// </summary>
    private void ReturnToAir()
    {
        Vector3 pos = transform.position;

        Vector3 velocity = (_approachPos - pos).normalized * _status.speed;

        transform.position = pos + velocity;
    }
}
