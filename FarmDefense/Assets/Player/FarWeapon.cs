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

        Bullet script = bullet.GetComponent<Bullet>();

        //TODO:攻撃力を取得できるようにする
        script.SetMoveVec(dir, 0.5f);

        Debug.Log("遠距離攻撃");
    }

}
