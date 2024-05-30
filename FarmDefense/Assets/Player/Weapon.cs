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
        kSpeed,
        kRange,
        kStatusNum
    };


    //武器のステータスの情報をまとめる
    protected struct StatusInfo
    {
        public int point;//ステータスの値

        public int level;
    }

    //ステータスの情報を保存する配列
    protected StatusInfo[] statusInfo = new StatusInfo[(int)WeaponStatus.kStatusNum];

    /// <summary>
    /// 武器のステータスのレベルを設定する
    /// </summary>
    /// <param name="status">どのステータスのレベルを変更するか</param>
    /// <param name="level">変更後のレベル</param>
    public void SetLevel(WeaponStatus status,int level)
    {
        statusInfo[(int)status].level = level;
        statusInfo[(int)status].point = level;//TODO 外部ファイル化して値を変化できるようにする
    }
    /// <summary>
    /// 武器のステータスのレベルを取得する
    /// </summary>
    /// <param name="status">取得したいステータス</param>
    /// <returns></returns>
    public int GetLevel(WeaponStatus status)
    {
        return statusInfo[(int)status].level;
    }
    public int GetStatusPoint(WeaponStatus status)
    {
        return statusInfo[(int)status].point;
    }
    public virtual void Update()
    {

    }



}
