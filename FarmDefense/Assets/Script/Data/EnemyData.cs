using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EnemyStatus
{
    public int maxHp;            // �ő�HP
    public float speed;            // �ړ����x
    public int attack;           // �U����
    public int attackInterval;   // �U���Ԋu
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
