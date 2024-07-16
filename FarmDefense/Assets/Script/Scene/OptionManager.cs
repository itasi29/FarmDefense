using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionManager : SelectManager
{
    /* 型 */
    enum Kind
    {
        kBgm,
        kSe,
        kReturn,
        kClose,
        kMax
    }
    public enum ReturnScene
    {
        kTitle,
        kStageSelect
    }

    /* 定数 */
    private const string kReturnTitleName = "TitleScene";
    private const string kReturnStageSelectName = "StageSelect";
    private const float kSlidePosX = 64;
    private const float kBasePosX = -508;
    private const float kBasePosY = 128;
    private const float kIntervalY = 128;
    private const float kCursorShakeWidth = 48;

    /* 変数 */
    private int _preIndex;
    private int _bgmVolLv;
    private int _seVolLv;
    private bool _isSoundChange;
    private bool _isBgmLv;
    private ReturnScene _returnScene;
    private UserData _user;
    private SoundManager _sndMgr;
    private GameObject[] _items;
    [SerializeField] private GameObject _bgmGauge;
    [SerializeField] private GameObject _seGauge;
    private Image[] _gauge = new Image[SoundManager.kVolumeLvMax];
    [SerializeField] private Sprite _activImg;
    [SerializeField] private Sprite _inactivImg;

    protected override void Init()
    {
        _maxY = (int)Kind.kMax;
        _valY = 1;
        _isY = true;
        _cursorWidth = kCursorShakeWidth;
        _cursorPos = new Vector2[]
        {
            new Vector2(kBasePosX , kBasePosY                 ),
            new Vector2(kBasePosX , kBasePosY - kIntervalY    ),
            new Vector2(kBasePosX , kBasePosY - kIntervalY * 2),
            new Vector2(kBasePosX , kBasePosY - kIntervalY * 3),
        };
    }

    public void Init(ReturnScene returnScene)
    {
        _items = new GameObject[(int)Kind.kMax];
        for (int i = 0; i < (int)Kind.kMax; ++i)
        {
            _items[i] = transform.GetChild(2 + i).gameObject;
        }

        var gameMgr = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        var dataMgr = gameMgr.DataMgr;
        _sndMgr = gameMgr.SoundMgr;
        _user = dataMgr.User;

        var pos = _items[0].transform.localPosition;
        pos.x += kSlidePosX;
        _items[0].transform.localPosition = pos;

        _bgmVolLv = _user.GetBgmVolLv();
        _seVolLv = _user.GetSeVolLv();
        SetGauge(_bgmGauge);
        ChangeGaugeImage(_bgmVolLv);
        SetGauge(_seGauge);
        ChangeGaugeImage(_seVolLv);

        _preIndex = _index;
        _isSoundChange = false;

        _returnScene = returnScene;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (_optionSys.IsOpenOption()) return;

        if (_isSoundChange)
        {
            if (Input.GetButtonDown("B"))
            {
                _isSoundChange = false;
                return;
            }
            if (_isBgmLv)
            {
                ChangeSoundVolume(ref _bgmVolLv, _sndMgr.ChangeMasterVolumeBgm, _user.SetBgmVolLv);
            }
            else
            {
                ChangeSoundVolume(ref _seVolLv, _sndMgr.ChangeMasterVolumeSe, _user.SetSeVolLv);
            }
            return;
        }

        base.Update();
    }

    protected override void FixedUpdate()
    {
        if (_optionSys.IsOpenOption()) return;

        base.FixedUpdate();
        if (_isChange)
        {
            _isChange = false;
            // 元の位置に戻す
            var pos = _items[_preIndex].transform.localPosition;
            pos.x -= kSlidePosX;
            _items[_preIndex].transform.localPosition = pos;
            // 右にずらす
            pos = _items[_index].transform.localPosition;
            pos.x += kSlidePosX;
            _items[_index].transform.localPosition = pos;
        }
        _preIndex = _index;
    }

    private void ChangeSoundVolume(ref int soundLv, Action<int> changeVol, Action<int> setVol)
    {
        float input = Input.GetAxis("DPADX");

        if (!_isPrePush)
        {
            if (input == 1)
            {
                soundLv = Mathf.Min((soundLv + 1), SoundManager.kVolumeLvMax);
            }
            else if (input == -1)
            {
                soundLv = Mathf.Max((soundLv - 1), 0);
            }

            changeVol(soundLv);
            setVol(soundLv);
            ChangeGaugeImage(soundLv);

            _isPrePush = true;
        }
        else if (input == 0)
        {
            _isPrePush = false;
        }
    }

    private void ChangeGaugeImage(int soundLv)
    {
        for (int i = 0; i < SoundManager.kVolumeLvMax; ++i)
        {
            if (i < soundLv)
                _gauge[i].sprite = _activImg;
            else
                _gauge[i].sprite = _inactivImg;
        }
    }

    protected override void Select()
    {
        if (_index == (int)Kind.kBgm)
        {
            SelectVolumeChange(true, _bgmGauge);
        }
        else if (_index == (int)Kind.kSe)
        {
            SelectVolumeChange(false, _seGauge);
        }
        else if (_index == (int)Kind.kReturn)
        {
            // TODO: フェード後にシーン遷移するように
            if (_returnScene == ReturnScene.kTitle)
            {
                SceneManager.LoadScene(kReturnTitleName);
            }
            else if (_returnScene == ReturnScene.kStageSelect)
            {
                SceneManager.LoadScene(kReturnStageSelectName);
            }
        }
        else if (_index == (int)Kind.kClose)
        {
            // TODO: 後々演出をつけるようにする
            Destroy(this.gameObject);
        }
    }

    private void SelectVolumeChange(bool isBgmLv, GameObject sourceGauge)
    {
        _isSoundChange = true;
        _isBgmLv = isBgmLv;
        SetGauge(sourceGauge);
    }

    private void SetGauge(GameObject sourceGauge)
    {
        for (int i = 0; i < SoundManager.kVolumeLvMax; ++i)
        {
            _gauge[i] = sourceGauge.transform.GetChild(i).GetComponent<Image>();
        }
    }
}
