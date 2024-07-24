using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    private const int kMaxLevel = 10;

    private GameObject _cursor;
    private CursorMove _cursorScript;

    private GameObject _weaponCostUi;

    private GameObject _hasGoldUi;

    private int[] _stutasLevel = new int[(int)CursorMove.UpgradeParts.kPartsNum];

    private string[] _partsId = new string[(int)CursorMove.UpgradeParts.kPartsNum];

    private ShopData _shopData;

    private UserData _userData;

    private WeaponData _weaponData;

    private DataManager _dataManager;

    private Fade _fade;

    // Start is called before the first frame update
    void Start()
    {
        _hasGoldUi = GameObject.Find("hasGold");

        _weaponCostUi = GameObject.Find("weaponCost");

        _cursor = GameObject.Find("cursor");
        _cursorScript = _cursor.GetComponent<CursorMove>();

        _fade = GetComponent<Fade>();

        //TODO : 外部ファイルをここで読み込む

        _dataManager = GameObject.Find("GameDirector").GetComponent<GameDirector>().DataMgr;

        _shopData = _dataManager.Shop;
        _userData = _dataManager.User;
        _weaponData = _dataManager.Weapon;

        //データの読み込み
        InitUserData();

        //IDの読み込み
        for (int i = 0; i < (int)CursorMove.UpgradeParts.kPartsNum; i++)
        {
            _partsId[i] = _weaponData.GetIdList()[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        //所持ゴールドの表示
        Text goldUi = _hasGoldUi.GetComponent<Text>();

        goldUi.text = "しょじきん：" + _userData.GetMoney().ToString();


        for (int i = 0; i < (int)CursorMove.UpgradeParts.kPartsNum; i++)
        {
            Text weaponCost = _weaponCostUi.transform.GetChild(i).gameObject.GetComponentInChildren<Text>();

            if (_stutasLevel[i] >= kMaxLevel)
            {
                weaponCost.text = "MAX";
            }
            else
            {
                weaponCost.text = _stutasLevel[i].ToString() + ":" + _shopData.GetCost(_partsId[i], _stutasLevel[i]).ToString();
            }
        }

        if (Input.GetButtonDown("B"))
        {
            _fade.StartFadeOut("StageSelectScene");
        }

        //購入ボタンが押されたとき
        if (Input.GetButtonDown("A"))
        {
            //選んでいるパーツを取得する
            int selectPart = (int)_cursorScript.GetSelectPart();
            //選んでいるパーツのレベルがマックスじゃなければ
            if (_stutasLevel[selectPart] < kMaxLevel)
            {
                //選んでいるパーツのコストを取得する
                int cost = _shopData.GetCost(_partsId[selectPart], _stutasLevel[selectPart]);
                //選んでいるパーツの値段よりも持っているお金が多かったらお金を減らす
                if (_userData.SubMoney(cost))
                {
                    //選択している武器パーツを強化する
                    _stutasLevel[selectPart]++;
                    _userData.LvUpWeapon(_partsId[selectPart]);
                }

            }
        }
    }

    private void InitUserData()
    {
        //レベルの読み込み
        _stutasLevel[(int)CursorMove.UpgradeParts.kNearAtk] = _userData.GetWeaponLv("W_0");
        _stutasLevel[(int)CursorMove.UpgradeParts.kNearSpd] = _userData.GetWeaponLv("W_1");
        _stutasLevel[(int)CursorMove.UpgradeParts.kNearRange] = _userData.GetWeaponLv("W_2");
        _stutasLevel[(int)CursorMove.UpgradeParts.kFarAtk] = _userData.GetWeaponLv("W_3");
        _stutasLevel[(int)CursorMove.UpgradeParts.kFarRate] = _userData.GetWeaponLv("W_4");
        _stutasLevel[(int)CursorMove.UpgradeParts.kFarSpd] = _userData.GetWeaponLv("W_5");
    }

}
