using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class SelectManager : MonoBehaviour
{
    /* íËêî */
    private const float kCursorShakeSpeed = 0.05f;

    /* ïœêî */
    protected int _index;
    protected int _maxX = 0;
    protected int _maxY = 0;
    protected int _valX = 0;
    protected int _valY = 0;
    protected int _valRot = 0;
    protected int _valDivRot = 0;
    protected bool _isX = false;
    protected bool _isY = false;
    protected bool _isRot;
    protected bool _isChange;
    protected bool _isPrePush;
    protected Quaternion _cursorRot;
    protected float _cursorWidth;
    protected Vector2[] _cursorPos;
    protected OptionSystem _optionSys;

    private float _cursorShakeFrame;
    [SerializeField] protected GameObject _cursor;

    protected virtual void Start()
    {
        _index = 0;
        _isPrePush = false;
        _isChange  = false;
        _optionSys = new OptionSystem();
        Init();
    }

    protected virtual void Update()
    {
        if (_optionSys.IsOpenOption()) return;

        IndexUpdate();

        if (Input.GetButtonDown("A"))
        {
            Select();
        }
        if (Input.GetButtonDown("B"))
        {
            Cancel();
        }
    }

    protected virtual void FixedUpdate()
    {
        if (_optionSys.IsOpenOption()) return;

        CursorUpdate();
    }

    protected abstract void Init();
    protected abstract void Select();
    protected virtual void Cancel() {}

    private void CursorUpdate()
    {
        _cursorShakeFrame += kCursorShakeSpeed;

        CursorRot();

        var shakePos = _cursorPos[_index];
        shakePos.x += Mathf.Sin(_cursorShakeFrame) * _cursorWidth;
        _cursor.transform.localPosition = shakePos;
    }

    protected virtual void CursorRot()
    {
        if (!_isRot) return;

        if (_index % _valDivRot == _valRot)
        {
            _cursor.transform.localRotation = _cursorRot;
        }
        else
        {
            _cursor.transform.localRotation = Quaternion.identity;
        }
    }

    protected void IndexUpdate()
    {
        float x, y;
        x = y = 0.0f;

        if (_isX)
        {
            x = Move("DPADX", _valX, _maxX, -1);
        }
        if (_isY)
        {
            y = Move("DPADY", _valY, _maxY, 1);
        }

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
            }
            else if (input == -parm)
            {
                _index = (_index + val) % max;
                isPush = true;
            }

            if (isPush)
            {
                _isPrePush = true;
                _isChange = true;
            }
        }

        return input;
    }
}
