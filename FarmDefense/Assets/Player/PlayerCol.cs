using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//‚ ‚½‚Á‚½‚Æ‚«‚Ì‚µ‚å‚è‚Â‚­‚é‚æ
public class PlayerCol
{
    private BoxCollider _GroundCheckCollider;

    public void SetGroundCheckCollision(BoxCollider col)
    {
        _GroundCheckCollider = col;
    }


}
