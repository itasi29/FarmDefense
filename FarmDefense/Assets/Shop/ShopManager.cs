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

    private GameObject _cursor;
    private CursorMove _cursorScript;

    private GameObject _weaponCostUi;
    private GameObject _itemCostUi;

    private GameObject _weaponShop;

    private GameObject _itemShop;

    private GameObject _hasGoldUi;

    private int[] _stutasLevel = new int[(int)CursorMove.UpgradeParts.kPartsNum];
    private int[] _hasItem = new int[(int)CursorMove.Item.kItemNum];

    private string[] _partsId = new string[(int)CursorMove.UpgradeParts.kPartsNum];
    private string[] _itemId = new string[(int)CursorMove.Item.kItemNum];

    private bool _isWeaponShop;

    private ShopData _shopData;

    private UserData _userData;

    private ItemData _itemData;

    private WeaponData _weaponData;

    private DataManager _dataManager;
    int userNo = 0;
    // Start is called before the first frame update
    void Start()
    {
        _isWeaponShop = true;

        _weaponShop = GameObject.Find("WeaponShop");

        _itemShop = GameObject.Find("ItemShop");

        _hasGoldUi = GameObject.Find("hasGold");

        _weaponCostUi = GameObject.Find("weaponCost");
        _itemCostUi = GameObject.Find("itemCost");

        _cursor = GameObject.Find("cursor");
        _cursorScript = _cursor.GetComponent<CursorMove>();

        //TODO : �O���t�@�C���������œǂݍ���

        _dataManager = GameObject.Find("DataManager").GetComponent<DataManager>();

        _shopData = _dataManager.Shop;
        _userData = _dataManager.User;
        _itemData = _dataManager.Item;
        _weaponData = _dataManager.Weapon;

        //�f�[�^�̓ǂݍ���
        InitUserData(userNo);

        //ID�̓ǂݍ���
        for (int i = 0; i < (int)CursorMove.UpgradeParts.kPartsNum; i++)
        {
            _partsId[i] = _weaponData.GetIdList()[i];
        }
        for (int i = 0; i < (int)CursorMove.Item.kItemNum; i++)
        {
            _itemId[i] = _itemData.GetIdList()[i];
        }
        //�J���Ă��Ȃ��V���b�v�������Ă���
        _itemShop.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //�����S�[���h�̕\��
        Text goldUi = _hasGoldUi.GetComponent<Text>();

        goldUi.text = _userData.GetMoney(userNo).ToString();


        //����V���b�v���J���Ă���Ƃ�
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
                    weaponCost.text = _stutasLevel[i].ToString() + ":" + _shopData.GetCost(_partsId[i], _stutasLevel[i]).ToString();
                }
            }
        }
        //�A�C�e���V���b�v���J���Ă���Ƃ�
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
                    itemCost.text = _hasItem[i].ToString() + ":" + _shopData.GetCost(_itemId[i], _hasItem[i]).ToString();
                }
            }
        }

        //�A�C�e���ƕ���̃V���b�v�̐؂�ւ�
        if (_isWeaponShop && Input.GetButtonDown("RB"))
        {
            _isWeaponShop = false;
            _itemShop.SetActive(true);
            _weaponShop.SetActive(false);
            return;
        }
        else if (!_isWeaponShop && Input.GetButtonDown("LB"))
        {
            _isWeaponShop = true;
            _itemShop.SetActive(false);
            _weaponShop.SetActive(true);
            return;
        }


        //�w���{�^���������ꂽ�Ƃ�
        if (Input.GetButtonDown("A"))
        {
            //�����p�[�c�V���b�v���J���Ă����ꍇ
            if (_isWeaponShop)
            {
                //�I��ł���p�[�c���擾����
                int selectPart = (int)_cursorScript.GetSelectPart();
                //�I��ł���p�[�c�̃��x�����}�b�N�X����Ȃ����
                if (_stutasLevel[selectPart] < kMaxLevel)
                {
                    //�I��ł���p�[�c�̃R�X�g���擾����
                    int cost = _shopData.GetCost(_partsId[selectPart], _stutasLevel[selectPart]);
                    //�I��ł���p�[�c�̒l�i���������Ă��邨�������������炨�������炷
                    if (_userData.SubMoney(userNo,cost))
                    {
                        //�I�����Ă��镐��p�[�c����������
                        _stutasLevel[selectPart]++;
                        _userData.LvUpWeapon(userNo, _partsId[selectPart]);
                    }

                }

            }
            //�A�C�e���V���b�v���J���Ă����ꍇ
            else
            {
                int selectItem = (int)_cursorScript.GetSelectItem();
                int itemLevel = _cursorScript.GetSelectItemLevel();
                //�I��ł���A�C�e���̐����������Ȃ����
                if (_hasItem[selectItem] < kMaxItemNum)
                {
                    int cost = _shopData.GetCost(_itemId[selectItem], itemLevel);
                    //�I��ł���p�[�c�̒l�i���������Ă��邨�������������炨�������炷
                    if (_userData.SubMoney(userNo,cost))
                    {
                        //�I�����Ă���A�C�e�����w������
                        _hasItem[(int)_cursorScript.GetSelectItem()]++;
                        _userData.AddHasItemNum(userNo, _itemId[selectItem], itemLevel);
                    }

                }
            }
        }
    }

    private void InitUserData(int userNo)
    {
        //���x���̓ǂݍ���
        _stutasLevel[(int)CursorMove.UpgradeParts.kNearAtk] = _userData.GetWeaponLv(userNo, "W_0");
        _stutasLevel[(int)CursorMove.UpgradeParts.kNearSpd] = _userData.GetWeaponLv(userNo, "W_1");
        _stutasLevel[(int)CursorMove.UpgradeParts.kNearRange] = _userData.GetWeaponLv(userNo, "W_2");
        _stutasLevel[(int)CursorMove.UpgradeParts.kFarAtk] = _userData.GetWeaponLv(userNo, "W_3");
        _stutasLevel[(int)CursorMove.UpgradeParts.kFarRate] = _userData.GetWeaponLv(userNo, "W_4");
        _stutasLevel[(int)CursorMove.UpgradeParts.kFarSpd] = _userData.GetWeaponLv(userNo, "W_5");

        //�A�C�e���������Ă��鐔�̓ǂݍ���
        _hasItem[(int)CursorMove.Item.kFarmHealLv1] = _userData.GetHasItemNum(userNo, "I_0", 0);
        _hasItem[(int)CursorMove.Item.kFarmHealLv2] = _userData.GetHasItemNum(userNo, "I_1", 1);
        _hasItem[(int)CursorMove.Item.kFarmHealLv3] = _userData.GetHasItemNum(userNo, "I_2", 2);
        _hasItem[(int)CursorMove.Item.kPlayerHealLv1] = _userData.GetHasItemNum(userNo, "I_3", 0);
        _hasItem[(int)CursorMove.Item.kPlayerHealLv2] = _userData.GetHasItemNum(userNo, "I_4", 1);
        _hasItem[(int)CursorMove.Item.kPlayerHealLv3] = _userData.GetHasItemNum(userNo, "I_5", 2);
    }

    public bool IsGetWeaponShop() { return _isWeaponShop; }
}
