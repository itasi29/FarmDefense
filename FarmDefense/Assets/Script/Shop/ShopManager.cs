using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    private const int kMaxLevel = 10;

    private GameObject _cursor;
    private CursorMove _cursorScript;

    [SerializeField] private GameObject _weaponCostUi;

    [SerializeField] private GameObject _hasGoldUi;

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
        _cursor = GameObject.Find("cursor");
        _cursorScript = _cursor.GetComponent<CursorMove>();

        _fade = GetComponent<Fade>();

        //TODO : �O���t�@�C���������œǂݍ���

        _dataManager = GameObject.Find("GameDirector").GetComponent<GameDirector>().DataMgr;

        _shopData = _dataManager.Shop;
        _userData = _dataManager.User;
        _weaponData = _dataManager.Weapon;

        //�f�[�^�̓ǂݍ���
        InitUserData();

        //ID�̓ǂݍ���
        for (int i = 0; i < (int)CursorMove.UpgradeParts.kPartsNum; i++)
        {
            _partsId[i] = _weaponData.GetIdList()[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        //�����S�[���h�̕\��
        TextMeshProUGUI goldUi = _hasGoldUi.GetComponent<TextMeshProUGUI>();

        goldUi.text = "���傶����F" + _userData.GetMoney().ToString();


        for (int i = 0; i < (int)CursorMove.UpgradeParts.kPartsNum; i++)
        {
            TextMeshProUGUI weaponCost = _weaponCostUi.transform.GetChild(i).gameObject.GetComponentInChildren<TextMeshProUGUI>();

            if (_stutasLevel[i] >= kMaxLevel)
            {
                weaponCost.text = "Lv�FMAX";
            }
            else
            {
                var cost = _shopData.GetCost(_partsId[i], _stutasLevel[i]);
                var lv = _stutasLevel[i];
                weaponCost.text = "Lv�F" + lv.ToString("D2") + "\n�R�X�g�F" + cost.ToString("D4");
            }
        }

        if (Input.GetButtonDown("B"))
        {
            _fade.StartFadeOut("StageSelectScene");
        }

        //�w���{�^���������ꂽ�Ƃ�
        if (Input.GetButtonDown("A"))
        {
            //�I��ł���p�[�c���擾����
            int selectPart = _cursorScript.GetIndex();
            //�I��ł���p�[�c�̃��x�����}�b�N�X����Ȃ����
            if (_stutasLevel[selectPart] < kMaxLevel)
            {
                //�I��ł���p�[�c�̃R�X�g���擾����
                int cost = _shopData.GetCost(_partsId[selectPart], _stutasLevel[selectPart]);
                //�I��ł���p�[�c�̒l�i���������Ă��邨�������������炨�������炷
                if (_userData.SubMoney(cost))
                {
                    //�I�����Ă��镐��p�[�c����������
                    _stutasLevel[selectPart]++;
                    _userData.LvUpWeapon(_partsId[selectPart]);
                }

            }
        }
    }

    private void InitUserData()
    {
        //���x���̓ǂݍ���
        _stutasLevel[(int)CursorMove.UpgradeParts.kNearAtk] = _userData.GetWeaponLv("W_0");
        _stutasLevel[(int)CursorMove.UpgradeParts.kNearSpd] = _userData.GetWeaponLv("W_1");
        _stutasLevel[(int)CursorMove.UpgradeParts.kNearRange] = _userData.GetWeaponLv("W_2");
        _stutasLevel[(int)CursorMove.UpgradeParts.kFarAtk] = _userData.GetWeaponLv("W_3");
        _stutasLevel[(int)CursorMove.UpgradeParts.kFarRate] = _userData.GetWeaponLv("W_4");
        _stutasLevel[(int)CursorMove.UpgradeParts.kFarSpd] = _userData.GetWeaponLv("W_5");
    }

}
