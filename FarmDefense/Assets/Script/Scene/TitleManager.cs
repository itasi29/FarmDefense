using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
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


    /* 変数 */
    private int _index;
    private bool _isPrePush;
    private GameObject _cursor;

    void Start()
    {
        _index = 0;
        _isPrePush = false;
    }

    void Update()
    {
        CursorMove();

        if (Input.GetButtonDown("A"))
        {
            Select();
        }
    }

    private void CursorMove()
    {
        float input = Input.GetAxis("DPADY");

        if (!_isPrePush)
        {
            if (input == 1)
            {
                _index = ((int)Kind.kMax + _index - 1) % (int)Kind.kMax;
            }
            else if (input == -1)
            {
                _index = (_index + 1) % (int)Kind.kMax;
            }

            _isPrePush = true;
        }
        else if (input == 0)
        {
            _isPrePush = false;
        }
    }

    private void Select()
    {
        if (_index == (int)Kind.kStart)
        {
            // TODO: Sceneの切り替え(即時)
            // TODO: fadeの終了時に着かえるように変更
        }
        else if (_index == (int)Kind.kOption)
        {
            // TODO: キャンバス上に出すようにする
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
