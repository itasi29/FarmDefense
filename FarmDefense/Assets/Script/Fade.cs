using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    private Image _image;
    private Color _color;
    private bool _fadeIn;
    private bool _fadeOut;
    private string _nextSceneName;

    private SoundManager _soundMgr;
    [SerializeField] private string _bgmID;

    // Start is called before the first frame update
    void Start()
    {
        _fadeIn = true;
        _fadeOut = false;
        _image = GameObject.Find("FadePanel").gameObject.GetComponent<Image>();
        _color = _image.color;
        _color.a = 0.9999f;

        var director = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        _soundMgr = director.SoundMgr;

        _soundMgr.PlayBgm(_bgmID);
    }

    // Update is called once per frame
    void Update()
    {
        _soundMgr.PlayBgm(_bgmID);
        FadeIn();
        FadeOut();
    }

    public void StartFadeOut(string nextSceneName)
    {
        _fadeOut = true;
        _fadeIn = false;
        _nextSceneName = nextSceneName;
    }

    public void SetBgmID(string id)
    {
        _bgmID = id;
    }

    void FadeIn()
    {
        if (!_fadeIn) return;

        _color.a -= 0.01f;
        if (_color.a < 0)
        {
            _color.a = 0;
            _fadeIn = false;
        }
        _image.color = _color;
        _soundMgr.FadeBgm(1.0f - _color.a);
    }

    void FadeOut()
    {
        if (!_fadeOut) return;

        _color.a += 0.01f;
        if (_color.a > 1)
        {
            _color.a = 1;
            _soundMgr.StopBgm();
            SceneManager.LoadScene(_nextSceneName);
        }
        _image.color = _color;
        _soundMgr.FadeBgm(1.0f - _color.a);
    }
}