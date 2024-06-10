using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGround : EnemyBase
{
    private void Start()
    {
        targetBase = GameObject.Find("Farm").gameObject;

        FindFarm();
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    public override void Update()
    {
        base.Update();
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    public override void Init(Vector3 pos)
    {
        base.Init(pos);
    }

    protected override void FindFarm()
    {
        int childIdx = 0;

        bool isFirst = false;
        float dis = 0.0f;

        for (int i = 0; i < FarmManager.kFarmNum; ++i)
        {
            Farm tempFarm = targetBase.transform.GetChild(i).gameObject.GetComponent<Farm>();

            if (tempFarm.IsBreak) continue;

            if (isFirst)
            {
                var childSqrLen = targetBase.transform.GetChild(i).transform.position.sqrMagnitude;

                if (dis > childSqrLen)
                {
                    dis = childSqrLen;
                    childIdx = i;
                }
            }
            else
            {
                dis = targetBase.transform.GetChild(i).transform.position.sqrMagnitude;
                childIdx = i;

                isFirst = true;
            }

        }

        var pos = targetBase.transform.GetChild(childIdx).transform.position;
        // Init(pos);
        target = targetBase.transform.GetChild(childIdx).gameObject;
        farm = target.GetComponent<Farm>();
    }

    /// <summary>
    /// 物理挙動の更新処理
    /// </summary>
    public override void FixedUpdate()
    {
        if (farm.IsBreak)
        {
            FindFarm();
        }

        if(_isFindPlayer == false)  //Playerを発見していない状況
        {
            base.FixedUpdate();
        }
        else if(_isFindPlayer == true)  //Playerを発見している状況
        {
            Transform transform = this.transform;  //オブジェクトを取得

            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, _speed/* * Time.deltaTime*/);  //playerへ向かう
        }
       
    }

    /// <summary>
    /// 農場との当たり判定
    /// </summary>
    /// <param name="collision"></param>
    public override void OnCollisionStay(Collision collision)
    {
        base.OnCollisionStay(collision);
    }

    /// <summary>
    /// プレイヤーが索敵範囲に入ったら
    /// </summary>
    /// <param name="collision"></param>
    public override void OnTriggerEnter(Collider collision)
    {
        base.OnTriggerEnter(collision);
    }

    /// <summary>
    /// プレイヤーが索敵範囲を出たら
    /// </summary>
    /// <param name="collision"></param>
    public override void OnTriggerExit(Collider collision)
    {
        base.OnTriggerExit(collision);
    }
}
