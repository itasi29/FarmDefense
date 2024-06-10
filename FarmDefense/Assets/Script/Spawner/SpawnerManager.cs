using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    /* �萔 */
    // �����ꏊ�̐�
    [SerializeField] private const int kCreatePosNum = 6;

    // �G�̐�
    // FIXME: �܂��G�̎�ނ������������Ă��Ȃ����߁A�o�����摝�₵�Ă���
    [SerializeField] private const int kEnemyTypeNum = 2;


    /* �ϐ� */
    // �J����
    private CameraControl _camera;
    // �����ꏊ�̎擾
    [SerializeField] private GameObject[] createPos = new GameObject[kCreatePosNum];
    // �v���n�u�f�[�^
    [SerializeField] private GameObject[] enemyPrefab = new GameObject[kEnemyTypeNum];  
    // �X�e�[�W��
    [SerializeField] private int _stageNo;
    // �X�e�[�W�����f�[�^
    private List<WaveData> _waveData;
    // ���݂�wave�ԍ�
    private int _nowWaveNo;
    // �����ԍ�
    private int _createNo;
    // wave�J�n����̌o�߃t���[��
    private int _elapsFrame;
    // �|�����G�̐�
    private int _killedEnemyNum;
    // �������~���邩
    private bool _isStopCreate;
    // �Swave�̓G�𐶐�������
    private bool _isAllCreate;

    void Start()
    {
        _camera = GameObject.Find("Main Camera").GetComponent<CameraControl>();
        GameObject parent = GameObject.Find("CreatePos");
        // �����ꏊ�̎擾
        for (int i = 0; i < kCreatePosNum; ++i)
        {
            createPos[i] = parent.transform.GetChild(i).gameObject;
        }
        // �v���n�u�f�[�^�̎擾
        for (int i = 0; i < kEnemyTypeNum; ++i)
        {
            enemyPrefab[i] = (GameObject)Resources.Load("Enemy/No" + i);
        }

        // �X�e�[�W�f�[�^�̎擾
        _waveData = GameObject.Find("DataManager").GetComponent<DataManager>().Spawner.GetWaveData(_stageNo);

        // �e�p�����[�^�̏�����
        _nowWaveNo = 1;
        _elapsFrame = 0;
        _createNo = 0;
        _killedEnemyNum = 0;
        _isStopCreate = false;
        _isAllCreate = false;
    }

    
    void FixedUpdate()
    {
        if (_isAllCreate) return;

        IsKillAllEnemys();
        Create();
    }

    /// <summary>
    /// �E�����G�̐��𑝂₷
    /// </summary>
    public void AddKilledEnemy()
    {
        ++_killedEnemyNum;
    }

    private void Create()
    {
        // �����I�����Ă���Ή������Ȃ�
        if (_isStopCreate) return;

        // �����t���[���̑���
        ++_elapsFrame;

        // �ꎞ�ϐ�
        WaveData wave = _waveData[_nowWaveNo];
        PatternData pattern = wave.patternDatas[_createNo];

        // �����t���[���𒴂��Ă��Ȃ���ΏI��
        if (_elapsFrame < pattern.createFrame) return;

        // ��ށE�ꏊ�ɍ��킹�Đ���
        CreateEnemy(pattern.enemyNo, pattern.createPosNo);

        // �����������̑���
        ++_createNo;
        // ����wave�̐������𒴂��Ă����
        if (_createNo >= wave.createNum)
        {
            // ������~
            _isStopCreate = true;

            // ���݂�wave��3�ɂȂ��Ă����
            if (_nowWaveNo == 3)
            {
                // �S�Đ����������Ƃ�
                _isAllCreate = true;
            }

            // ���̎��_�Ŋ֐��͏I��������
            return;
        }

        // �����^�C�~���O�ł̐����悤�Ƀt���[�������炵�čċA����
        --_elapsFrame;
        Create();
    }

    /// <summary>
    /// ��ނƏꏊ�ɍ��킹�ēG�𐶐�����
    /// </summary>
    /// <param name="enemyNo">�G�̔ԍ�</param>
    /// <param name="posNo">�����ꏊ�̔ԍ�</param>
    private void CreateEnemy(int enemyNo, int posNo)
    {
        GameObject enemy;
        enemy = Instantiate(enemyPrefab[enemyNo]);

        // TODO: �G�̈ʒu�������ł����炻��ɕύX
        enemy.GetComponent<EnemyBase>().Init(createPos[posNo].transform.position);
        _camera.AddHpBarInfo(enemy);
    }


    /// <summary>
    /// ���݂�wave�̓G��S�ē|����
    /// </summary>
    /// <returns>true : �|���� / false : �|���Ă��Ȃ�</returns>
    private bool IsKillAllEnemys()
    {
        bool isAllKill = false;

        // ����wave�̑S�Ă̓G��|������
        if (_killedEnemyNum >= _waveData[_nowWaveNo].createNum)
        {
            // ���݂�wave���𑝂₷
            ++_nowWaveNo;
            // �e�����p�ϐ��̏�����
            _createNo = 0;
            _elapsFrame = 0;
            _killedEnemyNum = 0;
            _isStopCreate = false;

            // �|�������Ƃ�
            isAllKill = true;
        }

        return isAllKill;
    }

}
