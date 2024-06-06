using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGround : EnemyBase
{


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

    /// <summary>
    /// ���������̍X�V����
    /// </summary>
    public override void FixedUpdate()
    {
        if(_isFindPlayer == false)  //Player�𔭌����Ă��Ȃ���
        {
            base.FixedUpdate();
        }
        else if(_isFindPlayer == true)  //Player�𔭌����Ă����
        {
            Transform transform = this.transform;  //�I�u�W�F�N�g���擾

            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, _speed * Time.deltaTime);  //player�֌�����
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
