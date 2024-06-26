using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using UnityEngine;

class Tuple<T1, T2>
{
    public T1 item1;
    public T2 item2;
    public Tuple(T1 item1, T2 item2)
    {
        this.item1 = item1;
        this.item2 = item2;
    }
}

public class WeaponData
{
        private Dictionary<string, Dictionary<int, int>> _data = new Dictionary<string, Dictionary<int, int>>();
//    private Dictionary<Tuple<string, int>, int> _data = new Dictionary<Tuple<string, int>, int>();
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
            //_data.Add(new Tuple<string, int>(item.ID, 1), item.Lv1);
            //_data.Add(new Tuple<string, int>(item.ID, 2), item.Lv2);
            //_data.Add(new Tuple<string, int>(item.ID, 3), item.Lv3);
            //_data.Add(new Tuple<string, int>(item.ID, 4), item.Lv4);
            //_data.Add(new Tuple<string, int>(item.ID, 5), item.Lv5);
            //_data.Add(new Tuple<string, int>(item.ID, 6), item.Lv6);
            //_data.Add(new Tuple<string, int>(item.ID, 7), item.Lv7);
            //_data.Add(new Tuple<string, int>(item.ID, 8), item.Lv8);
            //_data.Add(new Tuple<string, int>(item.ID, 9), item.Lv9);
            //_data.Add(new Tuple<string, int>(item.ID, 10), item.Lv10);

            _idList.Add(item.ID);
        }

#if false
        foreach (var item in _data)
        {
            foreach (var lv in item.Value)
            {
                Debug.Log("ID:" + item.Key + ", lv:" + lv.Key + ", data" + lv.Value);
            }
        }
#endif
    }

    public int GetStatus(string id, int lv)
    {
        return _data[id][lv];
//        return _data[new Tuple<string, int>(id, lv)];
    }

    public List<string> GetIdList()
    {
        return _idList;
    }
}
