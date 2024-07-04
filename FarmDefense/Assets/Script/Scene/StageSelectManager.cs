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
    private const string kStageSceneNameBase = "StageScene";
    private const string kTitleSceneName = "TitleScene";
    private const string kShopSceneName = "ShopScene";
    private const float kBasePosX = -192;
    private const float kBasePosY = 216;
    private const float kIntervalX = 384;
    private const float kIntervalY = -256;
    private const float kShopPosX = 352;
    private const float kShopPosY = -244;
    private const float kCursorShakeWidth = 32;

    protected override void Init()
    {
        _maxX = (int)Kind.kMax;
        _maxY = _maxX - 1;
        _valX = 1;
        _valY = 3;
        _isX = true;
        _isY = true;
        _isRot = true;
        _valRot = 2;
        _valDivRot = 3;
        _cursorRot = Quaternion.AngleAxis(180, Vector3.up);
        _cursorWidth = kCursorShakeWidth;
        _cursorPos = new Vector2[]
        {
            new Vector2(kBasePosX                                  , kBasePosY             ),
            new Vector2(kBasePosX + kIntervalX + kCursorShakeWidth , kBasePosY             ),
            new Vector2(kBasePosX + kIntervalX                     , kBasePosY             ),
            new Vector2(kBasePosX                                  , kBasePosY + kIntervalY),
            new Vector2(kBasePosX + kIntervalX + kCursorShakeWidth , kBasePosY + kIntervalY),
            new Vector2(kBasePosX + kIntervalX                     , kBasePosY + kIntervalY),
            new Vector2(kShopPosX , kShopPosY)
        };
    }

    protected override void CursorRot()
    {
        if (!_isRot) return;

        if (_index % _valDivRot == _valRot || _index == (int)Kind.kShop)
        {
            _cursor.transform.localRotation = _cursorRot;
        }
        else
        {
            _cursor.transform.localRotation = Quaternion.identity;
        }
    }

    protected override void Select()
    {
        // TODO: Ç”Ç•Å[Ç«ÇµÇƒÇ©ÇÁ
        if (_index == (int)Kind.kShop)
        {
            SceneManager.LoadScene(kShopSceneName);
        }
        else
        {
            SceneManager.LoadScene(kStageSceneNameBase + (_index + 1).ToString());
        }
    }

    protected override void Cancel()
    {
        SceneManager.LoadScene(kTitleSceneName);
    }
}
