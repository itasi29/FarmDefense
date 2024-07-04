using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData
{
    private Dictionary<string, List<int>> _data = new Dictionary<string, List<int>>();
    private List<string> _idList = new List<string>();

    public void Load()
    {
        // csvファイルの読み込み
        TextAsset csv = Resources.Load(DataManager.kImteFileName) as TextAsset;
        // データ読み込み
        ItemCSV[] items = CSVSerializer.Deserialize<ItemCSV>(csv.text);

        foreach (var item in items)
        {
            // 既に含まれている場合
            if (_data.ContainsKey(item.ID))
            {
                _data[item.ID].Add(item.Effect1);
            }
            // まだ含まれていない場合
            else
            {
                var array = new List<int>();
                array.Add(item.Effect1);
                _data.Add(item.ID, array);

                _idList.Add(item.ID);
            }
        }
    }

    public int GetEffect(string id, int lv)
    {
        return _data[id][lv];
    }
    
    public List<string> GetIdList()
    {
        return _idList;
    }
}
