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
            // 動くアニメーションしている場合は止める
            if (_anim.GetBool(kAnimParmInfo[AnimParm.kMove]))
            {
                _anim.SetBool(kAnimParmInfo[AnimParm.kMove], false);
            }

            _rb.velocity = Vector3.zero;
            return;
        }

        // 動くアニメーションしていない場合は始める
        if (!_anim.GetBool(kAnimParmInfo[AnimParm.kMove]))
        {
            _anim.SetBool(kAnimParmInfo[AnimParm.kMove], true);
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
