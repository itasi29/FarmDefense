using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//あたったときのしょりつくるよ
public class PlayerCol
{
    private BoxCollider _GroundCheckCollider;

    public void SetGroundCheckCollision(BoxCollider col)
    {
        _GroundCheckCollider = col;
    }


}
