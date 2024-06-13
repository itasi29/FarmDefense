using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EnemyStatus
{
    public int maxHp;            // Å‘åHP
    public float speed;            // ˆÚ“®‘¬“x
    public int attack;           // UŒ‚—Í
    public int attackInterval;   // UŒ‚ŠÔŠu
};

public class EnemyData
{
    private EnemyStatus _status;

    public void Load()
    {
        _status = new EnemyStatus();
        _status.maxHp = 100;
        _status.speed = 0.2f;
        _status.attack = 2;
        _status.attackInterval = 60;
    }

    public EnemyStatus GetStatus(int no)
    {
        return _status;
    }
}
