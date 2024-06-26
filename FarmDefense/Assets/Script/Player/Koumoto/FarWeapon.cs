using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarWeapon : Weapon
{
    [SerializeField] private GameObject _bullet;

    private void Start()
    {
        weaponLevel[(int)WeaponStatus.kAtk] = 1;
    }

    public override void Update()
    {

    }
    public void Attack(Vector3 dir)
    {
        GameObject bullet = Instantiate(_bullet, this.transform.position, Quaternion.identity);

        Bullet_Koumoto script = bullet.GetComponent<Bullet_Koumoto>();

        //TODO:UŒ‚—Í‚ğæ“¾‚Å‚«‚é‚æ‚¤‚É‚·‚é
        script.SetMoveVec(dir, 0.5f);

        Debug.Log("‰“‹——£UŒ‚");
    }

}
