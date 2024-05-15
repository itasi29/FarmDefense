using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    /* 定数 */
    private const float kDistance = 2.2f;   // ターゲットとカメラとの距離
    private const float kShiftY   = 1.2f;   // ターゲット中心から上にずらす量

    /* 変数 */
    private GameObject target;  // ターゲットの情報
    private Vector3 pos;  // カメラの座標

    void Start()
    {
        // ターゲットの取得
        target = GameObject.Find("Player");

        // 初期位置の設定
        pos = new Vector3(0.0f, kShiftY, kDistance);
    }

    private void Update()
    {
        Vector3 vel = Vector3.zero; // 速度用変数



        if ()
        {

        }
    }

    private void FixedUpdate()
    {
        
    }
}
