using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EnemyStatus
{
    public int maxHp;            // ÅåHP
    public int speed;            // Ú®¬x
    public int attack;           // UÍ
    public int attackInterval;   // UÔu
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
