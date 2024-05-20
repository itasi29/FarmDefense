using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//プレイヤーの処理まとめるよ
public class Player : MonoBehaviour
{
    private const float kDownSpeed = 0.1f;//スタミナ切れ時の移動速度

    private const float kSpeed = 0.2f;//基本的な移動速度

    private const float kDashMaxSpeed = 0.5f;//ダッシュ時の移動速度

    private const float kDashMaxSpeedTime = 0.05f;//最高速度に達するまでの時間

    private const float kDashAddSpeed = (kDashMaxSpeed - kSpeed) * kDashMaxSpeedTime;

    private const float kJumpPower = 30.0f;//ジャンプ力

    private const int kStaminaMax = 1200;//スタミナの最大値

    private Vector3 _moveVec;
    private Rigidbody _rigidBody;

    private float _speed;

    private int _stamina;

    private bool _isDash;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _stamina = kStaminaMax;
        _speed = kSpeed;
    }

    void FixedUpdate()
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
        else if (_stamina < kStaminaMax)
        {
            //スタミナを回復する
            _stamina++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        //ジャンプボタンを押したとき
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }
    private void Move()
    {

        _moveVec = new Vector3(0, 0, 0);
        Vector3 dirVec = new Vector3(0, 0, 0);


        dirVec.x = Input.GetAxis("Horizontal");
        dirVec.z = Input.GetAxis("Vertical");

        dirVec.Normalize();

        //ダッシュボタンを押していたら
        if (Input.GetButton("Fire1"))
        {
            //スタミナがあって移動入力がされていたら
            if (_stamina > 0 && dirVec != Vector3.zero)
            {
                //ダッシュフラグを立てる
                _isDash = true;
            }
            //スタミナがない状態だったら
            else
            {
                //通常より少し遅いスピード
                _speed = kDownSpeed;
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
        _moveVec = dirVec * _speed;
        Debug.Log(_stamina);
    }

    private void Jump()
    {
        _rigidBody.velocity = new Vector3(0, 0, 0);
        _rigidBody.AddForce(new Vector3(0, kJumpPower, 0), ForceMode.Impulse);
    }

}

