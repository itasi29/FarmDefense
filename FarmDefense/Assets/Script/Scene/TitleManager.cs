using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
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


    /* �ϐ� */
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
            // TODO: Scene�̐؂�ւ�(����)
            // TODO: fade�̏I�����ɒ�������悤�ɕύX
        }
        else if (_index == (int)Kind.kOption)
        {
            // TODO: �L�����o�X��ɏo���悤�ɂ���
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
