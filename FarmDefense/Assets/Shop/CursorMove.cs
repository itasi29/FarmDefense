using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cursorMove : MonoBehaviour
{
    private const int kImageWidth = 110;

    private const int kWaitTime = 15;



    private Vector2 _cursorIndex;

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
            }
            else if (_cursorIndex.y == 1)
            {
                _selectImage = GameObject.Find("nearRange");
            }
            else if (_cursorIndex.y == 2)
            {
                _selectImage = GameObject.Find("nearSpd");
            }
        }
        else
        {
            if (_cursorIndex.y == 0)
            {
                _selectImage = GameObject.Find("farAtk");
            }
            else if (_cursorIndex.y == 1)
            {
                _selectImage = GameObject.Find("farSpd");
            }
            else if (_cursorIndex.y == 2)
            {
                _selectImage = GameObject.Find("farRate");
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

        //‰½‚à“ü—Í‚ª‚È‚©‚Á‚½‚çŽŸ‚ÌƒtƒŒ[ƒ€‚©‚ç“ü—Í‚Å‚«‚é‚æ‚¤‚É‚È‚é
        if (Input.GetAxis("DPADX") == 0 && Input.GetAxis("DPADY") == 0)
        {
            _isMoveCursor = false;
        }
    }
}
