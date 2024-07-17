using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class User
{
    public int money;   // 所持金お金
    public int time;    // プレイ時間
    public Dictionary<string, int> weapon;    // 武器のレベル
    public Dictionary<string, List<int>> item;        // アイテム所持数
    public Dictionary<int, bool> clearInfo; // クリア情報
}

public class UserData
{
    // ユーザデータ保持数
    private const int kUserNum = 4;

    // 各ユーザデータの情報
    private Dictionary<int, User> _data = new Dictionary<int, User>();
    // 共通ユーザデータ
    private int _bgmVolLv;
    private int _seVolLv;
    // 現在のユーザ番号
    private int _nowUserNo;

    /// <summary>
    /// 読み込み
    /// </summary>
    public void Load(List<string> weaponIdList, List<string> itemIdList)
    {
        try
        {
            // ファイルを開く
            // using(){}すると勝手にCloseしてくれる
            using (var reader = new BinaryReader(new FileStream(Application.dataPath + DataManager.kUserFileName, FileMode.Open)))
            {
                // ファイルが存在する場合
                _bgmVolLv = reader.ReadInt32();
                _seVolLv = reader.ReadInt32();

                for (int i = 0; i < kUserNum; ++i)
                {
                    User user = new User();
                    // 所持金読み込み
                    user.money = reader.ReadInt32();
                    // プレイ時間読み込み
                    user.time = reader.ReadInt32();
                    // 武器のレベル読み込み
                    Dictionary<string, int> weapon = new Dictionary<string, int>();
                    for (int j = 0; j < weaponIdList.Count; ++j)
                    {
                        weapon.Add(weaponIdList[j], reader.ReadInt32());
                    }
                    user.weapon = weapon;
                    // 各種アイテム所持数読み込み
                    Dictionary<string, List<int>> item = new Dictionary<string, List<int>>();
                    for (int j = 0; j < itemIdList.Count; ++j)
                    {
                        List<int> lv = new List<int>();
                        for (int k  = 0; k < 3; ++k)
                        {
                            lv.Add(reader.ReadInt32());
                        }
                        item.Add(itemIdList[j], lv);
                    }
                    user.item = item;
                    // クリアデータ読み込み
                    Dictionary<int, bool> clearInfo = new Dictionary<int, bool>();
                    for (int j = 0; j < 6; ++j)
                    {
                        clearInfo.Add(j, reader.ReadBoolean());
                    }
                    user.clearInfo = clearInfo;

                    // データ挿入
                    _data.Add(i, user);
                }
            }
        }
        catch
        {
            // ファイルが存在しない場合
            _bgmVolLv = SoundManager.kVolumeLvMax;
            _seVolLv = SoundManager.kVolumeLvMax;

            for (int i = 0; i < kUserNum; ++i)
            {
                User user = new User();
                // 所持金
                user.money = 100000;
                // プレイ時間
                user.time = 0;
                // 武器のレベル
                Dictionary<string, int> weapon = new Dictionary<string, int>();
                for (int j = 0; j < weaponIdList.Count; ++j)
                {
                    weapon.Add(weaponIdList[j], 0);
                }
                user.weapon = weapon;
                // アイテム所持数
                Dictionary<string, List<int>> item = new Dictionary<string, List<int>>();
                for (int j = 0; j < itemIdList.Count; ++j)
                {
                    List<int> lv = new List<int>();
                    for (int k = 0; k < 3; ++k)
                    {
                        lv.Add(0);
                    }
                    item.Add(itemIdList[j], lv);
                }
                user.item = item;
                // クリアデータ読み込み
                Dictionary<int, bool> clearInfo = new Dictionary<int, bool>();
                for (int j = 0; j < 6; ++j)
                {
                    if (j == 0)
                    {
                        clearInfo.Add(j, true);
                    }
                    else
                    {
                        clearInfo.Add(j, false);
                    }
                }
                user.clearInfo = clearInfo;

                // データ挿入
                _data.Add(i, user);
            }
        }

#if false
        // データの中身見る用
        int x = 0;
        foreach (var user in _data)
        {
            ++x;
            Debug.Log(x + "回目： moeny = " + user.Value.money + ", time = " + user.Value.time);
            foreach (var weapon in user.Value.weapon)
            {
                Debug.Log(weapon.Key + ": " + weapon.Value);
            }
            foreach (var item in user.Value.item)
            {
                Debug.Log(x + "回目： " + item.Key + " = " + item.Value);
            }
        }
#endif
    }

