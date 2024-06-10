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

            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, _speed/* * Time.deltaTime*/);  //player�֌�����
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
