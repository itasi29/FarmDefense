using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

// パターンデータ
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

// ステージデータ
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
    // ステージデータ
    [SerializeField] private List<StageData> _stageData = new List<StageData>();

    /// <summary>
    /// ステージデータの読み込み
    /// </summary>
    public void Load()
    {
        // csvファイルの読み込み
        TextAsset stageCsv = Resources.Load("Csv/StageManager") as TextAsset;
#if true
        // データ読み込み
        StageCSV[] items = CSVSerializer.Deserialize<StageCSV>(stageCsv.text);

        foreach (var item in items)
        {
            /* 各種データ代入 */
            StageData data;
            data.no = item.StageNo;
            data.enemyBoostRate = item.EnemyBoostRate;
            data.staticMoney = item.StaticMoney;
            data.dynamicMoney = item.DynamicMoney;
            data.waveDatas = LoadPatter(item.StageName);

            _stageData.Add(data);
        }

#else
        // 変換
        StringReader reader = new StringReader(stageCsv.text);

        // TODO:一番上のデータから要素番号を取得
        // 一行読み込んで、','で区切る
        var lineSplit = reader.ReadLine().Split(',');
        int idxStageNo = 0;
        int idxStageName = 1;
        int idxEnemyBoostRate = 2;
        int idxStaticMoney = 3;
        int idxDynamicMoney = 4;

        // 全データ読み込み
        while (reader.Peek() != -1)
        {
            // 一行読み込んで、','で区切る
            lineSplit = reader.ReadLine().Split(',');

            /* 各種データ取得 */
            int no               = int.Parse(lineSplit[idxStageNo]);
            string name          = lineSplit[idxStageName];
            float enemyBoostRate = float.Parse(lineSplit[idxEnemyBoostRate]);
            int staticMoney      = int.Parse(lineSplit[idxStaticMoney]);
            int dynamicMoeny     = int.Parse(lineSplit[idxDynamicMoney]);

            /* 各種データの代入 */
            StageData data;
            data.no = no;
            data.enemyBoostRate = enemyBoostRate;
            data.staticMoney = staticMoney;
            data.dynamicMoney = dynamicMoeny;
            data.waveDatas = LoadPatter(name);

            _stageData.Add(data);
        }
#endif
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

#if true
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
                tempPatternData.Clear();
                // 上がバグったら下のに
//                tempPatternData = new List<PatternData>();
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
#else
        // 変換
        StringReader reader = new StringReader(patternCsv.text);

        // TODO:一番上のデータから要素番号を取得
        // 一行読み込んで、','で区切る
        var lineSplit = reader.ReadLine().Split(',');
        int idxWaveNo = 0;
        int idxEnemyNo = 1;
        int idxCreateSec = 2;
        int idxCreatePosNo = 3;

        // 現在確認中のwaveNo
        int nowCheckWaveNo = 1;
        // 敵生成数
        int createNum = 0;
        // データ一時保存用
        WaveData itemWave = new WaveData();
        List<PatternData> itemPattern = new List<PatternData>();

        // 全データ読み込み
        while (reader.Peek() != -1) 
        {
            // 一行読み込んで、','で区切る
            lineSplit = reader.ReadLine().Split(',');

            /* 各種データ取得 */
            int waveNo      = int.Parse(lineSplit[idxWaveNo]);
            int enemyNo     = int.Parse(lineSplit[idxEnemyNo]);
            float createSec = float.Parse(lineSplit[idxCreateSec]);
            int createPosNo = int.Parse(lineSplit[idxCreatePosNo]);

            /* パターンデータに代入 */
            PatternData data;
            data.enemyNo = enemyNo;
            data.createFrame = (int)(50.0f * createSec); // 時間からフレームに変換
            data.createPosNo = createPosNo;

            // ウェーブ数が増加したら
            if (nowCheckWaveNo < waveNo)
            {
                Debug.Log("本データ追加");
                // 本体データに追加
                itemWave.patternDatas = itemPattern;
                itemWave.createNum = createNum;
                result.Add(itemWave);
                // 一時データの初期化
                itemPattern = new List<PatternData>();
                createNum = 0;
                // 現在ウェーブ数の増加
                nowCheckWaveNo++;
            }

            // データ追加
            itemPattern.Add(data);
            // 生成数増加
            createNum++;
        }

        // 最後のウェーブデータを追加
        itemWave.patternDatas = itemPattern;
        itemWave.createNum = createNum;
        result.Add(itemWave);
#endif

        return result;
    }

    public List<WaveData> GetWaveData(int no)
    {
        return _stageData[no].waveDatas;
    }
}
