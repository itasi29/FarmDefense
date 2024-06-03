using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearWeapon : Weapon
{
    private const int kAttackTime = 60;

    private const int kHeavyAttackTime = 30;

    private const int kAttackRota = 180;

    private const int kHeavyAttackRota = 180;

    private bool _isAttack;

    private bool _isHeavy;

    private int _attackTime;

    private void Start()
    {
        statusInfo[(int)WeaponStatus.kAtk].point = 10;

        _isAttack = false;

        _isHeavy = false;

        _attackTime = 0;
    }

    public void FixedUpdate()
    {
            _attackTime++;
        if (_isAttack)
        {
            Debug.Log("UŒ‚");
            if (_isHeavy)
            {

                this.transform.rotation = Quaternion.AngleAxis(kHeavyAttackRota / kHeavyAttackTime * _attackTime,Vector3.up);
                if (_attackTime > kHeavyAttackTime)
                {
                    _isAttack = false;
                    _attackTime = 0;
                    this.transform.rotation = Quaternion.identity;
                }
            }
            else
            {
                this.transform.rotation = Quaternion.AngleAxis(kAttackRota / kAttackTime * _attackTime, new Vector3(this.transform.rotation.x, 0, 0));
                if (_attackTime > kAttackTime)
                {
                    _isAttack = false;
                    _attackTime = 0;
                    this.transform.rotation = Quaternion.identity;
                }
            }
        }
    }

    public override void Update()
    {

    }

    public void Attack()
    {
        _isAttack = true;
        _isHeavy = false;
        _attackTime = 0;
    }
    public void HeavyAttack()
    {
        this.transform.rotation = Quaternion.AngleAxis(90, new Vector3(this.transform.rotation.x, 0, 0));
        _isAttack = true;
        _isHeavy = true;
        _attackTime = 0;
    }


}
