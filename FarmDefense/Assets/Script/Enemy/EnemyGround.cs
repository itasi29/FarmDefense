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
        if (DeathAfterUpdate()) return;

        // 農場が破壊されたら次の農場へ
        if (_farmScript.IsBreak)
        {
            FindFarm(true);
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
            base.MoveToFarm();
        }

        FrontUpdate();
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
