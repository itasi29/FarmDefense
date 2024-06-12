using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private SpawnerData _spawner;
    private EnemyData _enemy;

    void Start()
    {
        // ���M���j��I�u�W�F�N�g�ɂ���
        DontDestroyOnLoad(this.gameObject);

        // �e��f�[�^�n�𐶐�
        _spawner = new SpawnerData();
        _enemy = new EnemyData();

        // �f�[�^�̓ǂݍ���
        _spawner.Load();
        _enemy.Load();
    }

    // �e�v���p�e�B
    public SpawnerData Spawner { get { return _spawner; } }
    public EnemyData Enemy { get { return _enemy; } }
}
