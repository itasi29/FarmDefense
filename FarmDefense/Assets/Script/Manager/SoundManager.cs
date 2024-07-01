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

public class SoundManager : MonoBehaviour
{
    /* 定数 */
    public const int kVolumeLvMax = 5;
    private const float kVolumeLvRate = 0.2f;

    /* サウンドリソース */
    public AudioSource _seSource;
    public AudioSource _bgmSource;

    /* サウンドデータ */
    private Dictionary<string, SoundData> _seData;
    private Dictionary<string, SoundData> _bgmData;

    /* 全体ボリューム */
    private float _seMasterVolume;
    private float _bgmMasterVolume;

    // 現在流れているBGM
    private string _nowPlayBgm;

    // Start is called before the first frame update
    void Start()
    {
        var user = GetComponent<DataManager>().User;
        ChangeMasterVolumeBgm(user.GetBgmVolLv());
        ChangeMasterVolumeSe(user.GetSeVolLv());
    }

    public void PlaySe(string name)
    {
        var data = _seData[name];
        // ボリューム調整
        _seSource.volume = data.volume * _seMasterVolume;
        // 再生
        _seSource.PlayOneShot(data.sound);
    }

    public void PlayBgm(string name)
    {
        var data = _bgmData[name];

        // 再生中は終了
        if (_bgmSource.isPlaying) return;

        // ループ再生
        _bgmSource.loop = true;
        // ボリューム調整
        _bgmSource.volume = data.volume * _bgmMasterVolume;
        // 再生
        _bgmSource.PlayOneShot(data.sound);
        // 現在流れているものとして保存
        _nowPlayBgm = name;
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
//        _bgmSource.volume = _bgmData[_nowPlayBgm].volume * _bgmMasterVolume;
    }
}
