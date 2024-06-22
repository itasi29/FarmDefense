using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{

    /* �e�f�[�^�̃t�@�C���p�X */
    public const string kSpawnerFileName = "Csv/StageMaster";
    public const string kEnemyFileName = "Csv/EnemyMaster";
    public const string kWeaponFileName = "Csv/WeaponMaster";
    public const string kImteFileName = "Csv/ItemMaster";
    public const string kShopFileName = "Csv/ShopMaster";
    public const string kUserFileName = "/Bin/UserData.bin";

    /* �ێ��f�[�^ */
    private SpawnerData _spawner;
    private EnemyData _enemy;
    private WeaponData _weapon;
    private ItemData _item;
    private ShopData _shop;
    private UserData _user;

    void Awake()
    {
        // ���M���j��I�u�W�F�N�g�ɂ���
        DontDestroyOnLoad(this.gameObject);

        // �e��f�[�^�n�𐶐�
        _spawner = new SpawnerData();
        _enemy = new EnemyData();
        _weapon = new WeaponData();
        _item = new ItemData();
        _shop = new ShopData();
        _user = new UserData();

        // �f�[�^�̓ǂݍ���
        _spawner.Load();
        _enemy.Load();
        _weapon.Load();
        _item.Load();
        _shop.Load();
        _user.Load(_weapon.GetIdList(), _item.GetIdList());

        // ��
        _user.Write();
    }

    // �e�v���p�e�B
    public SpawnerData Spawner { get { return _spawner; } }
    public EnemyData Enemy { get { return _enemy; } }
}
