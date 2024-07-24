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

    private const int kImageWidth = 230;//�摜�̉���(���ݒ�)

    private const int kWaitTime = 15;//�������ŃJ�[�\���𓮂����Ƃ��̊Ԋu

    private Vector2 _cursorIndex;//�J�[�\���̂���ʒu

    private UpgradeParts _selectPart;//�I��ł��鋭���p�[�cenum

    private ShopManager _shopManager;//�V���b�v���Ǘ����Ă���X�N���v�g

    private GameObject _weaponShop;//����V���b�v

    private GameObject _selectImage;//�I�����Ă���摜
    private GameObject _cursorImage;//�J�[�\���̉摜

    private bool _isMoveCursor;//�J�[�\���𓮂��������ǂ���
    private int _waitTime;//�J�[�\���𓮂����Ԋu

    // Start is called before the first frame update
    void Start()
    {
        _waitTime = 0;
        _isMoveCursor = false;
        _cursorIndex = new Vector2(0, 0);
        _cursorImage = GameObject.Find("cursor");
        _selectImage = GameObject.Find("nearAtk");
        _selectPart = UpgradeParts.kNearAtk;
        _shopManager = GameObject.Find("Manager").GetComponent<ShopManager>();

        _weaponShop = GameObject.Find("WeaponShop");

    }

    // Update is called once per frame
    void Update()
    {
        MoveCursor();

        Vector2 cursorPos = new Vector2(0, 0);

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
}
