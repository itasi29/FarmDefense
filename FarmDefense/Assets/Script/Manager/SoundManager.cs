using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct SoundData
{
    // �T�E���h�f�[�^
    public AudioClip sound;
    // ���ꂼ��̃{�����[��
    public float volume;
}

public class SoundManager
{
    /* �萔 */
    public const int kVolumeLvMax = 5;
    private const float kVolumeLvRate = 0.2f;
    private const string kSoundManager = "Csv/SoundManager";

    /* �T�E���h���\�[�X */
    private AudioSource _seSource;
    private AudioSource _bgmSource;

    /* �T�E���h�f�[�^ */
    private Dictionary<string, SoundData> _seData;
    private Dictionary<string, SoundData> _bgmData;

    /* �S�̃{�����[�� */
    private float _seMasterVolume;
    private float _bgmMasterVolume;

    // ���ݗ���Ă���BGM
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
        // �{�����[������
        _seSource.volume = data.volume * _seMasterVolume;
        // �Đ�
        _seSource.PlayOneShot(data.sound);
    }

    public void PlayBgm(string id)
    {
        var data = _bgmData[id];

        // �Đ����͏I��
        if (_bgmSource.isPlaying) return;

        // ���[�v�Đ�
        _bgmSource.loop = true;
        // �{�����[������
        _bgmSource.volume = data.volume * _bgmMasterVolume;
        // �Đ�
        _bgmSource.PlayOneShot(data.sound);
        // ���ݗ���Ă�����̂Ƃ��ĕۑ�
        _nowPlayBgm = id;
    }

    public void FadeBgm(float rate)
    {
        // �Đ����Ă��Ȃ���Ή������Ȃ�
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

        // �Đ����Ă��Ȃ���ΏI��
        if (!_bgmSource.isPlaying) return;
        // ���ݗ����Ă���BGM�̃{�����[����ύX����
        _bgmSource.volume = _bgmData[_nowPlayBgm].volume * _bgmMasterVolume;
    }

    private void Load()
    {
        _bgmData = new Dictionary<string, SoundData>();
        _seData = new Dictionary<string, SoundData>();

        // csv�t�@�C���̓ǂݍ���
        TextAsset stageCsv = Resources.Load(kSoundManager) as TextAsset;
        // �f�[�^�ǂݍ���
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
