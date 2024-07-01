using UnityEngine;

public class Sword : MonoBehaviour
{
    // MEMO: �ʂ����Ĕ���̂��߂����̃N���X�͂���̂�...
    int _attack;

    public void SetStatus(int attack, float range)
    {
        _attack = attack;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("���������ɂ��");
            EnemyBase enemy = collision.gameObject.GetComponent<EnemyBase>();
            enemy.OnDamage(_attack);
        }
    }
}
