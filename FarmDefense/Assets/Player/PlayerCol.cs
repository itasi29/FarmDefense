using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//���������Ƃ��̂��������
public class PlayerCol
{
    private BoxCollider _GroundCheckCollider;

    public void SetGroundCheckCollision(BoxCollider col)
    {
        _GroundCheckCollider = col;
    }


}
