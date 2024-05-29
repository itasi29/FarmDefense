using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

public class SpawnerData : MonoBehaviour
{
    // �X�e�[�W�f�[�^
    [SerializeField] private List<StageData> _stageData;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("���[�h!");
        LoadStage();
    }

    /// <summary>
    /// �X�e�[�W�f�[�^�̓ǂݍ���
    /// </summary>
    private void LoadStage()
    {
        // csv�t�@�C���̓ǂݍ���
        TextAsset stageCsv = Resources.Load("StageManager") as TextAsset;
        // �ϊ�
        StringReader reader = new StringReader(stageCsv.text);

        // TODO:��ԏ�̃f�[�^����v�f�ԍ����擾
        // ��s�ǂݍ���ŁA','�ŋ�؂�
        var lineSplit = reader.ReadLine().Split(',');
        int idxStageNo = 0;
        int idxStageName = 1;
        int idxEnemyBoostRate = 2;
        int idxStaticMoney = 3;
        int idxDynamicMoney = 4;

        // �S�f�[�^�ǂݍ���
        while (reader.Peek() != -1)
        {
            // ��s�ǂݍ���ŁA','�ŋ�؂�
            lineSplit = reader.ReadLine().Split(',');

            /* �e��f�[�^�擾 */
            int no               = int.Parse(lineSplit[idxStageNo]);
            string name          = lineSplit[idxStageName];
            float enemyBoostRate = float.Parse(lineSplit[idxEnemyBoostRate]);
            int staticMoney      = int.Parse(lineSplit[idxStaticMoney]);
            int dynamicMoeny     = int.Parse(lineSplit[idxDynamicMoney]);

            /* �e��f�[�^�̑�� */
            StageData data;
            data.no = no;
            data.enemyBoostRate = enemyBoostRate;
            data.staticMoney = staticMoney;
            data.dynamicMoney = dynamicMoeny;
            data.waveDatas = LoadPatter(name);

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
        TextAsset patternCsv = Resources.Load(stageName) as TextAsset;
        // �ϊ�
        StringReader reader = new StringReader(patternCsv.text);

        // TODO:��ԏ�̃f�[�^����v�f�ԍ����擾
        // ��s�ǂݍ���ŁA','�ŋ�؂�
        var lineSplit = reader.ReadLine().Split(',');
        int idxWaveNo = 0;
        int idxEnemyNo = 1;
        int idxCreateSec = 2;
        int idxCreatePosNo = 3;

        // ���݊m�F����waveNo
        int nowCheckWaveNo = 1;
        // �G������
        int createNum = 0;
        // �f�[�^�ꎞ�ۑ��p
        WaveData itemWave = new WaveData();
        List<PatternData> itemPattern = new List<PatternData>();

        int num = 0;

        // �S�f�[�^�ǂݍ���
        while (reader.Peek() != -1) 
        {
            // ��s�ǂݍ���ŁA','�ŋ�؂�
            lineSplit = reader.ReadLine().Split(',');

            /* �e��f�[�^�擾 */
            int waveNo      = int.Parse(lineSplit[idxWaveNo]);
            int enemyNo     = int.Parse(lineSplit[idxEnemyNo]);
            float createSec = float.Parse(lineSplit[idxCreateSec]);
            int createPosNo = int.Parse(lineSplit[idxCreatePosNo]);

            /* �p�^�[���f�[�^�ɑ�� */
            PatternData data;
            data.enemyNo = enemyNo;
            data.createFrame = (int)(50.0f * createSec); // ���Ԃ���t���[���ɕϊ�
            data.createPosNo = createPosNo;

            // �E�F�[�u��������������
            if (nowCheckWaveNo < waveNo)
            {
                Debug.Log("�{�f�[�^�ǉ�");
                // �{�̃f�[�^�ɒǉ�
                itemWave.patternDatas = itemPattern;
                itemWave.createNum = createNum;
                result.Add(itemWave);
                // �ꎞ�f�[�^�̏�����
                itemPattern = new List<PatternData>();
                createNum = 0;
                // ���݃E�F�[�u���̑���
                nowCheckWaveNo++;
            }

            Debug.Log("�ǉ� : " + num);
            // �f�[�^�ǉ�
            itemPattern.Add(data);
            // ����������
            createNum++;

            num++;
        }

        // �Ō�̃E�F�[�u�f�[�^��ǉ�
        itemWave.patternDatas = itemPattern;
        itemWave.createNum = createNum;
        result.Add(itemWave);

        return result;
    }

    public List<WaveData> GetWaveData(int no)
    {
        return _stageData[no].waveDatas;
    }
}
