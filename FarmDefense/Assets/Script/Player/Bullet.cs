using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private const int kLifeTime = 180;

    private int _attack;

    private Vector3 _velocity;
    private int _lifeTime;

    public void Init(int attack, Vector3 velocity)
    {
        _attack = attack;
        _velocity = velocity;
    }

    private void FixedUpdate()
    {
        this.transform.position += _velocity;
        ++_lifeTime;
        if (_lifeTime > kLifeTime)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            EnemyBase enemy = collision.gameObject.GetComponent<EnemyBase>();
            enemy.OnDamage(_attack);
            Destroy(this.gameObject);
            return;
        }

        // プレイヤー以外にあったたら消す
        if (collision.gameObject.tag != "Player")
        {
            Destroy(this.gameObject);
        }
    }
}
