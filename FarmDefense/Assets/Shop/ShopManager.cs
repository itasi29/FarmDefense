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

        //TODO : �O���t�@�C���������œǂݍ���    �֐������܂�

        //���x���̓ǂݍ���
        _stutasLevel[(int)CursorMove.UpgradeParts.kNearAtk] = 1;
        _stutasLevel[(int)CursorMove.UpgradeParts.kNearRange] = 1;
        _stutasLevel[(int)CursorMove.UpgradeParts.kNearSpd] = 1;
        _stutasLevel[(int)CursorMove.UpgradeParts.kFarAtk] = 1;
        _stutasLevel[(int)CursorMove.UpgradeParts.kFarSpd] = 1;
        _stutasLevel[(int)CursorMove.UpgradeParts.kFarRate] = 1;

        //�A�C�e���������Ă��鐔�̓ǂݍ���
        _hasItem[(int)CursorMove.Item.kFarmHeal] = 0;
        _hasItem[(int)CursorMove.Item.kPlayerHeal] = 0;

        //�R�X�g�̓ǂݍ���
        for (int i = 0; i < kMaxLevel; i++)
        {
            _partsCost[i] = (i) * 100;
        }
        //�J���Ă��Ȃ��V���b�v�������Ă���
        _itemShop.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //�����S�[���h�̕\��
        Text goldUi = _hasGoldUi.GetComponent<Text>();

        goldUi.text = _hasGold.ToString();


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
                    weaponCost.text = _stutasLevel[i].ToString()+ ":" + _partsCost[_stutasLevel[i]].ToString();
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
                    itemCost.text = _hasItem[i].ToString() + ":" + kItemCost.ToString(); 
                }
            }
        }

        //�A�C�e���ƕ���̃V���b�v�̐؂�ւ�
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


        //�w���{�^���������ꂽ�Ƃ�
        if (Input.GetButtonDown("A"))
        {
            //�����p�[�c�V���b�v���J���Ă����ꍇ
            if (_isWeaponShop)
            {
                //�I��ł���p�[�c�̃��x�����}�b�N�X����Ȃ����
                if (_stutasLevel[(int)_cursorScript.GetSelectPart()] < kMaxLevel)
                {
                    //�I��ł���p�[�c�̒l�i���������Ă��邨��������������
                    if (_partsCost[_stutasLevel[(int)_cursorScript.GetSelectPart()]] <= _hasGold)
                    {
                        //���������炷
                        _hasGold -= _partsCost[_stutasLevel[(int)_cursorScript.GetSelectPart()]];

                        //�I�����Ă��镐��p�[�c����������
                        _stutasLevel[(int)_cursorScript.GetSelectPart()]++;
                        Debug.Log(_hasGold);
                        Debug.Log(_stutasLevel[(int)_cursorScript.GetSelectPart()]);
                    }

                }

            }
            //�A�C�e���V���b�v���J���Ă����ꍇ
            else
            {
                //�I��ł���A�C�e���̐����������Ȃ����
                if (_hasItem[(int)_cursorScript.GetSelectItem()] < kMaxItemNum)
                {
                    //�I��ł���p�[�c�̒l�i���������Ă��邨��������������
                    if (kItemCost <= _hasGold)
                    {
                        //���������炷
                        _hasGold -= kItemCost;

                        //�I�����Ă���A�C�e�����w������
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
