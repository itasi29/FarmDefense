using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Koumoto : MonoBehaviour
{
    private const int kLifeTime = 180;

    private float _spd;

    private int _atk = 10;

    private Vector3 _moveVec;

    private int _lifeTime = 0;

    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.position += _moveVec;
        _lifeTime++;
        if(_lifeTime > kLifeTime)
        {
            Destroy(this.gameObject);
        }
    }

    public void SetMoveVec(Vector3 dirVec, float speed)
    {
        _moveVec = dirVec * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            EnemyBase enemy = collision.gameObject.GetComponent<EnemyBase>();
            enemy.OnDamage(_atk);
        }

        // プレイヤー以外にあったたら消す
        if (!(collision.gameObject.tag == "Player")) 
        {
            Destroy(this.gameObject);
        }
    }
}
