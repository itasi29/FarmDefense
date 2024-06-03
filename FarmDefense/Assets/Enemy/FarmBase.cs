using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmBase : MonoBehaviour
{
    [SerializeField] EnemyBase enemy;  //enemyの宣言

    /* 定数 */
    private const int kMaxHp = 100;    // 最大HP
    private const int kDecreaseSpeed = 1;

    /* 変数 */
    [SerializeField] private int _hp;        // 現在のHP
    [SerializeField] private int _deltaHp;   // 減少HP
    private bool _isBreak;  // 壊れているか
    private bool _isDelta;  // 現在のHPと減少HPの差があるか

    /* プロパティ */
    public int Hp { get { return _hp; } }
    public int DeltaHp { get { return _deltaHp; } }
    public int MaxHp { get { return kMaxHp; } }
    public bool IsBreak { get { return _isBreak; } }


    private void Start()
    {
        /* 初期化 */
        _hp = kMaxHp;
        _deltaHp = kMaxHp;
        _isBreak = false;
        _isDelta = false;
    }

    private void FixedUpdate()
    {
        if (_isDelta)
        {
            _deltaHp -= kDecreaseSpeed;

            // 減少HPが現在のHPまで減ったら終了
            if (_deltaHp < _hp)
            {
                _deltaHp = _hp;
                _isDelta = false;
            }
        }
    }

    /// <summary>
    /// ダメージ処理
    /// </summary>
    /// <param name = "damage">ダメージ値</param>
    public void OnDamage(int damage)
    {
        _hp -= damage;
        _isDelta = true;

        // HPが無くなったら
        if (_hp <= 0)
        {
            // 補正
            _hp = 0;
            // 壊れていることに
            _isBreak = true;
        }
    }

    /// <summary>
    /// 回復処理
    /// </summary>
    /// <param name = "repairVal">回復量</param>
    public void OnRepair(int repairVal)
    {
        // 既に破壊されていたら回復しない
        if (_isBreak) return;

        _hp += repairVal;

        // HP上限を超えないように
        _hp = Mathf.Min(_hp, kMaxHp);
    }

    /// <summary>
    /// プレイヤーが索敵範囲に入ったら
    /// </summary>
    /// <param name="collision"></param>
    public void OnTriggerEnter(Collider collision)
    {
        enemy.OnTriggerEnter(collision);
    }

    /// <summary>
    /// プレイヤーが索敵範囲を出たら
    /// </summary>
    /// <param name="collision"></param>
    public void OnTriggerExit(Collider collision)
    {
        enemy.OnTriggerExit(collision);
    }
}
