using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{

    /* 各データのファイルパス */
    public const string kSpawnerFileName = "Csv/StageMaster";
    public const string kEnemyFileName = "Csv/EnemyMaster";
    public const string kWeaponFileName = "Csv/WeaponMaster";
    public const string kImteFileName = "Csv/ItemMaster";
    public const string kShopFileName = "Csv/ShopMaster";
    public const string kUserFileName = "/Bin/UserData.bin";

    /* 保持データ */
    private SpawnerData _spawner;
    private EnemyData _enemy;
    private WeaponData _weapon;
    private ItemData _item;
    private ShopData _shop;
    private UserData _user;

    private void Awake()
    {
        // 自信を非破壊オブジェクトにする
        DontDestroyOnLoad(this.gameObject);

        // 各種データ系を生成
        _spawner = new SpawnerData();
        _enemy = new EnemyData();
        _weapon = new WeaponData();
        _item = new ItemData();
        _shop = new ShopData();
        _user = new UserData();

        // データの読み込み
        _spawner.Load();
        _enemy.Load();
        _weapon.Load();
        _item.Load();
        _shop.Load();
        _user.Load(_weapon.GetIdList(), _item.GetIdList());
    }

    private void OnApplicationQuit()
    {
        // データの保存
        _user.Save();
    }

    // 各プロパティ
    public SpawnerData Spawner { get { return _spawner; } }
    public EnemyData Enemy { get { return _enemy; } }
    public WeaponData Weapon { get {  return _weapon; } }
    public ItemData Item { get { return _item;} }
    public ShopData Shop { get { return _shop; } }
    public UserData User { get { return _user; } }
}
