using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionManager : MonoBehaviour
{
    /* å^ */
    enum Kind
    {
        kBgm,
        kSe,
        kReturn,
        kClose,
        kMax
    }

    /* íËêî */

    /* ïœêî */
    private int _index;
    private bool _isPrePush;
    [SerializeField] private GameObject _cursor;
    [SerializeField] private GameObject _bgm;
    [SerializeField] private GameObject _se;
    private GameObject[] _gauge;
    private int _bgmVol;
    private int _seVol;

    void Start()
    {
        _gauge = new GameObject[5];
    }

    // Update is called once per frame
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
        if (_index == (int)Kind.kBgm)
        {
            
        }
        else if (_index == (int)Kind.kSe)
        {

        }
        else if (_index == (int)Kind.kReturn)
        {
            
        }
        else if (_index == (int)Kind.kClose)
        {

        }
    }
}
