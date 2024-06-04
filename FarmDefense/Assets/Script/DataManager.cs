/* �f�[�^�n���ꊇ�ɊǗ����� */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private SpawnerData _spawner;

    void Start()
    {
        // ���M���j��I�u�W�F�N�g�ɂ���
        DontDestroyOnLoad(this.gameObject);

        // �e��f�[�^�n�𐶐�
        _spawner = new SpawnerData();

        // �f�[�^�̓ǂݍ���
        _spawner.Load();
    }

    // �e�v���p�e�B
    public SpawnerData Spawner { get { return _spawner; } }
}
