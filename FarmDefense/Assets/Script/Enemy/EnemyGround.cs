using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGround : EnemyBase
{
    // HACK:�@���u�����ɓ��삷��悤
    //        ��X�͏���
    private void Start()
    {
        Init(this.transform.position, 0);
    }

    public override void Init(Vector3 pos, int enemyNo)
    {
        base.Init(pos, enemyNo);
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

        // �v���C���[������
        if (_isFindPlayer)
        {
            base.MoveToPlayer();
        }
        // �v���C���[�񔭌���
        else
        {
            base.MoeveToFarm();
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
