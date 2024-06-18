using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyGround : EnemyBase
{
    public override void Init(Vector3 pos, string enemyID)
    {
        base.Init(pos, enemyID);
        FindFarm(true);
    }

    private void FixedUpdate()
    {
        // �_�ꂪ�j�󂳂ꂽ�玟�̔_���
        if (_farmScript.IsBreak)
        {
            FindFarm(true);
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
            base.MoveToFarm();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Farm")
        {
            AttackFarm();
        }
        if (collision.gameObject.tag == "Player")
        {
            AttackPlayer();
        }
    }
}
