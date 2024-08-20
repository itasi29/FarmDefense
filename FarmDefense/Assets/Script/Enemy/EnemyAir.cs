using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAir : EnemyBase
{
    /* �萔 */
    private const int kAirWaitRate = 1;
    private const float kAngleSpeed = 180 / Mathf.PI * 0.02f;    // ��]���x
    private Quaternion kCircleMotionRot = Quaternion.AngleAxis(kAngleSpeed, Vector3.up);    // ����̃N�I�[�^�j�I��

    /* �ϐ� */
    [SerializeField] private float _enemyAirPosY; // ���Y���W
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

        // �󒆑ҋ@���Ԃ͍U�����Ԃ�kAirWaitRate�{����
        _airWaitInterval = _status.attackInterval * kAirWaitRate;
        // ������
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

        // �_�ꂪ�j�󂳂ꂽ�玟�̔_���
        if (_farmScript.IsBreak)
        {
            FindFarm(false);
        }

        // �U���ҋ@����
        base.AttackInterval();

        ReduceDeltaHp();

        if (_isStopMove)
        {
            _rb.velocity = Vector3.zero;
            return;
        }

        // �v���C���[������
        if (_isFindPlayer)
        {
            base.MoveToPlayer();
        }
        // �v���C���[�񔭌���
        else
        {
            // �_��Ɍ�������
            if (!_isInFarm)
            {
                this.MoveToFarm();
            }
            // ����s����
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
            // �_��ڋߒ�
            else if (_isApproachFarm)
            {
                base.MoveToFarm();
            }
            // �U���A�j���[�V������
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
            // �󒆋A�Ғ�
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
        // �܂��_��ֈړ����ɍU���Ώۂ̔_��ɓ�������������
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
        // �񔭌�������
        if (!_isFindPlayer && _farmScript.IsInPlayer)
        {
            _approachPos = _player.transform.position;
            _approachPos.y = _enemyAirPosY;
            
        }
        // �������񔭌�
        if (_isFindPlayer && !_farmScript.IsInPlayer)
        {
            _isCircleMotion = false;
            _isApproachFarm = false;
            _isAirReturn = true;
        }

        _isFindPlayer = _farmScript.IsInPlayer;
    }

    /// <summary>
    /// �󒆂ł̔_��Ɍ������Ă̈ړ�
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
    /// ����s��
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
    /// �󒆂ւ̋A�ҍs��
    /// </summary>
    private void ReturnToAir()
    {
        Vector3 pos = transform.position;

        Vector3 velocity = (_approachPos - pos).normalized * _status.speed;

        _rb.velocity = velocity;
    }
}
