using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSelectManager : SelectManager
{
    enum Kind
    {
        kMax
    }

    private const float kInitPosY = 232;
    private const float kIntervalPosY = 232;
    private const float kCursorPosx = 64;
    private const float kCursorShakeWidth = 48;

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

        SetCursorPos(kInitPosY, kIntervalPosY);
        CursorShake(kCursorPosx, kCursorShakeWidth);
    }

    protected override void Select()
    {
        
    }
}
