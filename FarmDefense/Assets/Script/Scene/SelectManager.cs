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
    protected int _max;
    protected int _valX;
    protected int _valY;
    protected int _valRot;
    protected int _valDivRot;
    protected bool _isX;
    protected bool _isY;
    protected bool _isRot;
    protected bool _isChange;
    protected bool _isPrePush;
    protected Quaternion _cursorRot;
    protected float _cursorWidth;
    protected Vector2[] _cursorPos;
    protected OptionSystem _optionSys;

    private float _cursorShakeFrame;
    [SerializeField] private GameObject _cursor;

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

        if (_isRot)
        {
            if (_index % _valDivRot == _valRot)
            {
                _cursor.transform.localRotation = _cursorRot;
            }
            else
            {
                _cursor.transform.localRotation = Quaternion.identity;
            }
        }

        var shakePos = _cursorPos[_index];
        shakePos.x += Mathf.Sin(_cursorShakeFrame) * _cursorWidth;
        _cursor.transform.localPosition = shakePos;
    }

    protected void IndexUpdate()
    {
        float x, y;
        x = y = 0.0f;

        if (_isX)
        {
            x = Move("DPADX", _valX, -1);
        }
        if (_isY)
        {
            y = Move("DPADY", _valY, 1);
        }

        if (y == 0.0f && x == 0.0f)
        {
            _isPrePush = false;
        }
    }

    private float Move(string padName, int val, float parm)
    {
        float input = Input.GetAxis(padName);

        if (!_isPrePush)
        {
            bool isPush = false;
            if (input == parm)
            {
                _index = (_max + _index - val) % _max;
                isPush = true;
            }
            else if (input == -parm)
            {
                _index = (_index + val) % _max;
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
