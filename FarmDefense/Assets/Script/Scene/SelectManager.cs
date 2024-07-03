using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SelectManager : MonoBehaviour
{
    private const float kCursorShakeSpeed = 0.05f;

    protected int _index;
    private float _cursorShakeFrame;
    protected bool _isPrePush;
    [SerializeField] protected GameObject _cursor;
    protected OptionSystem _optionSys;

    protected virtual void Start()
    {
        _index = 0;
        _isPrePush = false;
        _optionSys = new OptionSystem();
    }

    protected virtual void SetCursorPos(float initY, float intervalY)
    {
        var pos = _cursor.transform.localPosition;
        pos.y = initY - (_index * intervalY);
        _cursor.transform.localPosition = pos;
    }

    protected void CursorShake(float centerX, float width)
    {
        _cursorShakeFrame += kCursorShakeSpeed;

        var pos = _cursor.transform.localPosition;
        pos.x = centerX + Mathf.Sin(_cursorShakeFrame) * width;
        _cursor.transform.localPosition = pos;
    }

    protected virtual bool CursorMove(int max)
    {
        float input = Input.GetAxis("DPADY");

        if (!_isPrePush)
        {
            bool isPush = false;
            if (input == 1)
            {
                _index = (max + _index - 1) % max;
                isPush = true;
            }
            else if (input == -1)
            {
                _index = (_index + 1) % max;
                isPush = true;
            }

            if (isPush)
            {
                _isPrePush = true;
                return true;
            }
        }
        else if (input == 0)
        {
            _isPrePush = false;
        }

        return false;
    }

    protected abstract void Select();
}
