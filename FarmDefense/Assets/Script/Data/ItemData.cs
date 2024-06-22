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
    private List<string> _idList = new List<string>();

    public void Load()
    {
        // csvファイルの読み込み
        TextAsset csv = Resources.Load(DataManager.kImteFileName) as TextAsset;
        // データ読み込み
        ItemCSV[] items = CSVSerializer.Deserialize<ItemCSV>(csv.text);

        foreach (var item in items)
        {
            Item temp = new Item();
            temp.type = item.Type;
            temp.effect1 = item.Effect1;

            _data.Add(item.ID, temp);

            _idList.Add(item.ID);
        }
    }

    public Item GetItem(string id)
    {
        return _data[id];
    }
    
    public List<string> GetIdList()
    {
        return _idList;
    }
}
