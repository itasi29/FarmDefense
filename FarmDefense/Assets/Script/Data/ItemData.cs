using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Item
{
    public string type;
    public float effect1;
}

public class ItemData
{
    private Dictionary<string, Item> _data = new Dictionary<string, Item>();

    public void Load()
    {
        // csvファイルの読み込み
        TextAsset csv = Resources.Load("Csv/ItemManager") as TextAsset;
        // データ読み込み
        ItemCSV[] items = CSVSerializer.Deserialize<ItemCSV>(csv.text);

        foreach (var item in items)
        {
            Item temp = new Item();
            temp.type = item.Type;
            temp.effect1 = item.Effect1;

            _data[item.ID] = temp;
        }
    }

    public Item GetItem(string id)
    {
        return _data[id];
    }
}
