using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShopData
{
    private Dictionary<string, Dictionary<int, int>> _data;

    public void Load()
    {
        // csvファイルの読み込み
        TextAsset csv = Resources.Load("Csv/ShopManager") as TextAsset;
        // データ読み込み
        ShopCSV[] items = CSVSerializer.Deserialize<ShopCSV>(csv.text);

        foreach (var item in items)
        {
            _data[item.id][1]  = item.Lv1;
            _data[item.id][2]  = item.Lv2;
            _data[item.id][3]  = item.Lv3;
            _data[item.id][4]  = item.Lv4;
            _data[item.id][5]  = item.Lv5;
            _data[item.id][6]  = item.Lv6;
            _data[item.id][7]  = item.Lv7;
            _data[item.id][8]  = item.Lv8;
            _data[item.id][9]  = item.Lv9;
            _data[item.id][10] = item.Lv10;
        }
    }

    public int GetCost(string id, int lv)
    {
        return _data[id][lv];
    }
}
