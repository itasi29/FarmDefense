using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Farm : MonoBehaviour 
{
    /* 定数 */
    private const int kMaxHp = 100;    // 最大HP
    private const int kDecreaseSpeed = 1;

    /* 変数 */
    [SerializeField] private int _hp;        // 現在のHP
    [SerializeField] private int _deltaHp;   // 減少HP
    [SerializeField] private bool _isBreak;  // 壊れているか
    [SerializeField] private bool _isDelta;  // 現在のHPと減少HPの差があるか
    [SerializeField] private bool _isInPlayer;

    /* プロパティ */
    public int Hp { get { return _hp; } }
    public int DeltaHp { get { return _deltaHp; } }
    public int MaxHp {  get { return kMaxHp; } }
    public bool IsBreak { get { return _isBreak;} }
    public bool IsInPlayer { get { return _isInPlayer; } }


    private void Start()
    {
        /* 初期化 */
        _hp = kMaxHp;
        _deltaHp = kMaxHp;
        _isBreak = false;
        _isDelta = false;
        _isInPlayer = false;
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


    /* FarmBaseを消す代わりに追加した部分 */
    // 渕本への質問：なぜわざわざこっちからUnityの機能読んでんの？
    //              　ここら辺の作り今度聞きます

    /// <summary>
    /// プレイヤーが索敵範囲に入ったら
    /// </summary>
    /// <param name="collision"></param>
    public void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            _isInPlayer = true;
        }
    }

    /// <summary>
    /// プレイヤーが索敵範囲を出たら
    /// </summary>
    /// <param name="collision"></param>
    public void OnTriggerExit(Collider collision)
    {
        if (collision.tag == "Player")
        {
            _isInPlayer = false;
        }
    }
}
