using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Timeline;
using static UnityEditor.PlayerSettings;

public class CameraControl : MonoBehaviour
{
    /* 定数 */
    private const float kDistance = 2.2f;   // ターゲットとカメラとの距離
    private const float kShiftPosY   = 1.2f;   // ターゲット中心から上にずらす量
    private const float kAxisMinThershold = 0.2f; // 入力情報の最小のしきい値:無視する割合
    private const float kAxisMaxThershold = 0.8f; // 入力情報の最大のしきい値:1.0とみなす割合
    private const float kRotLimitUpdownSwing = 30.0f * Mathf.Deg2Rad;   // 上下の回転制限
    private const float kRotSpeedLeftright = 0.4f  * Mathf.Deg2Rad;  // 左右の回転スピード(ラジアン)
    private const float kRotSpeedUpdown    = 0.25f * Mathf.Deg2Rad;  // 上下の回転スピード(ラジアン)

    /* 変数 */
    private GameObject _target;         // ターゲットのオブジェクト情報
    private Transform _targetTrs;       // ターゲットのTransform情報
    private Vector3 _centerPos;         // 中心座標
    private Vector2 _frontDir;          // カメラの正面方向(Y軸は無視)
    private float _rotLeftrightSwing;   // 左右のカメラの回転量
    private float _rotUpdownSwing;      // 上下のカメラの回転量
    private bool _isUpdownSwing;        // 上下にカメラを揺らしているか
    private bool _isLeftrightSwing;     // 左右に入力したか
    private bool _isUpdownInput;        // 上下に入力したか
    private bool _isReset;              // リセットしたか

    private void Start()
    {
        // ターゲット(プレイヤー)から情報取得
        _target = GameObject.Find("Player");
        _targetTrs = _target.transform;

        /* 初期設定 */
        // 中心座標
        _centerPos = _targetTrs.position;
        // 正面方向
        _frontDir.y = 1;
        _frontDir.Normalize();
        // 回転量無しに
        _rotLeftrightSwing = 0.0f;
        _rotUpdownSwing = 0.0f;
        // 回転していないに
        _isUpdownSwing = false;
        _isLeftrightSwing = false;
        // 入力していないに
        _isUpdownInput = false;
        // リセットしていないに
        _isReset = false;
    }

    private void Update()
    {
        RotLeftright();
        RotUpdown();

        ResetDirection();
    }

    private void FixedUpdate()
    {
        ReturnRotUpdown();

        Move();

        Debug.Log("Camera : Dir = " + _frontDir);
    }

    /// <summary>
    /// カメラの正面方向を取得(Y軸は無視)
    /// </summary>
    /// <returns>正面方向</returns>
    public Vector2 GetFrontDir()
    {
        return _frontDir;
    }

    /// <summary>
    /// 左右の回転
    /// </summary>
    private void RotLeftright()
    {
        // 入力値の取得 
        float inputRate = Input.GetAxis("HorizontalRight");

        // 入力されていないなら終了
        if (-kAxisMinThershold < inputRate && inputRate < kAxisMinThershold)
        {
            _isLeftrightSwing = false;
            return;
        }

        inputRate = LimitValue(inputRate, kAxisMinThershold, kAxisMaxThershold);

        // 代入
        _rotLeftrightSwing += inputRate * kRotSpeedLeftright;
        // 入力していることに
        _isLeftrightSwing = true;
    }

    /// <summary>
    /// 上下の回転
    /// </summary>
    private void RotUpdown()
    {
        // 入力値の取得
        float inputRate = Input.GetAxis("VerticalRight");

        // 入力されていないなら終了
        if (-kAxisMinThershold < inputRate && inputRate < kAxisMinThershold)
        {
            _isUpdownInput = false;
            return;
        }

        inputRate = LimitValue(inputRate, kAxisMinThershold, kAxisMaxThershold);

        // 上下の回転地に代入
        _rotUpdownSwing += inputRate * kRotSpeedUpdown;
        // 回転の制限
        _rotUpdownSwing = Mathf.Max(Mathf.Min(_rotUpdownSwing, kRotLimitUpdownSwing), -kRotLimitUpdownSwing);
        // 動かしていることに
        _isUpdownSwing = true;
        // 入力したことに
        _isUpdownInput = true;
    }

