using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    private const int kMaxLevel = 10;

    private GameObject _cursor;
    private CursorMove _cursorScript;

    private GameObject _costUi;

    int[] _stutasLevel = new int[(int)CursorMove.UpgradeParts.kPartsNum];



    private int[] _partsCost = new int[kMaxLevel];

    private int _hasGold;

    // Start is called before the first frame update
    void Start()
    {
        _hasGold = 10000;

        _costUi = GameObject.Find("Cost");

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

        //�R�X�g�̓ǂݍ���
        for (int i = 0; i < kMaxLevel; i++)
        {
            _partsCost[i] = (i) * 100;
        }
    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < (int)CursorMove.UpgradeParts.kPartsNum; i++)
        {
            GameObject a = _costUi.transform.GetChild(i).gameObject;
            
        }

        if (Input.GetButtonDown("A"))
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
    }
}
