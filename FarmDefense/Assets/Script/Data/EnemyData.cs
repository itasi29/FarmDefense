using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EnemyStatus
{
    public int maxHp;            // �ő�HP
    public int speed;            // �ړ����x
    public int attack;           // �U����
    public int attackInterval;   // �U���Ԋu
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
