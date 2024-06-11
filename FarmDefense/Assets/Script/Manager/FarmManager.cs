using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;


public class FarmManager : MonoBehaviour
{
    /* 定数 */
    public const int kFarmNum = 6; // 農場の数

    /* 変数 */
    private GameObject[] _farmList = new GameObject[kFarmNum];   // 農場のオブジェクト情報
    private Farm[] _farmScript = new Farm[kFarmNum];             // 農場のスクリプト情報
    private int _totalHp;
    private int _totalMaxHp;

    public int Hp { get { return _totalHp; } }
    public int MaxHp {  get { return _totalMaxHp; } }

    private void Start()
    {
        _totalHp = 0;

        GameObject parent = GameObject.Find("Farm");
        for (int i = 0; i < kFarmNum; ++i)
        {
            _farmList[i] = parent.transform.GetChild(i).gameObject;
            _farmScript[i] = _farmList[i].GetComponent<Farm>();
            _totalHp += _farmScript[i].Hp;
        }

        _totalMaxHp = _totalHp;
    }

    private void FixedUpdate()
    {
        // 総HPの更新
        _totalHp = 0;
        foreach (var farm in _farmScript)
        {
            _totalHp += farm.Hp;
        }

        // 総HPが0になったら終了
        if (_totalHp <= 0)
        {
            // TODO: ゲームオーバーとして、リザルト画面を呼ぶ
            return;
        }
    }

    /// <summary>
    /// 渡された場所から近い農場の座標を返す
    /// </summary>
    /// <param name="pos">自身の場所</param>
    /// <returns>近い農場の座標</returns>
    public Vector3 GetNearPos(Vector3 pos)
    {
        Vector3 nearPos = pos;
        bool isNeverIn = true;

        for (int i = 0; i < kFarmNum; ++i)
        {
            // 破壊されていたら次へ
            if (_farmScript[i].IsBreak) continue;

            // 一度も入れられていなければそのまま代入
            if (isNeverIn)
            {
                nearPos = _farmList[i].transform.position;
                isNeverIn = false;
            }
            // 入れられたことがあれば計算
            else
            {
                // 現在選んでいる農場までの距離
                float nowSqLength = (nearPos - pos).sqrMagnitude;
                // 確認する農場までの距離
                float checkSqLength = (_farmList[i].transform.position - pos).sqrMagnitude;
                // 現在保存されている農場までの距離より近ければ変更
                if (nowSqLength > checkSqLength)
                {
                    nearPos = _farmList[i].transform.position;
                }
            }
        }

        return nearPos;
    }

    /// <summary>
    /// 渡された場所から遠い農場の座標を返す
    /// </summary>
    /// <param name="pos">自身の場所</param>
    /// <returns>農場の座標</returns>
    public Vector3 GetFarPos(Vector3 pos)
    {
        Vector3 farPos = pos;
        bool isNeverIn = true;

        for (int i = 1; i < kFarmNum; ++i)
        {
            // 破壊されていたら次へ
            if (_farmScript[i].IsBreak) continue;

            // 一度も入れられていなければそのまま代入
            if (isNeverIn)
            {
                farPos = _farmList[i].transform.position;
                isNeverIn = false;
            }
            // 入れられたことがあれば計算
            else
            {
                // 現在選んでいる農場までの距離
                float nowSqLength = (farPos - pos).sqrMagnitude;
                // 確認する農場までの距離
                float checkSqLength = (_farmList[i].transform.position - pos).sqrMagnitude;
                // 現在保存されている農場までの距離より遠ければ変更
                if (nowSqLength < checkSqLength)
                {
                    farPos = _farmList[i].transform.position;
                }
            }
        }

        return farPos;
    }
}
