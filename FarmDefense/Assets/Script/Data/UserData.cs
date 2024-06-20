using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public struct User
{
    public int money;   // 所持金お金
    public int time;    // プレイ時間
    public Dictionary<string, int> weaponLv;    // 武器のレベル
    public Dictionary<string, int> item;        // アイテム所持数
}

public class UserData
{
    // ファイルのパス
    private const string kPath = "Bin/UserData.bin";
    // ユーザデータ保持数
    private const int kUserNum = 4;

    // 各ユーザデータの情報
    private Dictionary<int, User> _data;

    /// <summary>
    /// 読み込み
    /// </summary>
    public void Load(List<string> weaponIdList, List<string> itemIdList)
    {
        // kPathのファイルを読み込みモードで開く(存在しない場合は新しく作成する)
        try
        {

        }
        catch 
        { 

        }
    }

    /// <summary>
    /// 書き込み
    /// </summary>
    public void Write()
    {

    }
}
