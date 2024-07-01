using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    /* �^ */
    enum Next
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
            if (_index == (int)Next.kStart)
            {
                // TODO: Scene�̐؂�ւ�(����)
                // TODO: fade�̏I�����ɒ�������悤�ɕύX
            }
            else if (_index == (int)Next.kOption)
            {
                // TODO: �L�����o�X��ɏo���悤�ɂ���
            }
            else if (_index == (int)Next.kEnd)
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

    private void CursorMove()
    {
        float input = Input.GetAxis("DPADY");

        if (!_isPrePush)
        {
            if (input == 1)
            {
                _index = ((int)Next.kMax + _index - 1) % (int)Next.kMax;
            }
            else if (input == -1)
            {
                _index = (_index + 1) % (int)Next.kMax;
            }

            _isPrePush = true;
        }
        else if (input == 0)
        {
            _isPrePush = false;
        }
    }
}
