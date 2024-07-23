using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DataSelectManager : MonoBehaviour
{
    enum Kind
    {
        Data1,
        Data2,
        Data3,
        Data4,
        kMax
    }

    /* 定数 */
    private const string kStageSelectSceneName = "StageSelectScene";
    private const string kTitleSceneName       = "TitleScene";
    private const float kBasePosX = -80;
    private const float kBasePosY = 192;
    private const float kIntervalX = 640;
    private const float kIntervalY = -320;
    private const float kCursorShakeWidth = 32;

    /* 変数 */
    private int _index;
    private int _scaleCount;
    private int _stopCount;
    private bool _isStop;
    private bool _isPrePush;
    private UserData _user;
    private OptionSystem _optionSys;
    private Fade _fade;
    [SerializeField] Image _rightImg;
    [SerializeField] Image _leftImg;
    [SerializeField] Animator _bookAnim;
    [SerializeField] GameObject _strs;

    private void Start()
    {
        _index = (int)Kind.Data1;
        _scaleCount = 0;
        _stopCount = 0;
        _isStop = false;
        _isPrePush = false;

        _user = GameObject.Find("GameDirector").GetComponent<GameDirector>().DataMgr.User;
        _optionSys = new OptionSystem();
        _fade = GetComponent<Fade>();

        SetStringData();
    }


    private void Update()
    {
        if (_optionSys.IsOpenOption()) return;

        if (_isStop) return;

        IndexUpdate();

        if (Input.GetButtonDown("A"))
        {
            Select();
        }
        if (Input.GetButtonDown("B"))
        {
            Cancel();
        }
    }

    private void FixedUpdate()
    {
        if (_isStop)
        {
            --_stopCount;

            if (_stopCount < 0)
            {
                _isStop = false;

                SetStringData();
                _strs.SetActive(true);
            }
        }

        if (_optionSys.IsOpenOption()) return;

        ++_scaleCount;

        float scale = 1.0f + Mathf.Sin(_scaleCount * Mathf.Deg2Rad) * 0.2f;
        Vector3 vS = new Vector3(scale, scale, scale);
        _rightImg.transform.localScale = vS;
        _leftImg.transform.localScale = vS;
    }

    private void IndexUpdate()
    {
        if (_isStop) return;

        float input = Input.GetAxis("DPADX");

        if (!_isPrePush)
        {
            int max = (int)Kind.kMax;
            bool isPush = false;

            if (input < 0)
            {
                _index = (max + _index - 1) % max;
                isPush = true;
                _bookAnim.SetTrigger("Right");
            }
            else if (input > 0)
            {
                _index = (_index + 1) % max;
                isPush = true;
                _bookAnim.SetTrigger("Left");
            }

            if (isPush)
            {
                _isPrePush = true;
                _isStop = true;
                _stopCount = 30;

                _strs.SetActive(false);
            }
        }
        else if (input == 0)
        {
            _isPrePush = false;
        }

    }

    private void Select()
    {
        _user.SetUserNo(_index);
        _fade.StartFadeOut(kStageSelectSceneName);
    }

    private void Cancel()
    {
        _fade.StartFadeOut(kTitleSceneName);
    }

    private void SetStringData()
    {
        var data = _strs.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        var time = _strs.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        var money = _strs.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        var near = _strs.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        var far = _strs.transform.GetChild(4).GetComponent<TextMeshProUGUI>();

        data.text = "データ" + (_index + 1).ToString();
        int t = _user.GetTime(_index);
        int s = t / 50;
        int m = s / 60;
        time.text = "じかん：" + m.ToString("D2") + ":" + s.ToString("D2");
        money.text = "おかね：" + _user.GetMoney(_index).ToString("D4");
        near.text = "きんせつ\n　こうげき：Lv" + _user.GetWeaponLv("W_0", _index) + "\n　そくど　：Lv" + _user.GetWeaponLv("W_1", _index) + "\n　はんい　：Lv" + _user.GetWeaponLv("W_2", _index);
        far.text = "えんきょり\n　こうげき：Lv" + _user.GetWeaponLv("W_3", _index) + "\n　そくど　：Lv" + _user.GetWeaponLv("W_4", _index) + "\n　はんい　：Lv" + _user.GetWeaponLv("W_5", _index);
    }
}
