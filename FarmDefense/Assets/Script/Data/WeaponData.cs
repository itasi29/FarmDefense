using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponData
{
    private Dictionary<string, Dictionary<int, int>> _data;
    private List<string> _idList;

    public void Load()
    {
        // csvファイルの読み込み
        TextAsset csv = Resources.Load("Csv/WeaponManager") as TextAsset;
        // データ読み込み
        WeaponCSV[] items = CSVSerializer.Deserialize<WeaponCSV>(csv.text);

        foreach (var item in items)
        {
            _data[item.ID][1] = item.Lv1;
            _data[item.ID][2] = item.Lv2;
            _data[item.ID][3] = item.Lv3;
            _data[item.ID][4] = item.Lv4;
            _data[item.ID][5] = item.Lv5;
            _data[item.ID][6] = item.Lv6;
            _data[item.ID][7] = item.Lv7;
            _data[item.ID][8] = item.Lv8;
            _data[item.ID][9] = item.Lv9;
            _data[item.ID][10] = item.Lv10;

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
