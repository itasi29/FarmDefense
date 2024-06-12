using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyAir : EnemyBase
{
    /* �萔 */
    private const int kAirWaitRate = 3;
    private const float kAngleSpeed = 180 / Mathf.PI * 0.2f;    // ��]���x
    private Quaternion kCircleMotionRot = Quaternion.AngleAxis(kAngleSpeed, Vector3.up);    // ����̃N�I�[�^�j�I��

    /* �ϐ� */
    [SerializeField] private int _enemyAirPosY; // ���Y���W
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

        // �_���_��̍��W�𒆐S��
        _center = _farm.transform.position;
        // �󒆑ҋ@���Ԃ͍U�����Ԃ�kAirWaitRate�{����
        _airWaitInterval = _status.attackInterval * kAirWaitRate;
        // ������
        _airWaitFrame = 0;
        _isMoveFarm = false;
        _isCircleMotion = false;
        _isApproachFarm = false;
        _isAirReturn = false;
    }

    private void FixedUpdate()
    {
        // �_�ꂪ�j�󂳂ꂽ�玟�̔_���
        if (_farmScript.IsBreak)
        {
            FindFarm(false);
            _center = _farm.transform.position;
        }


        // �U���ҋ@����
        base.AttackInterval();

        // �v���C���[������
        if (_isFindPlayer)
        {
            base.MoveToPlayer();
        }
        // �v���C���[�񔭌���
        else
        {
            // �_��Ɍ�������
            if (_isMoveFarm)
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
        if (_isMoveFarm && collider.gameObject.name == _farm.name)
        {
            _isMoveFarm = false;
            _isCircleMotion = true;

            _airWaitFrame = _airWaitInterval;
        }
    }

    /// <summary>
    /// �󒆂ł̔_��Ɍ������Ă̈ړ�
    /// </summary>
    private new void MoveToFarm()
    {
        Vector3 pos = transform.position;
        Vector3 farmPos = _farm.transform.position;
        // Y���W�������ɕύX
        farmPos.y = _enemyAirPosY;

        Vector3 velocity = (farmPos - pos).normalized * _status.speed;

        transform.position = pos + velocity;
    }

    /// <summary>
    /// ����s��
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
    /// �󒆂ւ̋A�ҍs��
    /// </summary>
    private void ReturnToAir()
    {
        Vector3 pos = transform.position;

        Vector3 velocity = (_approachPos - pos).normalized * _status.speed;

        transform.position = pos + velocity;
    }
}
