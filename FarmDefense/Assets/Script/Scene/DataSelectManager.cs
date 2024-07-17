using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataSelectManager : SelectManager
{
    enum Kind
    {
        Data1,
        Data2,
        Data3,
        Data4,
        kMax
    }

    /* íËêî */
    private const string kStageSelectSceneName = "StageSelectScene";
    private const string kTitleSceneName       = "TitleScene";
    private const float kBasePosX = -80;
    private const float kBasePosY = 192;
    private const float kIntervalX = 640;
    private const float kIntervalY = -320;
    private const float kCursorShakeWidth = 32;

    /* ïœêî */
    private UserData _user;

    private new void Start()
    {
        base.Start();
        _user = GameObject.Find("GameDirector").GetComponent<GameDirector>().DataMgr.User;
    }

    protected override void Init()
    {
        _maxX = _maxY = (int)Kind.kMax;
        _valX = 1;
        _valY = 2;
        _isX = true;
        _isY = true;
        _cursorWidth = kCursorShakeWidth;
        _cursorPos = new Vector2[]
        {
            new Vector2(kBasePosX               , kBasePosY             ),
            new Vector2(kBasePosX + kIntervalX  , kBasePosY             ),
            new Vector2(kBasePosX               , kBasePosY + kIntervalY),
            new Vector2(kBasePosX + kIntervalX  , kBasePosY + kIntervalY)
        };
    }

    protected override void Select()
    {
        _user.SetUserNo(_index);
        _fade.StartFadeOut(kStageSelectSceneName);
    }

    protected override void Cancel()
    {
        _fade.StartFadeOut(kTitleSceneName);
    }
}
