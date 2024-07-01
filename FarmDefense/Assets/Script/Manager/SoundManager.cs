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

public class SoundManager : MonoBehaviour
{
    /* �萔 */
    public const int kVolumeLvMax = 5;
    private const float kVolumeLvRate = 0.2f;

    /* �T�E���h���\�[�X */
    public AudioSource _seSource;
    public AudioSource _bgmSource;

    /* �T�E���h�f�[�^ */
    private Dictionary<string, SoundData> _seData;
    private Dictionary<string, SoundData> _bgmData;

    /* �S�̃{�����[�� */
    private float _seMasterVolume;
    private float _bgmMasterVolume;

    // ���ݗ���Ă���BGM
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
        // �{�����[������
        _seSource.volume = data.volume * _seMasterVolume;
        // �Đ�
        _seSource.PlayOneShot(data.sound);
    }

    public void PlayBgm(string name)
    {
        var data = _bgmData[name];

        // �Đ����͏I��
        if (_bgmSource.isPlaying) return;

        // ���[�v�Đ�
        _bgmSource.loop = true;
        // �{�����[������
        _bgmSource.volume = data.volume * _bgmMasterVolume;
        // �Đ�
        _bgmSource.PlayOneShot(data.sound);
        // ���ݗ���Ă�����̂Ƃ��ĕۑ�
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

        // �Đ����Ă��Ȃ���ΏI��
        if (!_bgmSource.isPlaying) return;
        // ���ݗ����Ă���BGM�̃{�����[����ύX����
//        _bgmSource.volume = _bgmData[_nowPlayBgm].volume * _bgmMasterVolume;
    }
}
