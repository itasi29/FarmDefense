using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EnemyStatus
{
    public int maxHp;            // Å‘åHP
    public int speed;            // ˆÚ“®‘¬“x
    public int attack;           // UŒ‚—Í
    public int attackInterval;   // UŒ‚ŠÔŠu
};

public class EnemyData
{
    private EnemyStatus _status;

    public void Load()
    {

    }

    public EnemyStatus GetStatus(int no)
    {
        return _status;
    }
}
