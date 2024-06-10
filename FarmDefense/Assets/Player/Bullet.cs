using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
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
        // collision.gameObject.SetActive(false);

        //TODO:�^�O�Ŕ���
        //GameObject hitEnemy = collision.gameObject;

        //Destroy(this.gameObject);
        Debug.Log("���������ɂ��collision");
        if (collision.gameObject.tag == "Enemy")
        {
            EnemyBase enemy = collision.gameObject.GetComponent<EnemyBase>();
            enemy.OnDamage(_atk);
        }

//        Destroy(this.gameObject);
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("���������ɂ��trigger");
    //    if (other.gameObject.tag == "Enemy")
    //    {
    //        Debug.Log("���������ɂ��");
    //        EnemyBase enemy = other.gameObject.GetComponent<EnemyBase>();
    //        enemy.OnDamage(_atk);

    //        Destroy(this.gameObject);
    //    }

    //}
}
