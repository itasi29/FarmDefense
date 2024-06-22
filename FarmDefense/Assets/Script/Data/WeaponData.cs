using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponData
{
    private Dictionary<string, Dictionary<int, int>> _data = new Dictionary<string, Dictionary<int, int>>();
    private List<string> _idList = new List<string>();

    public void Load()
    {
        // csvファイルの読み込み
        TextAsset csv = Resources.Load(DataManager.kWeaponFileName) as TextAsset;
        // データ読み込み
        WeaponCSV[] items = CSVSerializer.Deserialize<WeaponCSV>(csv.text);

        foreach (var item in items)
        {
            Dictionary<int, int> effect = new Dictionary<int, int>();
            effect.Add(1, item.Lv1);
            effect.Add(2, item.Lv2);
            effect.Add(3, item.Lv3);
            effect.Add(4, item.Lv4);
            effect.Add(5, item.Lv5);
            effect.Add(6, item.Lv6);
            effect.Add(7, item.Lv7);
            effect.Add(8, item.Lv8);
            effect.Add(9, item.Lv9);
            effect.Add(10, item.Lv10);
            _data.Add(item.ID, effect);

            _idList.Add(item.ID);
        }
    }

    public int GetStatus(string id, int lv)
    {
        return _data[id][lv];
    }

    public List<string> GetIdList()
    {
        return _idList;
    }
}
