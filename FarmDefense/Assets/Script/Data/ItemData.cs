using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData
{
    private Dictionary<string, List<int>> _data = new Dictionary<string, List<int>>();
    private List<string> _idList = new List<string>();

    public void Load()
    {
        // csv�t�@�C���̓ǂݍ���
        TextAsset csv = Resources.Load(DataManager.kImteFileName) as TextAsset;
        // �f�[�^�ǂݍ���
        ItemCSV[] items = CSVSerializer.Deserialize<ItemCSV>(csv.text);

        foreach (var item in items)
        {
            // ���Ɋ܂܂�Ă���ꍇ
            if (_data.ContainsKey(item.ID))
            {
                _data[item.ID].Add(item.Effect1);
            }
            // �܂��܂܂�Ă��Ȃ��ꍇ
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
