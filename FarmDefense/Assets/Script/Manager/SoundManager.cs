using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct SoundData
{
    // サウンドデータ
    public AudioClip sound;
    // それぞれのボリューム
    public float volume;
}

public class SoundManager
{
    /* 定数 */
    public const int kVolumeLvMax = 5;
    private const float kVolumeLvRate = 0.2f;
    private const string kSoundManager = "Csv/SoundManager";

    /* サウンドリソース */
    private AudioSource _seSource;
    private AudioSource _bgmSource;

    /* サウンドデータ */
    private Dictionary<string, SoundData> _seData;
    private Dictionary<string, SoundData> _bgmData;

    /* 全体ボリューム */
    private float _seMasterVolume;
    private float _bgmMasterVolume;

    // 現在流れているBGM
    private string _nowPlayBgm;

    // Start is called before the first frame update
    public void Init(UserData user, AudioSource se, AudioSource bgm)
    {
        _seSource = se;
        _bgmSource = bgm;
        ChangeMasterVolumeBgm(user.GetBgmVolLv());
        ChangeMasterVolumeSe(user.GetSeVolLv());

        Load();
    }

    public void PlaySe(string id)
    {
        var data = _seData[id];
        // ボリューム調整
        _seSource.volume = data.volume * _seMasterVolume;
        // 再生
        _seSource.PlayOneShot(data.sound);
    }

    public void PlayBgm(string id)
    {
        var data = _bgmData[id];

        // 再生中は終了
        if (_bgmSource.isPlaying) return;

        // ループ再生
        _bgmSource.loop = true;
        // ボリューム調整
        _bgmSource.volume = data.volume * _bgmMasterVolume;
        // 再生
        _bgmSource.PlayOneShot(data.sound);
        // 現在流れているものとして保存
        _nowPlayBgm = id;
    }

    public void FadeBgm(float rate)
    {
        // 再生していなければ何もしない
        if (!_bgmSource.isPlaying) return;

        var data = _bgmData[_nowPlayBgm];

        _bgmSource.volume = data.volume * _bgmMasterVolume * rate;
    }

    public void StopSe()
    {
        _seSource.Stop();
    }

    public void StopBgm()
    {
        _bgmSource.Stop();
    }

    public void ChangeMasterVolumeSe(int lv)
    {
        _seMasterVolume = lv * kVolumeLvRate;
        Mathf.Max(Mathf.Min(_seMasterVolume, 1.0f), 0.0f);

        _seSource.volume = _seMasterVolume;
    }

    public void ChangeMasterVolumeBgm(int lv)
    {
        _bgmMasterVolume = lv * kVolumeLvRate;
        Mathf.Max(Mathf.Min(_bgmMasterVolume, 1.0f), 0.0f);
        _bgmSource.volume = _bgmMasterVolume;

        // 再生していなければ終了
        if (!_bgmSource.isPlaying) return;
        // 現在流しているBGMのボリュームを変更する
        _bgmSource.volume = _bgmData[_nowPlayBgm].volume * _bgmMasterVolume;
    }

    private void Load()
    {
        _bgmData = new Dictionary<string, SoundData>();
        _seData = new Dictionary<string, SoundData>();

        // csvファイルの読み込み
        TextAsset stageCsv = Resources.Load(kSoundManager) as TextAsset;
        // データ読み込み
        SoundCSV[] items = CSVSerializer.Deserialize<SoundCSV>(stageCsv.text);

        foreach (var item in items)
        {
            string path = "Sound/" + item.Path;
            var sound = Resources.Load(path) as AudioClip;
            SoundData data = new SoundData();
            data.sound = sound;
            data.volume = item.Volume;
            if (item.Type == "BGM")
            {
                _bgmData.Add(item.ID, data);
            }
            else if (item.Type == "SE")
            {
                _seData.Add(item.ID, data);
            }
        }
    }
}
