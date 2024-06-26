using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//武器の処理作るよ
public abstract class Weapon : MonoBehaviour
{
    public enum WeaponStatus
    {
        kAtk,
        kRate,
        kSpeed,
        kRange = kSpeed,
        kStatusNum
    };


    //武器のステータスの情報をまとめる

    public int level;


    //レベルの情報を保存する配列
    protected int[] weaponLevel = new int[(int)WeaponStatus.kStatusNum];

    /// <summary>
    /// 武器のステータスのレベルを設定する
    /// </summary>
    /// <param name="status">どのステータスのレベルを変更するか</param>
    /// <param name="level">変更後のレベル</param>
    public void SetLevel(WeaponStatus status, int level)
    {
        weaponLevel[(int)status] = level;
    }
    /// <summary>
    /// 武器のステータスのレベルを取得する
    /// </summary>
    /// <param name="status">取得したいステータス</param>
    /// <returns></returns>
    public int GetLevel(WeaponStatus status)
    {
        return weaponLevel[(int)status];
    }

    public virtual void Update()
    {

    }



}
