using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyGround : EnemyBase
{
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
        // 農場が破壊されたら次の農場へ
        if (_farmScript.IsBreak)
        {
            FindFarm(true);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log(Time.time + "| in:" + other.gameObject.name + "," + other.gameObject.tag);
            _isFindPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log(Time.time + "| out:" + other.gameObject.name + "," + other.gameObject.tag);
            _isFindPlayer = false;
        }
    }
}
