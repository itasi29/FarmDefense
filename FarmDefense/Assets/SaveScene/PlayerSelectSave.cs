using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectSave : PlayerSelectBase
{
    [SerializeField] public Image save1;
    [SerializeField] public Image save2;
    [SerializeField] public Image save3;
    [SerializeField] public Image save4;
    [SerializeField] public Image option;
    [SerializeField] public Image back;
    // Start is called before the first frame update
    public override void Start()
    {
        save1 = GameObject.Find("Canvas/SaveSelect/Save1").GetComponent<Image>();
        save2 = GameObject.Find("Canvas/SaveSelect/Save2").GetComponent<Image>();
        save3 = GameObject.Find("Canvas/SaveSelect/Save3").GetComponent<Image>();
        save4 = GameObject.Find("Canvas/SaveSelect/Save4").GetComponent<Image>();
        option = GameObject.Find("Canvas/Option").GetComponent<Image>();
        back = GameObject.Find("Canvas/back").GetComponent<Image>();

        _select = save1;  //�ŏ��ɒl������

    }

    // Update is called once per frame
    public void Update()
    {
        GetComponent<PlayerSelectBase>().Update();

        //GetComponent<PlayerSelectBase>().Select(save1);  //�Z�[�u1�ɔ��
        //GetComponent<PlayerSelectBase>().Select(save2);  //�Z�[�u2�ɔ��
        //GetComponent<PlayerSelectBase>().Select(save3);  //�Z�[�u3�ɔ��
        //GetComponent<PlayerSelectBase>().Select(save4);  //�Z�[�u4�ɔ��

        GetComponent<PlayerSelectBase>().Select(option, 2, "OptionScene");  //�ݒ�ɔ��
        GetComponent<PlayerSelectBase>().Select(back, 0, "TitleScene");  //�^�C�g����ʂɔ��

        if(_lsh > 0)  //�E�ɍs��
        {
            //�Z�[�u�I��
            if(_select == save1)
            {
                _select = save2;  //�Z�[�u2
            }
            else if(_select == save2)
            {
                _select = save3;  //�Z�[�u3
            }
            else if(_select == save3)
            {
                _select = save4;  //�Z�[�u4
            }
            else if(_select == save4)
            {
                _select = save1; //�Z�[�u1
            }

            //���̑I��
            if(_select == option)
            {
                _select = back;
            }
            else if(_select == back)
            {
                _select = option;
            }
        }
        if (_lsh < 0) //���ɍs��
        {
            //�Z�[�u�I��
            if (_select == save1)
            {
                _select = save4;  //�Z�[�u4
            }
            else if (_select == save4)
            {
                _select = save3;  //�Z�[�u3
            }
            else if (_select == save3)
            {
                _select = save2;  //�Z�[�u2
            }
            else if (_select == save2)
            {
                _select = save1; //�Z�[�u1
            }

            //���̑I��
            if(_select == option)
            {
                _select = back;
            }
            else if (_select == back)
            {
                _select = option;
            }
        }

        if(_lsv > 0) //��ɍs��
        {
            if(_select == save1)
            {
                _select = option;  //�ݒ���
            }
            else if(_select == save2)
            {
                _select = back;  //�^�C�g�����
            }
            else if(_select == option)
            {
                _select = save3;  //�Z�[�u3
            }
            else if(_select == back)
            {
                _select = save4;  //�Z�[�u4
            }
            else if(_select == save3)
            {
                _select = save1;  //�Z�[�u1
            }
            else if(_select == save4)
            {
                _select = save2;  //�Z�[�u2
            }
        }

        if(_lsv < 0) //���ɍs��
        {
            if(_select == save1)
            {
                _select = save3;  //�Z�[�u3
            }
            else if(_select == save2)
            {
                _select = save4; //�Z�[�u4
            }
            else if(_select == save3)
            {
                _select = option;  //�ݒ���
            }
            else if(_select == save4)
            {
                _select = back;  //�^�C�g�����
            }
            else if(_select == option)
            {
                _select = save1;  //�Z�[�u1
            }
            else if(_select == back)
            {
                _select = save2;  //�Z�[�u2
            }
        }
       
    }
}
