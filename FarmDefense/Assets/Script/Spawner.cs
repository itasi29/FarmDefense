using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    /* 変数 */
    // ステージ名
    [SerializeField] private string _stageName;
    // 現在のwave番号
    private int _nowWaveNo;
    // wave開始からの経過フレーム
    private int _elapsFrame;
    // 生成終了しているか
    private bool _isEndCreate;

    void Start()
    {
        
    }

    
    void FixedUpdate()
    {
        
    }
}
