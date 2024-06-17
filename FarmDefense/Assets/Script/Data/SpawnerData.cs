using System.Collections.Generic;
using UnityEngine;

// �p�^�[���f�[�^
public struct PatternData
{
    public int enemyNo;
    public int createFrame;
    public int createPosNo;
};

public struct WaveData
{
    public int createNum;
    public List<PatternData> patternDatas;
}

// �X�e�[�W�f�[�^
public struct StageData
{
    public int no;
    public float enemyBoostRate;
    public int staticMoney;
    public int dynamicMoney;
    public List<WaveData> waveDatas;
};

public class SpawnerData
{
    // �X�e�[�W�f�[�^
    [SerializeField] private List<StageData> _stageData = new List<StageData>();

    /// <summary>
    /// �X�e�[�W�f�[�^�̓ǂݍ���
    /// </summary>
    public void Load()
    {
        // csv�t�@�C���̓ǂݍ���
        TextAsset stageCsv = Resources.Load("Csv/StageManager") as TextAsset;
        // �f�[�^�ǂݍ���
        StageCSV[] items = CSVSerializer.Deserialize<StageCSV>(stageCsv.text);

        foreach (var item in items)
        {
            /* �e��f�[�^��� */
            StageData data;
            data.no = item.StageNo;
            data.enemyBoostRate = item.EnemyBoostRate;
            data.staticMoney = item.StaticMoney;
            data.dynamicMoney = item.DynamicMoney;
            data.waveDatas = LoadPatter(item.StageName);

            _stageData.Add(data);
        }
    }

    /// <summary>
    /// �p�^�[���f�[�^�̓ǂݍ���
    /// </summary>
    private List<WaveData> LoadPatter(string stageName)
    {
        // �Ԃ��p�̃f�[�^
        List<WaveData> result = new List<WaveData>();

        // csv�t�@�C���̓ǂݍ���
        TextAsset patternCsv = Resources.Load("Csv/" + stageName) as TextAsset;

        PatternCSV[] items = CSVSerializer.Deserialize<PatternCSV>(patternCsv.text);

        int nowWaveNo = 1;
        int createNum = 0;
        WaveData tempWaveData = new WaveData();
        List<PatternData> tempPatternData = new List<PatternData>();
        foreach (var item in items) 
        {
            /* �e��f�[�^��� */
            PatternData data;
            data.enemyNo = item.EnemyNo;
            data.createFrame = (int)(item.CreateSec * 50.0f);
            data.createPosNo = item.CreatePosNo;

            // �E�F�[�u�����������Ă����
            if (nowWaveNo < item.WaveNo)
            {
                // Wave�Ƀf�[�^��ǉ�
                tempWaveData.createNum = createNum;
                tempWaveData.patternDatas = tempPatternData;
                // �{�̂ɒǉ�
                result.Add(tempWaveData);

                // �E�F�[�u��i�߂�
                ++nowWaveNo;
                // �ꎞ�f�[�^�̃��Z�b�g
                createNum = 0;
                tempPatternData = new List<PatternData>();
            }

            // �������̑���
            ++createNum;
            // Pattern�f�[�^�ɒǉ�
            tempPatternData.Add(data);
        }

        // �Ō�̃f�[�^��ǉ�
        tempWaveData.createNum = createNum;
        tempWaveData.patternDatas = tempPatternData;
        // �{�̂ɒǉ�
        result.Add(tempWaveData);

        return result;
    }

    public List<WaveData> GetWaveData(int no)
    {
        return _stageData[no].waveDatas;
    }
}
