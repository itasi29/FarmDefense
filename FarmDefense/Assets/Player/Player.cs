using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;

//プレイヤーの処理まとめるよ
public class Player : MonoBehaviour
{
    private const float kDownSpeed = 0.1f;//スタミナ切れ時の移動速度

    private const float kSpeed = 0.2f;//基本的な移動速度

    private const float kDashMaxSpeed = 0.5f;//ダッシュ時の移動速度

    private const float kDashMaxSpeedTime = 0.05f;//最高速度に達するまでの時間

    private const float kDashAddSpeed = (kDashMaxSpeed - kSpeed) * kDashMaxSpeedTime;

    private const float kJumpPower = 30.0f;//ジャンプ力

    private const int kStaminaMax = 500;//スタミナの最大値

    private const int kHpMax = 100;//体力の最大値

    private const int kHitStanTime = 30;//エネミーにぶつかったときのスタン時間

    private const int kHitSafeTime = 40;//エネミーにぶつかったときの無敵時間

    private const int kReviveSafeTime = 60;//エネミーにぶつかったときの無敵時間

    private const int kHeavyAttackNeedStamina = 100;//強攻撃時に必要になるスタミナ

    private Vector3 kInitPos = new Vector3(0, 0, 0);

    private Vector3 _dirVec;
    private Vector3 _moveVec;
    private Rigidbody _rigidBody;

    private float _speed;

    private int _stamina;

    private bool _isDash;

    private bool _isJump;

    private int _hp;

    private int _hitStanTime;

    private bool _isStan;

    private bool _isSafe;

    private int _safeTime;

    private bool _isUseNearWeapon;

    private bool _isTired;

    private NearWeapon _nearWeapon;
    private FarWeapon _farWeapon;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _stamina = kStaminaMax;
        _hp = kHpMax;
        _speed = kSpeed;

        _isJump = false;

        _isStan = false;

        _isTired = false;

        _isDash = false;

        _nearWeapon = null;

        _farWeapon = null;

        _isSafe = false;

        _isUseNearWeapon = true;

        //TODO:ほかのいい感じの処理に変えたい.取得はできてた
        GameObject weapon = GameObject.Find("scop");

        _nearWeapon = weapon.GetComponent<NearWeapon>();

        weapon = null;

        weapon = GameObject.Find("gun");

        _farWeapon = weapon.GetComponent<FarWeapon>();


    }

    void FixedUpdate()
    {
 //       Debug.Log(_stamina);
        //スタンしていないときの処理
        if (!_isStan)
        {
            this.transform.position += _moveVec;

            //ダッシュ時の処理
            if (_isDash)
            {
                _stamina--;
                //ダッシュの速度が最高速度じゃなかったら
                if (_speed < kDashMaxSpeed)
                {
                    //移動速度を徐々に上げていく
                    _speed += kDashAddSpeed;
                }
            }
            //スタミナが最大値よりも少なかったら
            if (_stamina < kStaminaMax && !_isDash)
            {
                //スタミナを回復する
                _stamina++;
            }
            //スタミナが最大値になったら
            if (_stamina >= kStaminaMax)
            {
                _stamina = kStaminaMax;
                _isTired = false;
            }
            //スタミナが0以下になったときの処理
            else if (_stamina < 0)
            {
                _stamina = 0;
                _isDash = false;
                _isTired = true;
            }

        }
        else
        {
            //硬直時間を減らす
            _hitStanTime--;

            if (_hitStanTime <= 0)
            {
                _isStan = false;
            }
        }
        //無敵時間があるとき
        if (_safeTime >= 0)
        {
            Debug.Log("むてきだよ");
            _isSafe = true;
            _safeTime--;
        }
        else
        {
            _isSafe = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

        //攻撃ボタンを押したとき
        if (Input.GetButtonDown("X"))
        {
            Debug.Log("おした");
            //近距離武器を持っているかどうか
            if (_isUseNearWeapon)
            {
                _nearWeapon.Attack();
            }
            else
            {
                _farWeapon.Attack(_dirVec);
            }
        }
        //強攻撃を使ったとき(ボタンわかんない)
        if (Input.GetButtonDown("Y") && !_isTired)
        {
            _stamina -= kHeavyAttackNeedStamina;
        }
        //武器切り替え
        if (Input.GetButtonDown("RB"))
        {
            _isUseNearWeapon = !_isUseNearWeapon;
        }
        //移動処理(仮)
        Move();
        if (!_isStan)
        {

            //ジャンプボタンを押したとき
            if (Input.GetButtonDown("A") && !_isJump)
            {
                Jump();
            }

            //アイテム使用処理(仮)
            if (Input.GetButtonDown("B"))
            {
                RecoveryHp(10);
                Debug.Log(_hp);
            }
        }
        //ダメージを食らったとき
        //if文はデバッグ用
        if (Input.GetButtonDown("StickPushRight") && !_isSafe)
        {
            OnDamage(10);
            Debug.Log(_hp);
        }
    }
    private void Move()
    {


        _moveVec = new Vector3(0, 0, 0);
        Vector3 dirVec = new Vector3(0, 0, 0);


        dirVec.x = Input.GetAxis("Horizontal");
        dirVec.z = Input.GetAxis("Vertical");

        dirVec.Normalize();

        if(dirVec.sqrMagnitude != 0)
        {
            _dirVec = dirVec;
        }

        //ダッシュボタンを押していて疲れ状態じゃなかったら
        if (Input.GetButton("X") && !_isTired)
        {
            //スタミナがあって移動入力がされていたら
            if (_stamina > 0 && dirVec != Vector3.zero)
            {
                //ダッシュフラグを立てる
                _isDash = true;
            }
        }
        //押していない場合
        else
        {
            //ダッシュフラグをなくす
            _isDash = false;
            //通常のスピード
            _speed = kSpeed;
        }
        //疲れ状態だった場合
        if (_isTired)
        {
            _speed = kDownSpeed;
        }
        _moveVec = dirVec * _speed;
    }

    private void Jump()
    {
        _rigidBody.velocity = new Vector3(0, 0, 0);
        _rigidBody.AddForce(new Vector3(0, kJumpPower, 0), ForceMode.Impulse);
        _isJump = true;
    }

    public void OnDamage(int damage)
    {
        //体力を減らす
        _hp -= damage;
        if (_hp < 0)
        {
            _hp = kHpMax;
            //TODO 死亡した時の処理を作成する
            this.transform.position = kInitPos;
            _safeTime = kReviveSafeTime;
        }

        //ヒット時の硬直処理
        _isStan = true;
        _hitStanTime = kHitStanTime;

        _safeTime = kHitSafeTime;
    }

    public void RecoveryHp(int heal)
    {
        _hp += heal;
        if (_hp > kHpMax)
        {
            _hp = kHpMax;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Debug.Log("あたった");
            _isJump = false;
        }
    }
}

