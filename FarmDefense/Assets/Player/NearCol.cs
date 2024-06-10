using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearCol : MonoBehaviour
{
    private int _attack = 10;

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("あたったにょん");
    //    collision.gameObject.SetActive(false);
    //    Debug.Log(Time.time + "| checkCollision");
    //    if (collision.gameObject.tag == "Enemy")
    //    {
    //        Debug.Log("あたったにょん");
    //        EnemyBase enemy = collision.gameObject.GetComponent<EnemyBase>();
    //        enemy.OnDamage(_attack);
    //    }
    //}
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Debug.Log("あたったにょん");
            EnemyBase enemy = other.GetComponent<EnemyBase>();
            enemy.OnDamage(_attack);
        }
    }
}
