using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : MonoBehaviour 
{
    /* 定数 */
    private const int kMaxHp = 100;    // 最大HP

    /* 変数 */
    private int _hp;        // HP
    private bool _isBreak;  // 壊れているか

    /* プロパティ */
    public int Hp { get { return _hp; } }
    public int MaxHp {  get { return kMaxHp; } }
    public bool IsBreak { get { return _isBreak;} }


    private void Start()
    {
        /* 初期化 */
        _hp = kMaxHp;
        _isBreak = false;
    }

    /// <summary>
    /// ダメージ処理
    /// </summary>
    /// <param name = "damage">ダメージ値</param>
    public void OnDamage(int damage)
    {
        _hp -= damage;

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
}
