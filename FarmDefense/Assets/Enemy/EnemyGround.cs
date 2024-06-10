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
    /// �X�V����
    /// </summary>
    public override void Update()
    {
        base.Update();
    }

    /// <summary>
    /// ����������
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
    /// ���������̍X�V����
    /// </summary>
    public override void FixedUpdate()
    {
        if (farm.IsBreak)
        {
            FindFarm();
        }

        if(_isFindPlayer == false)  //Player�𔭌����Ă��Ȃ���
        {
            base.FixedUpdate();
        }
        else if(_isFindPlayer == true)  //Player�𔭌����Ă����
        {
            Transform transform = this.transform;  //�I�u�W�F�N�g���擾

            //transform.position = Vector3.MoveTowards(transform.position, player.transform.position, _speed/* * Time.deltaTime*/);  //player�֌�����

            Vector3 pos = transform.position;
            Vector3 tarPos = player.transform.position;

            Vector3 move = (tarPos - pos).normalized * _speed;

            transform.position = pos + move;
        }
       
    }

    /// <summary>
    /// �_��Ƃ̓����蔻��
    /// </summary>
    /// <param name="collision"></param>
    public override void OnCollisionStay(Collision collision)
    {
        base.OnCollisionStay(collision);
    }

    /// <summary>
    /// �v���C���[�����G�͈͂ɓ�������
    /// </summary>
    /// <param name="collision"></param>
    public override void OnTriggerEnter(Collider collision)
    {
        base.OnTriggerEnter(collision);
    }

    /// <summary>
    /// �v���C���[�����G�͈͂��o����
    /// </summary>
    /// <param name="collision"></param>
    public override void OnTriggerExit(Collider collision)
    {
        base.OnTriggerExit(collision);
    }
}
