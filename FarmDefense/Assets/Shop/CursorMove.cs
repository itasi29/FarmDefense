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

    private const int kImageWidth = 230;//�摜�̉���(���ݒ�)

    private const int kWaitTime = 15;//�������ŃJ�[�\���𓮂����Ƃ��̊Ԋu

    private Vector2 _cursorIndex;//�J�[�\���̂���ʒu

    private UpgradeParts _selectPart;//�I��ł��鋭���p�[�cenum

    private Item _selectItem;//�I��ł���A�C�e��enum

    private ShopManager _shopManager;//�V���b�v���Ǘ����Ă���X�N���v�g

    private GameObject _weaponShop;//����V���b�v
    private GameObject _itemShop;//�A�C�e���V���b�v

    private GameObject _selectImage;//�I�����Ă���摜
    private GameObject _cursorImage;//�J�[�\���̉摜

    private bool _isMoveCursor;//�J�[�\���𓮂��������ǂ���
    private int _waitTime;//�J�[�\���𓮂����Ԋu

    private bool _lastShowWeaponShop;//�O�̃t���[���J���Ă����V���b�v

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

        //�O�̃t���[���J���Ă����V���b�v�ƈႤ�V���b�v���J���Ă�����
        if (_lastShowWeaponShop != _shopManager.IsGetWeaponShop())
        {
            //�J�[�\���̈ʒu�������ʒu�ɖ߂�
            _cursorIndex = new Vector2(0, 0);
            _lastShowWeaponShop = _shopManager.IsGetWeaponShop();
        }

        //�����p�[�c�V���b�v���J���Ă���ꍇ
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
        //�A�C�e���V���b�v���J���Ă���ꍇ
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
        //�摜�̈ʒu�ɃJ�[�\�����ړ�
        cursorPos = _selectImage.transform.position;
        //�摜�̉����̑傫�������J�[�\�����E�ɂ��炷
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

        //�������͂��Ȃ������玟�̃t���[��������͂ł���悤�ɂȂ�
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
