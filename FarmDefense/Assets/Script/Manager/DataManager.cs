using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{

    /* �e�f�[�^�̃t�@�C���p�X */
    public const string kSpawnerFileName = "Csv/StageMaster";
    public const string kPlayerFileName = "Csv/PlayerMaster";
    public const string kEnemyFileName = "Csv/EnemyMaster";
    public const string kWeaponFileName = "Csv/WeaponMaster";
    public const string kImteFileName = "Csv/ItemMaster";
    public const string kShopFileName = "Csv/ShopMaster";
    public const string kUserFileName = "/Bin/UserData.bin";

    /* �ێ��f�[�^ */
    private SpawnerData _spawner;
    private PlayerData _player;
    private EnemyData _enemy;
    private WeaponData _weapon;
    private ItemData _item;
    private ShopData _shop;
    private UserData _user;

    private void Awake()
    {
        // ���M���j��I�u�W�F�N�g�ɂ���
        DontDestroyOnLoad(this.gameObject);

        // �e��f�[�^�n�𐶐�
        _spawner = new SpawnerData();
        _player = new PlayerData();
        _enemy = new EnemyData();
        _weapon = new WeaponData();
        _item = new ItemData();
        _shop = new ShopData();
        _user = new UserData();

        // �f�[�^�̓ǂݍ���
        _spawner.Load();
        _player.Load();
        _enemy.Load();
        _weapon.Load();
        _item.Load();
        _shop.Load();
        _user.Load(_weapon.GetIdList(), _item.GetIdList());
    }

    private void OnApplicationQuit()
    {
        Debug.Log("�Ă΂�Ă�H");
        // �f�[�^�̕ۑ�
        _user.Save();
    }

    // �e�v���p�e�B
    public SpawnerData Spawner { get { return _spawner; } }
    public PlayerData Player { get { return _player; } }
    public EnemyData Enemy { get { return _enemy; } }
    public WeaponData Weapon { get {  return _weapon; } }
    public ItemData Item { get { return _item;} }
    public ShopData Shop { get { return _shop; } }
    public UserData User { get { return _user; } }
}
