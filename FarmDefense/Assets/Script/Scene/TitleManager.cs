using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : SelectManager
{
    /* �^ */
    enum Kind
    {
        kStart,
        kOption,
        kEnd,
        kMax
    }

    /* �萔 */
    [SerializeField] private const int kCursorHeight = 300;
    private const string kNextSceneName = "DataSelectScene";
    private const float kBasePosX = 64;
    private const float kBasePosY = 232;
    private const float kIntervalY = -232;
    private const float kCursorShakeWidth = 48;
    private Vector2[] kCursorPos = new Vector2[]
    {
        new Vector2(kBasePosX , kBasePosY                 ),
        new Vector2(kBasePosX , kBasePosY + kIntervalY    ),
        new Vector2(kBasePosX , kBasePosY + kIntervalY * 2),
    };

    protected override void Init()
    {
        _maxX = _maxY = (int)Kind.kMax;
        _valY = 1;
        _isY = true;
        _cursorWidth = kCursorShakeWidth;
        _cursorPos = new Vector2[]
        {
            new Vector2(kBasePosX , kBasePosY                 ),
            new Vector2(kBasePosX , kBasePosY + kIntervalY    ),
            new Vector2(kBasePosX , kBasePosY + kIntervalY * 2),
        };
    }

    protected override void Select()
    {
        if (_index == (int)Kind.kStart)
        {
            // TODO: Scene�̐؂�ւ�(����)
            // TODO: fade�̏I�����ɒ�������悤�ɕύX
            SceneManager.LoadScene(kNextSceneName);
        }
        else if (_index == (int)Kind.kOption)
        {
            _optionSys.Create(OptionManager.ReturnScene.kTitle, Instantiate);
        }
        else if (_index == (int)Kind.kEnd)
        {
#if UNITY_EDITOR
            // �G�f�B�^�[��̎�
            UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
#else
            // �r���h��̎�
            Application.Quit();//�Q�[���v���C�I��
#endif
        }
    }
}
