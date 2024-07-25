using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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

    /* 定数 */
    private const float kCursorShakeSpeed = 0.05f;
    private const float kCursorWidth = 16;
    private Vector2[] kCursorPos = 
    {
        new Vector2(-613, 176),
        new Vector2(-613, -66),
        new Vector2(-613, -338),
        new Vector2(355, 176),
        new Vector2(355, -66),
        new Vector2(355, -338)
    };

    private const int kImageWidth = 230;//画像の横幅(仮設定)

    private const int kWaitTime = 15;//長押しでカーソルを動かすときの間隔

    private int _index;
    private bool _isPrePush;
    private bool _isChange; 
    private float _cursorShakeFrame;

    private UpgradeParts _selectPart;//選んでいる強化パーツenum

    private ShopManager _shopManager;//ショップを管理しているスクリプト

    private GameObject _weaponShop;//武器ショップ

    private GameObject _selectImage;//選択している画像
    private GameObject _cursorImage;//カーソルの画像

    private bool _isMoveCursor;//カーソルを動かしたかどうか
    private int _waitTime;//カーソルを動かす間隔

    // Start is called before the first frame update
    void Start()
    {
        _waitTime = 0;
        _isMoveCursor = false;
        _cursorImage = GameObject.Find("cursor");

    }

    // Update is called once per frame
    void Update()
    {
        IndexUpdate();
    }

    private void FixedUpdate()
    {
        _waitTime++;
        if (_waitTime > kWaitTime)
        {
            _isMoveCursor = false;
        }

        _cursorShakeFrame += kCursorShakeSpeed;

        var shakePos = kCursorPos[_index];
        shakePos.x += Mathf.Sin(_cursorShakeFrame) * kCursorWidth;
        _cursorImage.transform.localPosition = shakePos;
    }

    protected void IndexUpdate()
    {
        float x, y;

        x = Move("DPADX", 3, (int)UpgradeParts.kPartsNum, -1);
        y = Move("DPADY", 1, (int)UpgradeParts.kPartsNum, 1);

        if (y == 0.0f && x == 0.0f)
        {
            _isPrePush = false;
        }
    }

    private float Move(string padName, int val, int max, float parm)
    {
        float input = Input.GetAxis(padName);

        if (!_isPrePush)
        {
            bool isPush = false;
            if (input == parm)
            {
                _index = (max + _index - val) % max;
                isPush = true;
                _waitTime = 0;
            }
            else if (input == -parm)
            {
                _index = (_index + val) % max;
                isPush = true;
                _waitTime = 0;
            }

            if (isPush)
            {
                _isPrePush = true;
                _isChange = true;
            }
        }

        return input;
    }

    public int GetIndex() { return _index; }
}
