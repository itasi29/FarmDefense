using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectManager : SelectManager
{
    enum Kind
    {
        kStage1,
        kStage2, 
        kStage3, 
        kStage4,
        kStage5,
        kStage6,
        kShop,
        kMax
    }

    /* íËêî */
    private const string kStageSelectSceneName = "StageSelectScene";
    private const string kTitleSceneName = "TitleScene";
    private const float kBasePosX = -80;
    private const float kBasePosY = 192;
    private const float kIntervalX = 640;
    private const float kIntervalY = -320;
    private const float kCursorShakeWidth = 32;
    private Vector2[] kCursorPos = new Vector2[]
    {
        new Vector2(kBasePosX                   , kBasePosY             ),
        new Vector2(kBasePosX + kIntervalX      , kBasePosY             ),
        new Vector2(kBasePosX + kIntervalX * 2  , kBasePosY             ),
        new Vector2(kBasePosX                   , kBasePosY + kIntervalY),
        new Vector2(kBasePosX + kIntervalX      , kBasePosY + kIntervalY),
        new Vector2(kBasePosX + kIntervalX * 2  , kBasePosY + kIntervalY)
    };

    private void Update()
    {
        if (_optionSys.IsOpenOption()) return;

        CursorMove((int)Kind.kMax);

        if (Input.GetButtonDown("A"))
        {
            Select();
        }
    }

    private void FixedUpdate()
    {
        if (_optionSys.IsOpenOption()) return;

        SetCursorPos();
        CursorShake(kCursorPos[_index].x, kCursorShakeWidth);
    }

    private void SetCursorPos()
    {
        _cursor.transform.localPosition = kCursorPos[_index];
    }

    protected override bool CursorMove(int max)
    {
        float inputX = Input.GetAxis("DPADX");
        float inputY = Input.GetAxis("DPADY");

        if (!_isPrePush)
        {
            bool isPush = false;
            if (inputY == 1)
            {
                _index = (max + _index - 3) % max;
                isPush = true;
            }
            else if (inputY == -1)
            {
                _index = (_index + 3) % max;
                isPush = true;
            }
            if (inputX == 1)
            {
                _index = (_index + 1) % max;
                isPush = true;
            }
            else if (inputX == -1)
            {
                _index = (max + _index - 1) % max;
                isPush = true;
            }

            if (isPush)
            {
                _isPrePush = true;
                return true;
            }
        }
        else if (inputY == 0 && inputX == 0)
        {
            _isPrePush = false;
        }

        return false;
    }

    protected override void Select()
    {
        
    }
}