    /// <summary>
    /// 向いている方向をプレイヤーの向いている方向に戻す
    /// </summary>
    private void ResetDirection()
    {
        // MEMO:ボタン間違えている可能性あり
        // Yボタンを押すと方向リセット
        if (Input.GetButtonDown("StickPushRight"))
        {
            // TODO:現状Leftrightの回転を0にしているだけなのでプレイヤーの向いている方向を向くように
            _rotLeftrightSwing = 0.0f;
            _rotUpdownSwing = 0.0f;
            _isLeftrightSwing = false;
            _isUpdownSwing = false;

            _isReset = true;
        }
    }

    /// <summary>
    /// 上下方向の回転量を元に戻す
    /// </summary>
    private void ReturnRotUpdown()
    {
        // 上下方向の入力をしていれば戻さない
        if (_isUpdownInput) return;

        // 0に近づける
        _rotUpdownSwing = Mathf.Lerp(_rotUpdownSwing, 0.0f, 0.05f);

        // 限りなく0に近づいたら戻したことにする
        if (-0.001f < _rotUpdownSwing && _rotUpdownSwing < 0.001f)
        {
            _rotUpdownSwing = 0.0f;
            _isUpdownSwing = false;
        }
    }

    /// <summary>
    /// 位置の更新
    /// </summary>
    private void Move()
    {
        // 中心を更新するか
        bool isUpdateCenter = (_centerPos != _targetTrs.position);
        // リセットしてないかつどこも更新がなければ変更をしない
        if (!_isReset && !_isLeftrightSwing && !_isUpdownSwing && !isUpdateCenter) return;

        // 中心位置の更新
        if (isUpdateCenter)
        {
            _centerPos = Vector3.Lerp(_centerPos, _targetTrs.position, 0.5f);

            // 限りなく0に近づいたらもう重なっていることに
            if ((_centerPos - _targetTrs.position).sqrMagnitude < 0.001f)
            {
                _centerPos = _targetTrs.position;
            }
        }

        Vector3 pos = _centerPos;

        // 左右の回転量
        float sinLeftright = Mathf.Sin(_rotLeftrightSwing);
        float cosLeftright = Mathf.Cos(_rotLeftrightSwing);

        // 上下回転のみor左右+上下回転
        if (_isUpdownSwing)
        {
            // 上下の回転量
            float sinUpdown = Mathf.Sin(_rotUpdownSwing);
            float cosUpdown = Mathf.Cos(_rotUpdownSwing);

            // 回転した位置の適用
            pos.x += sinLeftright * (kDistance * cosUpdown);
            pos.y += kShiftPosY + kDistance * sinUpdown * -1.0f;
            pos.z += cosLeftright * (kDistance * cosUpdown) * -1.0f;
        }
        // 左右回転のみ
        else
        {
            // 回転した位置の適用
            pos.x += sinLeftright * kDistance;
            pos.y += kShiftPosY;
            pos.z += cosLeftright * kDistance * -1.0f;
        }

        // 位置の代入 
        transform.position = pos;

        // リセットまたは回転していれば方向の更新
        if (_isReset || _isLeftrightSwing || _isUpdownSwing)
        {
            // 正面方向変更
            _frontDir.x = -pos.x;
            _frontDir.y = -pos.z;
            // 正規化
            _frontDir.Normalize();

            // オブジェクトの向き変更
            Vector3 lookPos = _centerPos;
            lookPos.y += kShiftPosY;
            transform.LookAt(lookPos);
        }

        // リセットしていないに
        _isReset = false;
    }


    /// <summary>
    /// 値を制限する関数
    /// </summary>
    /// <param name="val">制限したい値</param>
    /// <param name="min">最小値</param>
    /// <param name="max">最大値</param>
    /// <returns>制限した値</returns>
    private float LimitValue(float val, float min, float max)
    {
        if (val > 0.0f)
        {
            // 正の場合
            val = (val - min) / (max - min);
            val = Mathf.Min(1.0f, Mathf.Max(0.0f, val));
        }
        else
        {
            // 負の場合
            val = (val + min) / (max - min);
            val = Mathf.Min(0.0f, Mathf.Max(-1.0f, val));
        }

        return val;
    }
}
