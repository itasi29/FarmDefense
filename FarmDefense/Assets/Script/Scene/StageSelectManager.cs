using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageSelectManager : MonoBehaviour
{
    enum Kind
    {
        kStage1,
        kStage2, 
        kStage3, 
        kStage4,
        kStage5,
        kStage6,
        kMax
    }

    /* 定数 */
    private const string kStageSceneNameBase = "StageScene";
    private const string kTitleSceneName = "TitleScene";
    private const string kShopSceneName = "ShopScene";

    private int _index;
    private int _scaleCount;
    private int _stopCount;
    private bool _isStop;
    private bool _isPrePush;
    private UserData _user;
    private SpawnerData _stage;
    private OptionSystem _optionSys;
    private Fade _fade;
    [SerializeField] Image _rightImg;
    [SerializeField] Image _leftImg;
    [SerializeField] Image _btImg;
    [SerializeField] Animator _bookAnim;
    [SerializeField] GameObject _strs;
    [SerializeField] Sprite _charengeOk;
    [SerializeField] Sprite _charengeNg;

    private void Start()
    {
        _index = (int)Kind.kStage1;
        _scaleCount = 0;
        _stopCount = 0;
        _isStop = false;
        _isPrePush = false;

        var director = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        _user = director.DataMgr.User;
        _stage = director.DataMgr.Spawner;
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
        if (Input.GetButtonDown("X"))
        {
            _fade.StartFadeOut(kShopSceneName);
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
        float scale2 = 1.0f - Mathf.Sin(_scaleCount * 1.7f * Mathf.Deg2Rad) * 0.1f;
        Vector3 vS = new Vector3(scale, scale, scale);
        Vector3 vS2 = new Vector3(scale2, scale2, scale2);
        _rightImg.transform.localScale = vS;
        _leftImg.transform.localScale = vS;
        _btImg.transform.localScale = vS2;
        Debug.Log(_scaleCount);
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
        _fade.StartFadeOut(kStageSceneNameBase + (_index + 1).ToString());
    }

    private void Cancel()
    {
        _fade.StartFadeOut(kTitleSceneName);
    }

    private void SetStringData()
    {
        var data = _strs.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        var dMoney = _strs.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        var sMoney = _strs.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        var cImg = _strs.transform.GetChild(3).GetComponent<Image>();

        data.text = "ステージ" + (_index + 1).ToString();
        dMoney.text = "へんどうちん：\n　" + _stage.GetDynamicMoney(_index).ToString("D4");
        sMoney.text = "こていちん：\n　" + _stage.GetStaticMoney(_index).ToString("D4");
        if (_user.IsStageClear(_index))
        {
            cImg.sprite = _charengeOk;
        }
        else
        {
            cImg.sprite = _charengeNg;
        }
    }
}
