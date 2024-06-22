using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class User
{
    public int money;   // 所持金お金
    public int time;    // プレイ時間
    public Dictionary<string, int> weapon;    // 武器のレベル
    public Dictionary<string, int> item;        // アイテム所持数
}

public class UserData
{
    // ユーザデータ保持数
    private const int kUserNum = 4;

    // 各ユーザデータの情報
    private Dictionary<int, User> _data = new Dictionary<int, User>();

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
                    Dictionary<string, int> item = new Dictionary<string, int>();
                    for (int j = 0; j < itemIdList.Count; ++j)
                    {
                        item.Add(itemIdList[j], reader.ReadInt32());
                    }
                    user.item = item;

                    // データ挿入
                    _data.Add(i, user);
                }
            }
        }
        catch
        {
            // ファイルが存在しない場合
            for (int i = 0; i < kUserNum; ++i)
            {
                User user = new User();
                // 所持金
                user.money = 0;
                // プレイ時間
                user.time = 0;
                // 武器のレベル
                Dictionary<string, int> weapon = new Dictionary<string, int>();
                for (int j = 0; j < weaponIdList.Count; ++j)
                {
                    weapon.Add(weaponIdList[j], 1);
                }
                user.weapon = weapon;
                // アイテム所持数
                Dictionary<string, int> item = new Dictionary<string, int>();
                for (int j = 0; j < itemIdList.Count; ++j)
                {
                    item.Add(weaponIdList[j], 0);
                }
                user.item = item;

                // データ挿入
                _data.Add(i, user);
            }
        }

#if true
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
                        writer.Write((Int32)item.Value);
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
    /// <summary>
    /// 現在所持しているお金
    /// </summary>
    /// <param name="userNo">ユーザ番号</param>
    /// <returns>所持金</returns>
    public int GetMoney(int userNo)
    {
        return _data[userNo].money;
    }
    /// <summary>
    /// 現在のプレイ時間
    /// </summary>
    /// <param name="userNo">ユーザ番号</param>
    /// <returns>プレイ時間</returns>
    public int GetTime(int userNo) 
    {
        return _data[userNo].time;
    }
    /// <summary>
    /// 現在の武器のレベル
    /// </summary>
    /// <param name="userNo">ユーザ番号</param>
    /// <param name="id">武器のID</param>
    /// <returns>レベル</returns>
    public int GetWeaponLv(int userNo, string id)
    {
        return _data[userNo].weapon[id];
    }
    /// <summary>
    /// 現在所持しているアイテムの数
    /// </summary>
    /// <param name="userNo">ユーザ番号</param>
    /// <param name="id">アイテムのID</param>
    /// <returns>所持数</returns>
    public int GetHasItemNum(int userNo, string id)
    {
        return _data[userNo].item[id];
    }
    /// <summary>
    /// 所持金を増やす
    /// </summary>
    /// <param name="userNo">ユーザ番号</param>
    /// <param name="add">増やすお金の量</param>
    public void AddMoney(int userNo, int add)
    {
        _data[userNo].money += add;
    }
    /// <summary>
    /// 減らす金額で0未満にならない場合は所持金を減らす
    /// </summary>
    /// <param name="userNo">ユーザ番号</param>
    /// <param name="sub">減らすお金の量</param>
    /// <returns>true: 減らせる / false: 減らせない</returns>
    public bool SubMoney(int userNo, int sub)
    {
        int temp = _data[userNo].money;
        temp -= sub;
        if (temp < 0)
        {
            return false;
        }
        else
        {
            _data[userNo].money = temp;
            return true;
        }
    }
    /// <summary>
    /// プレイ時間の増加
    /// </summary>
    /// <param name="userNo">ユーザ番号</param>
    public void AddTime(int userNo)
    {
        ++_data[userNo].time;
    }
    /// <summary>
    /// 武器のレベルを上げる
    /// </summary>
    /// <param name="userNo">ユーザ番号</param>
    /// <param name="id">武器のID</param>
    public void LvUpWeapon(int userNo, string id)
    {
        ++_data[userNo].weapon[id];
    }
    /// <summary>
    /// 所持アイテム数を増やす
    /// </summary>
    /// <param name="userNo">ユーザ番号</param>
    /// <param name="id">アイテムのID</param>
    public void AddHasItemNum(int userNo, string id)
    {
        ++_data[userNo].item[id];
    }
    /// <summary>
    /// 所持している場合アイテムを使用する
    /// </summary>
    /// <param name="userNo">ユーザ番号</param>
    /// <param name="id">アイテムのID</param>
    /// <returns>true: 使用可能 / false: 使用不可能</returns>
    public bool UseItem(int userNo, string id)
    {
        int temp = _data[userNo].item[id];
        --temp;
        if (temp < 0)
        {
            return false;
        }
        else
        {
            _data[userNo].item[id] = temp;
            return true;
        }
    }
}
