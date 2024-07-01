using UnityEngine;

public class Sword : MonoBehaviour
{
    // MEMO: 果たして判定のためだけのクラスはいるのか...
    int _attack;

    public void SetStatus(int attack, float range)
    {
        _attack = attack;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("あたったにょん");
            EnemyBase enemy = collision.gameObject.GetComponent<EnemyBase>();
            enemy.OnDamage(_attack);
        }
    }
}
