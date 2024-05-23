using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

// パターンデータ
[System.Serializable]
struct PatternData
{
    public int enemyNo;
    public int createFrame;
    public int createPosNo;
};

[System.Serializable]
struct WaveData
{
    public int waveNo;
    public int createNum;
    public List<PatternData> patternDatas;
}

// ステージデータ
[System.Serializable]
struct StageData
{
    public int no;
    public float enemyBoostRate;
    public int staticMoney;
    public int dynamicMoney;
    public List<WaveData> waveDatas;
};

public class SpawnerManager : MonoBehaviour
{
    // ステージデータ
    [SerializeField] private List<StageData> _stageData;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("ロード!");
        LoadStage();
    }

    /// <summary>
    /// ステージデータの読み込み
    /// </summary>
    private void LoadStage()
    {
        // csvファイルの読み込み
        TextAsset stageCsv = Resources.Load("StageManager") as TextAsset;
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
    }

    /// <summary>
    /// パターンデータの読み込み
    /// </summary>
    private List<WaveData> LoadPatter(string stageName)
    {
        // 返す用のデータ
        List<WaveData> waveData = new List<WaveData>();

        // csvファイルの読み込み
        TextAsset patternCsv = Resources.Load(stageName) as TextAsset;
        // 変換
        StringReader reader = new StringReader(patternCsv.text);

        // TODO:一番上のデータから要素番号を取得
        // 一行読み込んで、','で区切る
        var lineSplit = reader.ReadLine().Split(',');
        int idxWaveNo = 0;
        int idxEnemyNo = 1;
        int idxCreateSec = 2;
        int idxCreatePosNo = 3;

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
            PatternData patternData;
            patternData.enemyNo = enemyNo;
            patternData.createFrame = (int)(50.0f * createSec); // 時間からフレームに変換
            patternData.createPosNo = createPosNo;

            /* ウェーブデータに代入 */
            waveData[waveNo - 1].patternDatas.Add(patternData);
        }

        return waveData;
    }
}
