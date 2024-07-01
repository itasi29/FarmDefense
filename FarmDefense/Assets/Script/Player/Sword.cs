using UnityEngine;

public class Sword : MonoBehaviour
{
    // MEMO: �ʂ����Ĕ���̂��߂����̃N���X�͂���̂�...
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
            Debug.Log("���������ɂ��");
            EnemyBase enemy = collider.gameObject.GetComponent<EnemyBase>();
            enemy.OnDamage(_attack);
        }
    }
}
