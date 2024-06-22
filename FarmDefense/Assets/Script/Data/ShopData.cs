using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShopData
{
    private Dictionary<string, Dictionary<int, int>> _data = new Dictionary<string, Dictionary<int, int>>();

    public void Load()
    {
        // csvファイルの読み込み
        TextAsset csv = Resources.Load(DataManager.kShopFileName) as TextAsset;
        // データ読み込み
        ShopCSV[] items = CSVSerializer.Deserialize<ShopCSV>(csv.text);

        foreach (var item in items)
        {
            Dictionary<int, int> cost = new Dictionary<int, int>();
            cost.Add(1, item.Lv1);
            cost.Add(2, item.Lv2);
            cost.Add(3, item.Lv3);
            cost.Add(4, item.Lv4);
            cost.Add(5, item.Lv5);
            cost.Add(6, item.Lv6);
            cost.Add(7, item.Lv7);
            cost.Add(8, item.Lv8);
            cost.Add(9, item.Lv9);
            cost.Add(10, item.Lv10);
            _data.Add(item.ID, cost);
        }
    }

    public int GetCost(string id, int lv)
    {
        return _data[id][lv];
    }
}
