using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorMove : MonoBehaviour
{

    public enum UpgradeParts
    {
        kNearAtk,
        kNearSpd,
        kNearRange,
        kFarAtk,
        kFarRate,
        kFarSpd,
        kPartsNum
    }
    public enum Item
    {
        kFarmHealLv1,
        kFarmHealLv2,
        kFarmHealLv3,
        kPlayerHealLv1,
        kPlayerHealLv2,
        kPlayerHealLv3,
        kItemNum
    }

    private const int kImageWidth = 230;//画像の横幅(仮設定)

    private const int kWaitTime = 15;//長押しでカーソルを動かすときの間隔

    private Vector2 _cursorIndex;//カーソルのいる位置

    private UpgradeParts _selectPart;//選んでいる強化パーツenum

    private Item _selectItem;//選んでいるアイテムenum

    private ShopManager _shopManager;//ショップを管理しているスクリプト

    private GameObject _weaponShop;//武器ショップ
    private GameObject _itemShop;//アイテムショップ

    private GameObject _selectImage;//選択している画像
    private GameObject _cursorImage;//カーソルの画像

    private bool _isMoveCursor;//カーソルを動かしたかどうか
    private int _waitTime;//カーソルを動かす間隔

    private bool _lastShowWeaponShop;//前のフレーム開いていたショップ

    // Start is called before the first frame update
    void Start()
    {
        _waitTime = 0;
        _isMoveCursor = false;
        _cursorIndex = new Vector2(0, 0);
        _cursorImage = GameObject.Find("cursor");
        _selectImage = GameObject.Find("nearAtk");
        _selectPart = UpgradeParts.kNearAtk;
        _selectItem = Item.kFarmHealLv1;
        _shopManager = GameObject.Find("Manager").GetComponent<ShopManager>();

        _weaponShop = GameObject.Find("WeaponShop");
        _itemShop = GameObject.Find("ItemShop");

        _lastShowWeaponShop = true;
    }

    // Update is called once per frame
    void Update()
    {
        MoveCursor();

        Vector2 cursorPos = new Vector2(0, 0);

        //前のフレーム開いていたショップと違うショップを開いていたら
        if (_lastShowWeaponShop != _shopManager.IsGetWeaponShop())
        {
            //カーソルの位置を初期位置に戻す
            _cursorIndex = new Vector2(0, 0);
            _lastShowWeaponShop = _shopManager.IsGetWeaponShop();
        }

        //強化パーツショップを開いている場合
        if (_lastShowWeaponShop)
        {
            if (_cursorIndex.x == 0)
            {
                if (_cursorIndex.y == 0)
                {
                    _selectImage = _weaponShop.transform.GetChild((int)UpgradeParts.kNearAtk).gameObject;
                    _selectPart = UpgradeParts.kNearAtk;
                }
                else if (_cursorIndex.y == 1)
                {
                    _selectImage = _weaponShop.transform.GetChild((int)UpgradeParts.kNearRange).gameObject;
                    _selectPart = UpgradeParts.kNearRange;
                }
                else if (_cursorIndex.y == 2)
                {
                    _selectImage = _weaponShop.transform.GetChild((int)UpgradeParts.kNearSpd).gameObject;
                    _selectPart = UpgradeParts.kNearSpd;
                }
            }
            else
            {
                if (_cursorIndex.y == 0)
                {
                    _selectImage = _weaponShop.transform.GetChild((int)UpgradeParts.kFarAtk).gameObject;
                    _selectPart = UpgradeParts.kFarAtk;
                }
                else if (_cursorIndex.y == 1)
                {
                    _selectImage = _weaponShop.transform.GetChild((int)UpgradeParts.kFarSpd).gameObject;
                    _selectPart = UpgradeParts.kFarSpd;
                }
                else if (_cursorIndex.y == 2)
                {
                    _selectImage = _weaponShop.transform.GetChild((int)UpgradeParts.kFarRate).gameObject;
                    _selectPart = UpgradeParts.kFarRate;
                }
            }
        }
        //アイテムショップを開いている場合
        else
        {
            if (_cursorIndex.x == 0)
            {
                if (_cursorIndex.y == 0)
                {
                    _selectImage = _itemShop.transform.GetChild((int)Item.kFarmHealLv1).gameObject;
                    _selectItem = Item.kFarmHealLv1;
                }
                else if (_cursorIndex.y == 1)
                {
                    _selectImage = _itemShop.transform.GetChild((int)Item.kFarmHealLv2).gameObject;
                    _selectItem = Item.kFarmHealLv2;
                }
                else if (_cursorIndex.y == 2)
                {
                    _selectImage = _itemShop.transform.GetChild((int)Item.kFarmHealLv3).gameObject;
                    _selectItem = Item.kFarmHealLv3;
                }

            }
            else if (_cursorIndex.x == 1)
            {
                if (_cursorIndex.y == 0)
                {
                    _selectImage = _itemShop.transform.GetChild((int)Item.kPlayerHealLv1).gameObject;
                    _selectItem = Item.kPlayerHealLv1;
                }
                else if (_cursorIndex.y == 1)
                {
                    _selectImage = _itemShop.transform.GetChild((int)Item.kPlayerHealLv2).gameObject;
                    _selectItem = Item.kPlayerHealLv2;
                }
                else if (_cursorIndex.y == 2)
                {
                    _selectImage = _itemShop.transform.GetChild((int)Item.kPlayerHealLv3).gameObject;
                    _selectItem = Item.kPlayerHealLv3;
                }
            }
        }
        //画像の位置にカーソルを移動
        cursorPos = _selectImage.transform.position;
        //画像の横幅の大きさだけカーソルを右にずらす
        cursorPos.x = _selectImage.transform.position.x + kImageWidth;

        _cursorImage.transform.position = cursorPos;
    }

    private void FixedUpdate()
    {
        _waitTime++;
        if (_waitTime > kWaitTime)
        {
            _isMoveCursor = false;
        }
    }

    void MoveCursor()
    {
        if (!_isMoveCursor)
        {
            bool isHit = false;
            if (Input.GetAxis("DPADY") == 1)
            {
                if (_cursorIndex.y > 0)
                {
                    _cursorIndex.y--;
                    isHit = true;
                }
                else
                {
                    _cursorIndex.y = 2;
                    isHit = true;
                }
            }
            else if (Input.GetAxis("DPADY") == -1)
            {
                if (_cursorIndex.y < 2)
                {
                    _cursorIndex.y++;
                    isHit = true;
                }
                else
                {
                    _cursorIndex.y = 0;
                    isHit = true;
                }
            }
            if (Input.GetAxis("DPADX") == -1)
            {
                if (_cursorIndex.x > 0)
                {
                    _cursorIndex.x--;
                    isHit = true;
                }
            }
            else if (Input.GetAxis("DPADX") == 1)
            {
                if (_cursorIndex.x < 1)
                {
                    _cursorIndex.x++;
                    isHit = true;
                }
            }
            if (isHit)
            {
                _isMoveCursor = true;
                _waitTime = 0;
            }
        }

        //何も入力がなかったら次のフレームから入力できるようになる
        if (Input.GetAxis("DPADX") == 0 && Input.GetAxis("DPADY") == 0)
        {
            _isMoveCursor = false;
        }
    }

    public UpgradeParts GetSelectPart() { return _selectPart; }
    public Item GetSelectItem() { return _selectItem; }
    public int GetSelectItemLevel()
    {
        int ans = 0;

        if (_selectItem == Item.kPlayerHealLv1 || _selectItem == Item.kFarmHealLv1)
        {
            ans = 0;
        }
        else if (_selectItem == Item.kPlayerHealLv2 || _selectItem == Item.kFarmHealLv2)
        {
            ans = 1;
        }
        else if (_selectItem == Item.kPlayerHealLv3 || _selectItem == Item.kFarmHealLv3)
        {
            ans = 2;
        }
        return ans;
    }
}
