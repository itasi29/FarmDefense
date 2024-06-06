using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

/// <summary>
/// エネミーの親
/// </summary>
public class EnemyBase : MonoBehaviour
{
    /* 田代がやったこと */
    // 定数の追加
    // 変数の追加
    // プロパティの追加
    // 関数二つの追加
    // コーディング規約に則って変数名の変更
    // アクセス指定子の変更
    /* ここまで */

    [SerializeField] private int _hp;  //敵のHP

    [SerializeField] protected float _speed;  //敵のスピード

    [SerializeField] private int _attack;  //敵の攻撃力

    [SerializeField] private float _attackTime;  //敵の攻撃時間間隔

    [SerializeField] private bool _attackinterval;  //攻撃したかのフラグ

    [SerializeField] protected bool _isFindPlayer;  //プレイヤーを発見したかどうかのフラグ

    [SerializeField] protected GameObject target; //ターゲットのオブジェクト獲得

    [SerializeField] protected GameObject player; //Playerのオブジェクト獲得

    public Farm farm;  //農場のスクリプト呼び出し

    /* 田代が追加したところ */
    // 定数の追加
    private const int kDecreaseSpeed = 1;   // _deltaHpの減らすスピード

    // 変数の追加
    private int _maxHp;     // 最大HP(各キャラで違ってくるため変数として持っておく)
    private int _deltaHp;   // HPの赤表記部分
    private bool _isDelat;  // DeltaHPを減少しているか
    private bool _isExist;  // 生存しているか

    // プロパティの作成
    public int Hp { get { return _hp; } }
    public int DeltaHp { get { return _deltaHp; } }
    public int MaxHp { get { return _maxHp; } }
    public bool IsExist { get { return _isExist; } }
    /* ここまで */

    /// <summary>
    /// 更新処理
    /// </summary>
    public virtual void Update()
    {
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    public virtual void Init(Vector3 pos)
    {
        transform.position = pos;  //Enemyの初期位置初期化

        _hp = 0;
        _speed = 0;
        _attack = 0;
        _attackTime = 0;

        /* 田代が追加しところ */
        // TODO: 最大ＨＰを取得する関数は後々作るのでそれを使ってやる
        _deltaHp = _hp;
        _isDelat = false;
        _isExist = true;
        /* ここまで */

        _attackinterval = false;
        _isFindPlayer = false;
    }

    /// <summary>
    /// 物理挙動の更新処理
    /// </summary>
    public virtual void FixedUpdate()
    {
        Transform transform = this.transform; //オブジェクトを取得

        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, _speed * Time.deltaTime);  //ターゲットのオブジェクトに向かう

        ReduceDeltaHp();
    }

    /// <summary>
    /// 農場との当たり判定
    /// </summary>
    public virtual void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.name == "Farm") //Farmに当たったら攻撃
        {
            if(_attackinterval == false)    //フラグがfalseなら攻撃開始
            {
                farm.OnDamage(_attack);  //FarmのHpを減らす

                _attackinterval = true;  //1回だけ攻撃可能
            }
            else if(_attackinterval == true)  //フラグがtrueなら攻撃中止
            {
                _attackTime++;  //時間経過

                if(_attackTime >= 60.0f)  //時間がたてば
                {
                    _attackTime = 0;   //0秒に戻す

                    _attackinterval = false; //フラグをfalseに戻す
                }
            }

        }
        else if(collision.gameObject.name == "Player")  //Plyaerに当たったら攻撃
        {
            if (_attackinterval == false)    //フラグがfalseなら攻撃開始
            {
                //farm.m_farmHp -= m_enemyAttack; //当たってるときHPを減らす

                //Debug.Log(farm.m_farmHp -= (int)m_enemyAttack);

                _attackinterval = true;  //1回だけ攻撃可能
            }
            else if (_attackinterval == true)  //フラグがtrueなら攻撃中止
            {
                _attackTime++;  //時間経過

                if (_attackTime >= 60.0f)  //時間がたてば
                {
                    _attackTime = 0;   //0秒に戻す

                    _attackinterval = false; //フラグをfalseに戻す
                }
            }
        }
    }

    /// <summary>
    /// プレイヤーが攻撃したら
    /// </summary>
    /// <param name="collision"></param>
    //public virtual void OnCollisionEnter(Collider collision)
    //{
    //    //プレイヤーに攻撃されたらダメージを受ける
    //}

    /// <summary>
    /// プレイヤーが索敵範囲に入ったかどうか
    /// </summary>
    /// <param name="collision"></param>
    public virtual void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.name == "Player")  //Playerが索敵範囲に入ったらPlayerを追いかける
        {
            _isFindPlayer = true;  //m_playerをtrueにする

            Debug.Log("入った");
        }
    }

    /// <summary>
    /// プレイヤーが索敵範囲をでたかどうか
    /// </summary>
    /// <param name="collision"></param>
    public virtual void OnTriggerExit(Collider collision)
    {
        if(collision.gameObject.name == "Player")
        {
            _isFindPlayer = false; //m_playerをfalseにする

            Debug.Log("入った");

        }
    }



    /* 田代が追加したところ */
    /// <summary>
    /// ダメージ処理
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    public void OnDamage(int damage)
    {
        _hp -= damage;
        _isDelat = true;

        // HPが無くなったら死亡とする
        if (_hp <= 0)
        {
            _hp = 0;
            _isExist = false;
        }
    }

    /// <summary>
    /// _deltaHpを_hpまで減らす処理
    /// </summary>
    private void ReduceDeltaHp()
    {
        // 減少中でないなら終了
        if (!_isDelat) return;

        // 減少
        _deltaHp -= kDecreaseSpeed;
        // 現在のHP未満になったら終了
        if (_deltaHp < _hp)
        {
            _deltaHp = _hp;
            _isDelat = false;
        }
    }
    /* ここまで */
}
