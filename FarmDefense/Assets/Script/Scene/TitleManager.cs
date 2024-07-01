using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : SelectManager
{
    /* 型 */
    enum Kind
    {
        kStart,
        kOption,
        kEnd,
        kMax
    }

    /* 定数 */
    [SerializeField] private const int kCursorHeight = 300;
    private const string kNextSceneName = "UserSelectScene";
    private const float kInitPosY = 232;
    private const float kIntervalPosY = 232;
    private const float kCursorPosx = 64;
    private const float kCursorShakeWidth = 48;

    /* 変数 */
    OptionSystem _optionSys;

    void Start()
    {
        _index = 0;
        _isPrePush = false;
        _optionSys = new OptionSystem();
    }

    void Update()
    {
        if (_optionSys.IsOpenOption()) return;
        Debug.Log("lakjdfs");

        CursorMove((int)Kind.kMax);

        if (Input.GetButtonDown("A"))
        {
            Select();
        }
    }

    private void FixedUpdate()
    {
        if (_optionSys.IsOpenOption()) return;

        SetCursorPos(kInitPosY, kIntervalPosY);
        CursorShake(kCursorPosx, kCursorShakeWidth);
    }

    protected override void Select()
    {
        if (_index == (int)Kind.kStart)
        {
            // TODO: Sceneの切り替え(即時)
            // TODO: fadeの終了時に着かえるように変更
            SceneManager.LoadScene(kNextSceneName);
        }
        else if (_index == (int)Kind.kOption)
        {
            _optionSys.Create(OptionManager.ReturnScene.kTitle, Instantiate);
        }
        else if (_index == (int)Kind.kEnd)
        {
#if UNITY_EDITOR
            // エディター上の時
            UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
                // ビルド上の時
                Application.Quit();//ゲームプレイ終了
#endif
        }
    }
}
