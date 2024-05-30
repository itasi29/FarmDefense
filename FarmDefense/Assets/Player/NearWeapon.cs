using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearWeapon : Weapon 
{

    private void Start()
    {
        statusInfo[(int)WeaponStatus.kAtk].point = 10;
    }

    public override void Update()
    {
        
    }

    public void Attack()
    {
        Debug.Log("‹ß‹——£UŒ‚");
    }
    public void HeavyAttack()
    {
        Debug.Log("‹­UŒ‚");
    }


}
