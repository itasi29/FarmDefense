using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Item : MonoBehaviour
{
    /* 型 */
    // アイテムの種類
    enum Kind   
    {
        kOwnRecover,
        kFarmRecover
    };

    /* 定数 */
    // IDリストとの対応表
    private Dictionary<string, Kind> kItemTable = new Dictionary<string, Kind>
    {
        { "I_0", Kind.kOwnRecover  },
        { "I_1", Kind.kFarmRecover },
    };

    /* 変数 */
    private int _index;
    private bool _isPrePush;
    private ItemData _item;
    private UserData _user;
    private Player _player;
    private List<string> _idList;
    private int _maxIndex;

    public void Init(Player player)
    {
        _index = 0;
        _isPrePush = false;
        var director = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        _item = director.DataMgr.Item;
        _user = director.DataMgr.User;
        _player = player;
        _idList = _item.GetIdList();
        _maxIndex = _idList.Count;
    }

    // Update is called once per frame
    public void Change()
    {
        float input = Input.GetAxis("DPADY");

        if (!_isPrePush)
        {
            bool isPush = false;
            if (input == 1)
            {
                _index = (_maxIndex + _index - 1) % _maxIndex;
                isPush = true;
            }
            else if (input == -1)
            {
                _index = (_index + 1) % _maxIndex;
                isPush = true;
            }

            if (isPush)
            {
                _isPrePush = true;
            }
        }
        else
        {
            if (input == 0.0f)
            {
                _isPrePush = false;
            }
        }
    }

    public void Use(Farm farm)
    {
        int idIndex = _index / 3;
        string id = _idList[idIndex];
        int lv = _index % 3;

        if (_user.UseItem(id, lv))
        {
            var kind = kItemTable[id];
            if (kind == Kind.kOwnRecover)
            {
                _player.OnRecover(_item.GetEffect(id, lv));
            }
            else if (kind == Kind.kFarmRecover)
            {
                
                if (farm)
                {
                    farm.OnRepair(_item.GetEffect(id, lv));
                }
            }
        }
    }
}