    /// <summary>
    /// 保存
    /// </summary>
    public void Save()
    {
        try
        {
            // ファイルを開く(ない場合は作成)
            using (var writer = new BinaryWriter(new FileStream(Application.dataPath + DataManager.kUserFileName, FileMode.OpenOrCreate)))
            {
                writer.Write((Int32)_bgmVolLv);
                writer.Write((Int32)_seVolLv);

                foreach (var user in _data)
                {
                    // 所持金書き込み
                    writer.Write((Int32)user.Value.money);
                    // プレイ時間書き込み
                    writer.Write((Int32)user.Value.time);
                    // 武器のレベル書き込み
                    foreach (var weapon in user.Value.weapon)
                    {
                        writer.Write((Int32)weapon.Value);
                    }
                    // アイテム所持数書き込み
                    foreach (var item in user.Value.item)
                    {
                        foreach (var lv in item.Value)
                        {
                            writer.Write((Int32)lv);
                        }
                    }
                    // クリアデータ書き込み
                    foreach (var clear in user.Value.clearInfo)
                    {
                        writer.Write((Boolean)clear.Value);
                    }
                }
            }
        }
        catch
        {
            Debug.Log("何でぇ");
        }
    }

    // MEMO: プロパティがいいか下のままがいいか…
    public int GetBgmVolLv()
    {
        return _bgmVolLv;
    }
    public int GetSeVolLv() 
    {
        return _seVolLv;
    }
    public void SetUserNo(int userNo)
    {
        _nowUserNo = userNo;
    }
    public bool IsStageClear(int no)
    {
        return _data[_nowUserNo].clearInfo[no];
    }
    public void ChangeStageClear(int no)
    {
        _data[_nowUserNo].clearInfo[no] = true;
    }
    /// <summary>
    /// 現在所持しているお金
    /// </summary>
    /// <param name="userNo">ユーザ番号 def:_nowUserNo</param>
    /// <returns>所持金</returns>
    public int GetMoney(int userNo = -1)
    {
        if (userNo < 0)
        {
            userNo = _nowUserNo;
        }
        return _data[userNo].money;
    }
    /// <summary>
    /// 現在のプレイ時間
    /// </summary>
    /// <param name="userNo">ユーザ番号 def:_nowUserNo</param>
    /// <returns>プレイ時間</returns>
    public int GetTime(int userNo = -1) 
    {
        if (userNo < 0)
        {
            userNo = _nowUserNo;
        }
        return _data[userNo].time;
    }
    /// <summary>
    /// 現在の武器のレベル
    /// </summary>
    /// <param name="id">武器のID</param>
    /// <returns>レベル</returns>
    public int GetWeaponLv(string id)
    {
        return _data[_nowUserNo].weapon[id];
    }
    /// <summary>
    /// 現在所持しているアイテムの数
    /// </summary>
    /// <param name="id">アイテムのID</param>
    /// <param name="lv">アイテムのレベル</param>
    /// <returns>所持数</returns>
    public int GetHasItemNum(string id, int lv)
    {
        return _data[_nowUserNo].item[id][lv];
    }
    public void SetBgmVolLv(int lv)
    {
        _bgmVolLv = lv;
    }
    public void SetSeVolLv(int lv)
    {
        _seVolLv = lv;
    }
    /// <summary>
    /// 所持金を増やす
    /// </summary>
    /// <param name="add">増やすお金の量</param>
    public void AddMoney(int add)
    {
        _data[_nowUserNo].money += add;
    }
    /// <summary>
    /// 減らす金額で0未満にならない場合は所持金を減らす
    /// </summary>
    /// <param name="sub">減らすお金の量</param>
    /// <returns>true: 減らせる / false: 減らせない</returns>
    public bool SubMoney(int sub)
    {
        int temp = _data[_nowUserNo].money;
        temp -= sub;
        if (temp < 0)
        {
            return false;
        }
        else
        {
            _data[_nowUserNo].money = temp;
            return true;
        }
    }
    /// <summary>
    /// プレイ時間の増加
    /// </summary>
    public void AddTime()
    {
        ++_data[_nowUserNo].time;
    }
    /// <summary>
    /// 武器のレベルを上げる
    /// </summary>
    /// <param name="id">武器のID</param>
    public void LvUpWeapon(string id)
    {
        ++_data[_nowUserNo].weapon[id];
    }
    /// <summary>
    /// 所持アイテム数を増やす
    /// </summary>
    /// <param name="id">アイテムのID</param>
    /// <param name="lv">アイテムのレベル</param>
    public void AddHasItemNum(string id, int lv)
    {
        ++_data[_nowUserNo].item[id][lv];
    }
    /// <summary>
    /// 所持している場合アイテムを使用する
    /// </summary>
    /// <param name="id">アイテムのID</param>
    /// <param name="lv">アイテムのレベル</param>
    /// <returns>true: 使用可能 / false: 使用不可能</returns>
    public bool UseItem(string id, int lv)
    {
        int temp = _data[_nowUserNo].item[id][lv];
        --temp;
        if (temp < 0)
        {
            return false;
        }
        else
        {
            _data[_nowUserNo].item[id][lv] = temp;
            return true;
        }
    }
}
