using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmManager : MonoBehaviour
{
    /* 定数 */
    private const int kMaxHp = 100;    // 最大HP

    /* 変数 */
    private int _hp;        // HP
    private bool _isBreak;  // 壊れているか

    void Start()
    {
        /* 初期化 */
        _hp = kMaxHp;
        _isBreak = false;
    }

    private void FixedUpdate()
    {
        // MEMO : Debug用で出しているだけなので後ほど消す
        Debug.Log("Farm : hp = " + _hp);
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

    /// <summary>
    /// 現在のHPを返す
    /// </summary>
    /// <returns>現在のHP</returns>
    public int NowHp()
    {
        return _hp;
    }

    /// <summary>
    /// 破壊されているか
    /// </summary>
    /// <returns>true : 破壊されている / false : 残っている</returns>
    public bool IsRepair()
    {
        return _isBreak;
    }
}
