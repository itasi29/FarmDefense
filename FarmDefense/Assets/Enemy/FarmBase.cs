using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmBase : MonoBehaviour
{
    public int m_farmHp = 100;  //�_��̗̑�

    public Vector3 posFarm;

    /// <summary>
    /// �X�V����
    /// </summary>
    public virtual void Update()
    {

    }

    /// <summary>
    /// ����������
    /// </summary>
    public virtual void Init()
    {
        posFarm = this.transform.position;
    }

    /// <summary>
    /// ���������̍X�V����
    /// </summary>
    public virtual void FixedUpdate()
    {

    }
}
