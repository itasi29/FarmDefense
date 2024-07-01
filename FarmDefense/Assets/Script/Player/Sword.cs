using UnityEngine;

public class Sword : MonoBehaviour
{
    // MEMO: 果たして判定のためだけのクラスはいるのか...
    int _attack;
    bool _isCanAttack;

    public void OnAttack(int attack)
    {
        _attack = attack;
        _isCanAttack = true;
    }

    public void OffAttack()
    {
        _isCanAttack = false;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!_isCanAttack) return;

        if (collider.gameObject.tag == "Enemy")
        {
            Debug.Log("あたったにょん");
            EnemyBase enemy = collider.gameObject.GetComponent<EnemyBase>();
            enemy.OnDamage(_attack);
        }
    }
}
