using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyAir : EnemyBase
{
    /* �萔 */
    private const int kAirWaitRate = 5;
    private const float kAngleSpeed = 180 / Mathf.PI * 0.02f;    // ��]���x
    private Quaternion kCircleMotionRot = Quaternion.AngleAxis(kAngleSpeed, Vector3.up);    // ����̃N�I�[�^�j�I��

    /* �ϐ� */
    [SerializeField] private float _enemyAirPosY; // ���Y���W
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

        // �󒆑ҋ@���Ԃ͍U�����Ԃ�kAirWaitRate�{����
        _airWaitInterval = _status.attackInterval * kAirWaitRate;
        // ������
        _airWaitFrame = 0;
        _isInFarm = false;
        _isCircleMotion = false;
        _isApproachFarm = false;
        _isAirReturn = false;
    }

    private void FixedUpdate()
    {
        PlayerFindInfo();

        // �_�ꂪ�j�󂳂ꂽ�玟�̔_���
        if (_farmScript.IsBreak)
        {
            FindFarm(false);
        }

        // �U���ҋ@����
        base.AttackInterval();

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
            // �󒆋A�Ғ�
            else if (_isAirReturn)
            {
                ReturnToAir();

                // �󒆂܂Ŗ߂��������s���J�n
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
