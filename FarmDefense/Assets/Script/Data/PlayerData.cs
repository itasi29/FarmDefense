using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerStatus
{
    public int maxHp;
    public int maxStamina;
    public int recoveryStamina;
    public float speed;
    public float dashSpeed;
    public float downSpeed;
    public float jumpPower;
    public float fallPower;
};

public struct StaminaCost
{
    public int dash;
    public int strongAttack;
};

public struct PlayerTime
{
    public int stan;
    public int hitSafe;
    public int revivalSafe;
};

public class PlayerData
{
    private PlayerStatus _status = new PlayerStatus();
    private StaminaCost _cost = new StaminaCost();
    private PlayerTime _time = new PlayerTime();

    public PlayerStatus Status {  get { return _status; } }
    public StaminaCost Cost { get {  return _cost; } }
    public PlayerTime Time { get { return _time; } }

    public void Load()
    {
        // csvファイルの読み込み
        TextAsset csv = Resources.Load(DataManager.kPlayerFileName) as TextAsset;
        // データ読み込み
        PlayerCSV[] items = CSVSerializer.Deserialize<PlayerCSV>(csv.text);

        _status.maxHp = items[0].MaxHp;
        _status.maxStamina = items[0].MaxStamina;
        _status.recoveryStamina = items[0].RecoveryStamina;
        _status.speed = items[0].Speed;
        _status.dashSpeed = items[0].DashSpeed;
        _status.downSpeed = items[0].DownSpeed;
        _status.jumpPower = items[0].JumpPower;
        _status.fallPower = items[0].FallPower;
        _cost.dash = items[0].DashCost;
        _cost.strongAttack = items[0].StrongAttackCost;
        _time.stan = items[0].StanTime;
        _time.hitSafe = items[0].HitSafeTime;
        _time.revivalSafe = items[0].RevivalSafeTime;
    }
}
