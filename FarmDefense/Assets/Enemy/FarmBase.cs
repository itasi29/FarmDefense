using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmBase : MonoBehaviour
{
    [SerializeField] EnemyBase enemy;  //enemy�̐錾

    /* �萔 */
    private const int kMaxHp = 100;    // �ő�HP
    private const int kDecreaseSpeed = 1;

    /* �ϐ� */
    [SerializeField] private int _hp;        // ���݂�HP
    [SerializeField] private int _deltaHp;   // ����HP
    private bool _isBreak;  // ���Ă��邩
    private bool _isDelta;  // ���݂�HP�ƌ���HP�̍������邩

    /* �v���p�e�B */
    public int Hp { get { return _hp; } }
    public int DeltaHp { get { return _deltaHp; } }
    public int MaxHp { get { return kMaxHp; } }
    public bool IsBreak { get { return _isBreak; } }


    private void Start()
    {
        /* ������ */
        _hp = kMaxHp;
        _deltaHp = kMaxHp;
        _isBreak = false;
        _isDelta = false;
    }

    private void FixedUpdate()
    {
        if (_isDelta)
        {
            _deltaHp -= kDecreaseSpeed;

            // ����HP�����݂�HP�܂Ō�������I��
            if (_deltaHp < _hp)
            {
                _deltaHp = _hp;
                _isDelta = false;
            }
        }
    }

    /// <summary>
    /// �_���[�W����
    /// </summary>
    /// <param name = "damage">�_���[�W�l</param>
    public void OnDamage(int damage)
    {
        _hp -= damage;
        _isDelta = true;

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
    /// �v���C���[�����G�͈͂ɓ�������
    /// </summary>
    /// <param name="collision"></param>
    public void OnTriggerEnter(Collider collision)
    {
        enemy.OnTriggerEnter(collision);
    }

    /// <summary>
    /// �v���C���[�����G�͈͂��o����
    /// </summary>
    /// <param name="collision"></param>
    public void OnTriggerExit(Collider collision)
    {
        enemy.OnTriggerExit(collision);
    }
}
