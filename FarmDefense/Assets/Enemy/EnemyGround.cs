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
    public override void Init()
    {
        base.Init();
    }

    /// <summary>
    /// ���������̍X�V����
    /// </summary>
    public override void FixedUpdate()
    {
        base.FixedUpdate();

       
    }

    /// <summary>
    /// �_��Ƃ̓����蔻��
    /// </summary>
    /// <param name="collision"></param>
    public override void OnCollisionStay(Collision collision)
    {
        base.OnCollisionStay(collision);
    }
}
