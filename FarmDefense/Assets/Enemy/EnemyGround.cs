using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGround : EnemyBase
{
    private void Start()
    {
#if false
        targetBase = GameObject.Find("Farm").gameObject;
        player = GameObject.Find("Player").gameObject;

        FindFarm();
#endif
        Init(this.transform.position);
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
        targetBase = GameObject.Find("Farm").gameObject;
        player = GameObject.Find("Player").gameObject;

        base.Init(pos);
        FindFarm();
    }

    protected override void FindFarm()
    {
        int childIdx = 0;

        bool isFirst = true;
        float dis = 0.0f;

        Vector3 pos = this.transform.position;

        for (int i = 0; i < FarmManager.kFarmNum; ++i)
        {
            Farm tempFarm = targetBase.transform.GetChild(i).gameObject.GetComponent<Farm>();

            if (tempFarm.IsBreak) continue;

            if (isFirst)
            {
                Vector3 childPos = targetBase.transform.GetChild(i).transform.position;
                dis = (pos - childPos).sqrMagnitude;
                childIdx = i;

                isFirst = false;
            }
            else
            {
                Vector3 childPos = targetBase.transform.GetChild(i).transform.position;
                var childSqrLen = (pos - childPos).sqrMagnitude;

                if (dis > childSqrLen)
                {
                    dis = childSqrLen;
                    childIdx = i;
                }
            }

        }

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

            //transform.position = Vector3.MoveTowards(transform.position, player.transform.position, _speed/* * Time.deltaTime*/);  //playerへ向かう

            Vector3 pos = transform.position;
            Vector3 tarPos = player.transform.position;

            Vector3 move = (tarPos - pos).normalized * _speed;

            transform.position = pos + move;
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
