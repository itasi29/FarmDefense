using UnityEngine;

public class Sword : MonoBehaviour
{
    // MEMO: ‰Ê‚½‚µ‚Ä”»’è‚Ì‚½‚ß‚¾‚¯‚ÌƒNƒ‰ƒX‚Í‚¢‚é‚Ì‚©...
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
            Debug.Log("‚ ‚½‚Á‚½‚É‚å‚ñ");
            EnemyBase enemy = collider.gameObject.GetComponent<EnemyBase>();
            enemy.OnDamage(_attack);
        }
    }
}
