using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageRusultData
{
    bool _isCrear;
    int _totalFarmHp;
    int _stageNo;

    public bool IsCrear { get { return _isCrear; } set { _isCrear = value; } }
    public int TotalFarmHp { get { return _totalFarmHp; } set { _totalFarmHp = value; } }
    public int StageNo { get { return _stageNo; } set { _stageNo = value; } }
}
