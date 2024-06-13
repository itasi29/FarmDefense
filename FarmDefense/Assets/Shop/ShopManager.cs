using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ShopManager : MonoBehaviour
{
    private const int kMaxLevel = 10;

    private const int kMaxItemNum = 5;

    private const int kItemCost = 500;

    private GameObject _cursor;
    private CursorMove _cursorScript;

    private GameObject _weaponCostUi;
    private GameObject _itemCostUi;

    private GameObject _weaponShop;

    private GameObject _itemShop;

    private GameObject _hasGoldUi;

    int[] _stutasLevel = new int[(int)CursorMove.UpgradeParts.kPartsNum];
    int[] _hasItem = new int[(int)CursorMove.Item.kItemNum];


    private int[] _partsCost = new int[kMaxLevel];

    private int _hasGold;

    private bool _isWeaponShop;

    // Start is called before the first frame update
    void Start()
    {
        _hasGold = 10000;

        _isWeaponShop = true;

        _weaponShop = GameObject.Find("WeaponShop");

        _itemShop = GameObject.Find("ItemShop");

        _hasGoldUi = GameObject.Find("hasGold");

        _weaponCostUi = GameObject.Find("weaponCost");
        _itemCostUi = GameObject.Find("itemCost");

        _cursor = GameObject.Find("cursor");
        _cursorScript = _cursor.GetComponent<CursorMove>();

        //TODO : 外部ファイルをここで読み込む    関数化します

        //レベルの読み込み
        _stutasLevel[(int)CursorMove.UpgradeParts.kNearAtk] = 1;
        _stutasLevel[(int)CursorMove.UpgradeParts.kNearRange] = 1;
        _stutasLevel[(int)CursorMove.UpgradeParts.kNearSpd] = 1;
        _stutasLevel[(int)CursorMove.UpgradeParts.kFarAtk] = 1;
        _stutasLevel[(int)CursorMove.UpgradeParts.kFarSpd] = 1;
        _stutasLevel[(int)CursorMove.UpgradeParts.kFarRate] = 1;

        //アイテムを持っている数の読み込み
        _hasItem[(int)CursorMove.Item.kFarmHeal] = 0;
        _hasItem[(int)CursorMove.Item.kPlayerHeal] = 0;

        //コストの読み込み
        for (int i = 0; i < kMaxLevel; i++)
        {
            _partsCost[i] = (i) * 100;
        }
        //開いていないショップを消しておく
        _itemShop.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //所持ゴールドの表示
        Text goldUi = _hasGoldUi.GetComponent<Text>();

        goldUi.text = _hasGold.ToString();


        //武器ショップを開いているとき
        if (_isWeaponShop)
        {
            for (int i = 0; i < (int)CursorMove.UpgradeParts.kPartsNum; i++)
            {
                Text weaponCost = _weaponCostUi.transform.GetChild(i).gameObject.GetComponentInChildren<Text>();

                if (_stutasLevel[i] >= kMaxLevel)
                {
                    weaponCost.text = "MAX";
                }
                else
                {
                    weaponCost.text = _stutasLevel[i].ToString()+ ":" + _partsCost[_stutasLevel[i]].ToString();
                }
            }
        }
        //アイテムショップを開いているとき
        else
        {
            for (int i = 0; i < (int)CursorMove.Item.kItemNum; i++)
            {
                Text itemCost = _itemCostUi.transform.GetChild(i).gameObject.GetComponentInChildren<Text>();
                if (_hasItem[i] >= kMaxItemNum)
                {
                    itemCost.text = "MAX";
                }
                else
                {
                    itemCost.text = _hasItem[i].ToString() + ":" + kItemCost.ToString(); 
                }
            }
        }

        //アイテムと武器のショップの切り替え
        if (_isWeaponShop && Input.GetButtonDown("RB"))
        {
            _isWeaponShop = false;
            _itemShop.SetActive(true);
            _weaponShop.SetActive(false);
        }
        else if (!_isWeaponShop && Input.GetButtonDown("LB"))
        {
            _isWeaponShop = true;
            _itemShop.SetActive(false);
            _weaponShop.SetActive(true);
        }


        //購入ボタンが押されたとき
        if (Input.GetButtonDown("A"))
        {
            //強化パーツショップを開いていた場合
            if (_isWeaponShop)
            {
                //選んでいるパーツのレベルがマックスじゃなければ
                if (_stutasLevel[(int)_cursorScript.GetSelectPart()] < kMaxLevel)
                {
                    //選んでいるパーツの値段よりも持っているお金が多かったら
                    if (_partsCost[_stutasLevel[(int)_cursorScript.GetSelectPart()]] <= _hasGold)
                    {
                        //お金を減らす
                        _hasGold -= _partsCost[_stutasLevel[(int)_cursorScript.GetSelectPart()]];

                        //選択している武器パーツを強化する
                        _stutasLevel[(int)_cursorScript.GetSelectPart()]++;
                        Debug.Log(_hasGold);
                        Debug.Log(_stutasLevel[(int)_cursorScript.GetSelectPart()]);
                    }

                }

            }
            //アイテムショップを開いていた場合
            else
            {
                //選んでいるアイテムの数が上限じゃなければ
                if (_hasItem[(int)_cursorScript.GetSelectItem()] < kMaxItemNum)
                {
                    //選んでいるパーツの値段よりも持っているお金が多かったら
                    if (kItemCost <= _hasGold)
                    {
                        //お金を減らす
                        _hasGold -= kItemCost;

                        //選択しているアイテムを購入する
                        _hasItem[(int)_cursorScript.GetSelectItem()]++;
                        Debug.Log(_hasGold);
                        Debug.Log(_hasItem[(int)_cursorScript.GetSelectItem()]);
                    }

                }
            }
        }
    }

    public bool IsGetWeaponShop() { return _isWeaponShop; }
}
