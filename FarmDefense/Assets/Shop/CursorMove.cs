using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorMove : MonoBehaviour
{

    public enum UpgradeParts
    {
        kNearAtk,
        kNearRange,
        kNearSpd,
        kFarAtk,
        kFarSpd,
        kFarRate,
        kPartsNum
    }


    private const int kImageWidth = 110;

    private const int kWaitTime = 15;

    private Vector2 _cursorIndex;

    private UpgradeParts _selectPart;

    private GameObject _selectImage;
    private GameObject _cursorImage;

    private bool _isMoveCursor;
    private int _waitTime;
    // Start is called before the first frame update
    void Start()
    {
        _waitTime = 0;
        _isMoveCursor = false;
        _cursorIndex = new Vector2(0, 0);
        _cursorImage = GameObject.Find("cursor");
        _selectImage = GameObject.Find("nearAtk");
        _selectPart = UpgradeParts.kNearAtk;
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
                _selectImage = GameObject.Find("nearAtk");
                _selectPart = UpgradeParts.kNearAtk;
            }
            else if (_cursorIndex.y == 1)
            {
                _selectImage = GameObject.Find("nearRange");
                _selectPart = UpgradeParts.kNearRange;
            }
            else if (_cursorIndex.y == 2)
            {
                _selectImage = GameObject.Find("nearSpd");
                _selectPart = UpgradeParts.kNearSpd;
            }
        }
        else
        {
            if (_cursorIndex.y == 0)
            {
                _selectImage = GameObject.Find("farAtk");
                _selectPart = UpgradeParts.kFarAtk;
            }
            else if (_cursorIndex.y == 1)
            {
                _selectImage = GameObject.Find("farSpd");
                _selectPart = UpgradeParts.kFarSpd;
            }
            else if (_cursorIndex.y == 2)
            {
                _selectImage = GameObject.Find("farRate");
                _selectPart = UpgradeParts.kFarRate;
            }
        }
        cursorPos = _selectImage.transform.position;
        cursorPos.x = _selectImage.transform.position.x + kImageWidth;

        _cursorImage.transform.position = cursorPos;
    }

    private void FixedUpdate()
    {
        _waitTime++;
        if(_waitTime > kWaitTime)
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
            if(isHit)
            {
                _isMoveCursor = true;
                _waitTime = 0;
            }
        }

        //何も入力がなかったら次のフレームから入力できるようになる
        if (Input.GetAxis("DPADX") == 0 && Input.GetAxis("DPADY") == 0)
        {
            _isMoveCursor = false;
        }
    }

    public UpgradeParts GetSelectPart() { return _selectPart; }
}
