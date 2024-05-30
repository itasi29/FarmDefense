using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarWeapon : Weapon
{
    private void Start()
    {
        statusInfo[(int)WeaponStatus.kAtk].point = 5;
    }

    public override void Update()
    {

    }
    public void Attack()
    {
        Debug.Log("‰“‹——£UŒ‚");
    }

}
