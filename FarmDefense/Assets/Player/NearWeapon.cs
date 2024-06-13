using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NearWeapon : Weapon
{
    private const int kAttackTime = 60;

    private bool _isAttack;

    private int _attackTime;

    private GameObject _weaponCol;

    private SphereCollider _sphereCollider;


    private void Start()
    {
        weaponLevel[(int)WeaponStatus.kAtk] = 1;

        _isAttack = false;

        _attackTime = 0;

        _weaponCol = GameObject.Find("weaponCol");

        _sphereCollider = _weaponCol.GetComponent<SphereCollider>();
    }

    public void FixedUpdate()
    {
        _attackTime++;
        if (_isAttack)
        {
            Debug.Log("UŒ‚");
            _weaponCol.SetActive(true);
        }
        else
        {
            _weaponCol.SetActive(false);
        }
        if (_attackTime > kAttackTime)
        {
            _isAttack = false;
            _attackTime = 0;
        }
    }

    public override void Update()
    {

    }

    public void Attack()
    {
        _isAttack = true;
        _attackTime = 0;
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("‚ ‚½‚Á‚½‚É‚å‚ñ");
    //    collision.gameObject.SetActive(false);
    //    Debug.Log(Time.time + "| checkCollision");
    //    if (collision.gameObject.tag == "Enemy")
    //    {
    //        Debug.Log("‚ ‚½‚Á‚½‚É‚å‚ñ");
    //        EnemyBase enemy = collision.gameObject.GetComponent<EnemyBase>();
    //        enemy.OnDamage(_attack);
    //    }
    //}
    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log(Time.time + "| checkTrigger");
    //    if (other.tag == "Enemy")
    //    {
    //        Debug.Log("‚ ‚½‚Á‚½‚É‚å‚ñ");
    //        EnemyBase enemy = other.GetComponent<EnemyBase>();
    //        enemy.OnDamage(_attack);
    //    }
    //}
}
