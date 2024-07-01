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
    private const string kNextSceneName = "UserSelectScene";
    private const float kInitPosY = 232;
    private const float kIntervalPosY = 232;
    private const float kCursorPosx = 64;
    private const float kCursorShakeWidth = 48;

    /* �ϐ� */
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
