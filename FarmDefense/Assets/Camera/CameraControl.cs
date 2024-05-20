using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    /* 定数 */
    private const float kDistance = 2.2f;   // ターゲットとカメラとの距離
    private const float kShiftPosY   = 1.2f;   // ターゲット中心から上にずらす量
    private const float kAxisMinThershold = 0.2f; // 入力情報の最小のしきい値:無視する割合
    private const float kAxisMaxThershold = 0.8f; // 入力情報の最大のしきい値:1.0とみなす割合
    private const float kSpeedRotation = 0.25f * Mathf.Deg2Rad;  // 回転スピード(ラジアン)

    /* 変数 */
    private GameObject _target;     // ターゲットのオブジェクト情報
    private Transform _targetTrs;   // ターゲットのTransform情報
    private Vector3 _pos;           // カメラの中心座標
    private float _rot;

    void Start()
    {
        // ターゲット(プレイヤー)から情報取得
        _target = GameObject.Find("Player");
        _targetTrs = _target.transform;

        /* 初期設定 */
        _pos = _targetTrs.position;   // 座標
        _rot = 0.0f;
    }

    private void Update()
    {
        /* 回転 */
        // 入力値の取得 
        float inputRate = Input.GetAxis("HorizontalRight");  

        // 入力値の制限
        if (inputRate > 0.0f)
        {
            // 正の場合
            inputRate = (inputRate - kAxisMinThershold) / (kAxisMaxThershold - kAxisMinThershold);
            inputRate = Mathf.Min(1.0f, Mathf.Max(0.0f, inputRate));
        }
        else
        {
            // 負の場合
            inputRate = (inputRate + kAxisMinThershold) / (kAxisMaxThershold - kAxisMinThershold);
            inputRate = Mathf.Min(0.0f, Mathf.Max(-1.0f, inputRate));
        }

        // 代入
        _rot += inputRate * kSpeedRotation;
    }

    private void FixedUpdate()
    {
        /* 位置更新 */
        _pos = Vector3.Lerp(_pos, _targetTrs.position, 0.25f);

        /* 距離の反映 */
        Vector3 pos = _pos;
        pos.x += Mathf.Sin(_rot) * kDistance;
        pos.y += kShiftPosY;
        pos.z += Mathf.Cos(_rot) * kDistance * -1.0f;

        /* 位置の代入 */
        transform.position = pos;
    }
}
