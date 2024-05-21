using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmManager : MonoBehaviour
{
    /* �萔 */
    private const int kMaxHp = 100;    // �ő�HP

    /* �ϐ� */
    private int _hp;        // HP
    private bool _isBreak;  // ���Ă��邩

    void Start()
    {
        /* ������ */
        _hp = kMaxHp;
        _isBreak = false;
    }

    private void FixedUpdate()
    {
        // MEMO : Debug�p�ŏo���Ă��邾���Ȃ̂Ō�قǏ���
        Debug.Log("Farm : hp = " + _hp);
    }

    /// <summary>
    /// �_���[�W����
    /// </summary>
    /// <param name = "damage">�_���[�W�l</param>
    public void OnDamage(int damage)
    {
        _hp -= damage;

        // HP�������Ȃ�����
        if (_hp <= 0)
        {
            // �␳
            _hp = 0;
            // ���Ă��邱�Ƃ�
            _isBreak = true;
        }
    }

    /// <summary>
    /// �񕜏���
    /// </summary>
    /// <param name = "repairVal">�񕜗�</param>
    public void OnRepair(int repairVal)
    {
        // ���ɔj�󂳂�Ă�����񕜂��Ȃ�
        if (_isBreak) return;

        _hp += repairVal;

        // HP����𒴂��Ȃ��悤��
        _hp = Mathf.Min(_hp, kMaxHp);
    }

    /// <summary>
    /// ���݂�HP��Ԃ�
    /// </summary>
    /// <returns>���݂�HP</returns>
    public int NowHp()
    {
        return _hp;
    }

    /// <summary>
    /// �j�󂳂�Ă��邩
    /// </summary>
    /// <returns>true : �j�󂳂�Ă��� / false : �c���Ă���</returns>
    public bool IsRepair()
    {
        return _isBreak;
    }
}
