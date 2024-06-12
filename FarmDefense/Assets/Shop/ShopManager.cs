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

        //TODO : 外部ファイルをここで読み込む    関数化します

        //レベルの読み込み
        _stutasLevel[(int)CursorMove.UpgradeParts.kNearAtk] = 1;
        _stutasLevel[(int)CursorMove.UpgradeParts.kNearRange] = 1;
        _stutasLevel[(int)CursorMove.UpgradeParts.kNearSpd] = 1;
        _stutasLevel[(int)CursorMove.UpgradeParts.kFarAtk] = 1;
        _stutasLevel[(int)CursorMove.UpgradeParts.kFarSpd] = 1;
        _stutasLevel[(int)CursorMove.UpgradeParts.kFarRate] = 1;

        //コストの読み込み
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
    }
}
