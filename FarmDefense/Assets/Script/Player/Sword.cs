using UnityEngine;

public class Sword : MonoBehaviour
{
    // MEMO: ‰Ê‚½‚µ‚Ä”»’è‚Ì‚½‚ß‚¾‚¯‚ÌƒNƒ‰ƒX‚Í‚¢‚é‚Ì‚©...
    int _attack;

    public void SetStatus(int attack, float range)
    {
        _attack = attack;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("‚ ‚½‚Á‚½‚É‚å‚ñ");
            EnemyBase enemy = collision.gameObject.GetComponent<EnemyBase>();
            enemy.OnDamage(_attack);
        }
    }
}
