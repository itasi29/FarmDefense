using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAir : EnemyBase
{
    /* 定数 */
    private const int kAirWaitRate = 1;
    private const float kAngleSpeed = 180 / Mathf.PI * 0.02f;    // 回転速度
    private Quaternion kCircleMotionRot = Quaternion.AngleAxis(kAngleSpeed, Vector3.up);    // 旋回のクオータニオン

    /* 変数 */
    [SerializeField] private float _enemyAirPosY; // 飛ぶY座標
    private int _airWaitFrame;
    private int _airWaitInterval;
    private bool _isInFarm;
    private bool _isCircleMotion;
    private bool _isApproachFarm;
    private bool _isNowAttack;
    private bool _isAttackAnim;
    private bool _isAirReturn;
    private Vector3 _approachPos;

    public override void Init(Vector3 pos, string enemyID)
    {
        pos.y = _enemyAirPosY;
        base.Init(pos, enemyID);
        FindFarm(false);

        // 空中待機時間は攻撃時間のkAirWaitRate倍する
        _airWaitInterval = _status.attackInterval * kAirWaitRate;
        // 初期化
        _airWaitFrame = 0;
        _isInFarm = false;
        _isCircleMotion = false;
        _isApproachFarm = false;
        _isNowAttack = false;
        _isAttackAnim = false;
        _isAirReturn = false;
    }

    private void FixedUpdate()
    {
        if (DeathAfterUpdate()) return;
        PlayerFindInfo();

        // 農場が破壊されたら次の農場へ
        if (_farmScript.IsBreak)
        {
            FindFarm(false);
        }

        // 攻撃待機処理
        base.AttackInterval();

        ReduceDeltaHp();

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
            // 攻撃アニメーション中
            else if (_isNowAttack)
            {
                if (IsNowPlayClipName("Attack"))
                {
                    _isAttackAnim = true;
                }
                else if (_isAttackAnim)
                {
                    _isAttackAnim = false;
                    _isNowAttack = false;
                    _isAirReturn = true;
                }
            }
            // 空中帰還中
            else if (_isAirReturn)
            {
                ReturnToAir();

                Vector3 pos = transform.position;
                if ((_approachPos - pos).sqrMagnitude < 0.1f)
                {
                    _isAirReturn = false;
                    _isCircleMotion = true;

                    _airWaitFrame = _airWaitInterval;
                    _rb.velocity = Vector3.zero;

                    pos.y = _enemyAirPosY;
                    transform.position = pos;
                }
            }
        }

        FrontUpdate();
    }

    protected new void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if (collision.gameObject.tag == "Farm")
        {
            _isNowAttack = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Farm")
        {
            _rb.velocity = Vector3.zero;
            _isApproachFarm = false;
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

        Vector3 dir = pos - transform.position;
        transform.forward =  dir.normalized;

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
