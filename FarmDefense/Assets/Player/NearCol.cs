using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearCol : MonoBehaviour
{
    private int _attack = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Debug.Log("‚ ‚½‚Á‚½‚É‚å‚ñ");
            EnemyBase enemy = other.GetComponent<EnemyBase>();
            enemy.OnDamage(_attack);
        }
    }
}
