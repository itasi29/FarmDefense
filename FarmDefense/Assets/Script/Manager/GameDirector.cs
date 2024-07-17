using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    private DataManager _dataMgr;
    private SoundManager _soundMgr;
    private StageRusultData _resultData;

    private void Awake()
    {
        // ���M���j��I�u�W�F�N�g�ɂ���
        DontDestroyOnLoad(this.gameObject);

        _dataMgr = new DataManager();
        _soundMgr = new SoundManager();
        _resultData = new StageRusultData();

        _dataMgr.Load();
        var se = transform.AddComponent<AudioSource>();
        var bgm = transform.AddComponent<AudioSource>();
        _soundMgr.Init(_dataMgr.User, se, bgm);
    }

    private void OnApplicationQuit()
    {
        _dataMgr.End();
    }

    public DataManager DataMgr { get { return _dataMgr; } }
    public SoundManager SoundMgr { get { return _soundMgr;} }
    public StageRusultData ResultData { get { return _resultData; } }
}
