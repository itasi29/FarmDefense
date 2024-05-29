using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    /* �ϐ� */
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
        // �X�e�[�W�f�[�^�̎擾
        _waveData = GameObject.Find("DataManager").GetComponent<SpawnerData>().GetWaveData(_stageNo);

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
        
        // TODD:�G�̎�ނɍ��킹�Ă̐���

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
