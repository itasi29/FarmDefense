using System.Collections.Generic;
using UnityEngine;

// パターンデータ
public struct PatternData
{
    public string enemyNo;
    public int createFrame;
    public int createPosNo;
};

public struct WaveData
{
    public int createNum;
    public List<PatternData> patternDatas;
}

// ステージデータ
public struct StageData
{
    public float enemyBoostRate;
    public int staticMoney;
    public int dynamicMoney;
    public List<WaveData> waveDatas;
};

public class SpawnerData
{
    // ステージデータ
    private Dictionary<int, StageData> _data = new Dictionary<int, StageData>();

    /// <summary>
    /// ステージデータの読み込み
    /// </summary>
    public void Load()
    {
        // csvファイルの読み込み
        TextAsset stageCsv = Resources.Load(DataManager.kSpawnerFileName) as TextAsset;
        // データ読み込み
        StageCSV[] items = CSVSerializer.Deserialize<StageCSV>(stageCsv.text);

        foreach (var item in items)
        {
            /* 各種データ代入 */
            StageData data = new StageData();
            data.enemyBoostRate = item.EnemyBoostRate;
            data.staticMoney = item.StaticMoney;
            data.dynamicMoney = item.DynamicMoney;
            data.waveDatas = LoadPatter(item.StageName);

            _data[item.StageNo] = data;
        }
    }

    /// <summary>
    /// パターンデータの読み込み
    /// </summary>
    private List<WaveData> LoadPatter(string stageName)
    {
        // 返す用のデータ
        List<WaveData> result = new List<WaveData>();

        // csvファイルの読み込み
        TextAsset patternCsv = Resources.Load("Csv/" + stageName) as TextAsset;

        PatternCSV[] items = CSVSerializer.Deserialize<PatternCSV>(patternCsv.text);

        int nowWaveNo = 1;
        int createNum = 0;
        WaveData tempWaveData = new WaveData();
        List<PatternData> tempPatternData = new List<PatternData>();
        foreach (var item in items) 
        {
            /* 各種データ代入 */
            PatternData data;
            data.enemyNo = item.EnemyNo;
            data.createFrame = (int)(item.CreateSec * 50.0f);
            data.createPosNo = item.CreatePosNo;

            // ウェーブ数が増加していれば
            if (nowWaveNo < item.WaveNo)
            {
                // Waveにデータを追加
                tempWaveData.createNum = createNum;
                tempWaveData.patternDatas = tempPatternData;
                // 本体に追加
                result.Add(tempWaveData);

                // ウェーブを進める
                ++nowWaveNo;
                // 一時データのリセット
                createNum = 0;
                tempPatternData = new List<PatternData>();
            }

            // 生成数の増加
            ++createNum;
            // Patternデータに追加
            tempPatternData.Add(data);
        }

        // 最後のデータを追加
        tempWaveData.createNum = createNum;
        tempWaveData.patternDatas = tempPatternData;
        // 本体に追加
        result.Add(tempWaveData);

        return result;
    }

    public List<WaveData> GetWaveData(int no)
    {
        return _data[no].waveDatas;
    }

    public int GetStaticMoney(int no)
    {
        return _data[no].staticMoney;
    }

    public int GetDynamicMoney(int no)
    {
        return _data[no].dynamicMoney;
    }
}
