using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class NearWeapon : Weapon
{
    private const int kAttackTime = 60;


    private bool _isAttack;

    private int _attackTime;

    private List<Collider> _attackCollisions;

    private void Start()
    {
        statusInfo[(int)WeaponStatus.kAtk].point = 10;

        _isAttack = false;

        _attackTime = 0;


        _attackCollisions = new List<Collider>();

    }

    public void FixedUpdate()
    {
        _attackTime++;
        if (_attackTime > kAttackTime)
        {

            _isAttack = false;
            _attackTime = 0;
            this.gameObject.GetComponent<Renderer>().material.color = Color.white;
        }
    }

    public override void Update()
    {

    }

    public void Attack()
    {
        this.gameObject.GetComponent<Renderer>().material.color = Color.black;
        _isAttack = true;
        Debug.Log("��������������������");
        _attackTime = 0;
        _attackCollisions.Clear();
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("��������������������������");
        //�U�������Ă��Ȃ������牺�L�̏������s��Ȃ�
        if (!_isAttack) return;

        foreach (var enemy in _attackCollisions)
        {
            if (enemy == other)
            {
                return;
            }
        }

        if (_isAttack)
        {
            //TODO::enemy�̃X�N���v�g�������Ă��ă_���[�W��^����
            other.gameObject.SetActive(false);

            Debug.Log("���������ɂ傎");

            _attackCollisions.Add(other);
        }
    }

}
