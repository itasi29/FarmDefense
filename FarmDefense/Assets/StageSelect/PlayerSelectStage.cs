using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectStage : PlayerSelectBase
{
    [SerializeField] public Image stage1;
    [SerializeField] public Image stage2;
    [SerializeField] public Image stage3;
    [SerializeField] public Image stage4;
    [SerializeField] public Image stage5;
    [SerializeField] public Image stage6;
    [SerializeField] public Image option;
    [SerializeField] public Image shop;



    // Start is called before the first frame update
    public override void Start()
    {
        stage1 = GameObject.Find("Canvas/Stage/Stage1").GetComponent<Image>();
        stage2 = GameObject.Find("Canvas/Stage/Stage2").GetComponent<Image>();
        stage3 = GameObject.Find("Canvas/Stage/Stage3").GetComponent<Image>();
        stage4 = GameObject.Find("Canvas/Stage/Stage4").GetComponent<Image>();
        stage5 = GameObject.Find("Canvas/Stage/Stage5").GetComponent<Image>();
        stage6 = GameObject.Find("Canvas/Stage/Stage6").GetComponent<Image>();
        option = GameObject.Find("Canvas/Option").GetComponent<Image>();
        shop = GameObject.Find("Canvas/Shop").GetComponent<Image>();

        _select = stage1;  //�ŏ��ɒl�����Ă���
    }

    // Update is called once per frame
    public void Update()
    {
        GetComponent<PlayerSelectBase>().Update();

        //GetComponent<PlayerSelectBase>().Select(stage1);  //�X�e�[�W1�ɔ��
        //GetComponent<PlayerSelectBase>().Select(stage2);  //�X�e�[�W2�ɔ��
        //GetComponent<PlayerSelectBase>().Select(stage3);  //�X�e�[�W3�ɔ��
        //GetComponent<PlayerSelectBase>().Select(stage4);  //�X�e�[�W4�ɔ��
        //GetComponent<PlayerSelectBase>().Select(stage5);  //�X�e�[�W5�ɔ��
        //GetComponent<PlayerSelectBase>().Select(stage6);  //�X�e�[�W6�ɔ��
        GetComponent<PlayerSelectBase>().Select(option, 3, "OptionScene");  //�ݒ�ɔ��
        //GetComponent<PlayerSelectBase>().Select(shop, "");  //�V���b�v�ɔ��

        if(_lsh > 0)  //�E�ɍs��
        {
            //�X�e�[�W�I��
            if(_select == stage1)
            {
                _select = stage2;  //�X�e�[�W2
            }
            else if (_select == stage2)
            {
                _select = stage3;  //�X�e�[�W3
            }
            else if (_select == stage3)
            {
                _select = stage4; //�X�e�[�W4
            }
            else if (_select == stage4)
            {
                _select = stage5;  //�X�e�[�W5
            }
            else if (_select == stage5)
            {
                _select = stage6;  //�X�e�[�W6
            }
            else if (_select == stage6)
            {
                _select = stage1;  //�X�e�[�W1
            }

            if(_select == option)
            {
                _select = shop;  //�V���b�v
            }
            else if(_select == shop)
            {
                _select = option;  //�ݒ�
            }
        }
        if(_lsh < 0)  //���ɍs��
        {
            //�X�e�[�W�I��
            if (_select == stage1)
            {
                _select = stage6;  //�X�e�[�W6
            }
            else if (_select == stage6)
            {
                _select = stage5;  //�X�e�[�W5
            }
            else if (_select == stage5)
            {
                _select = stage4; //�X�e�[�W4
            }
            else if (_select == stage4)
            {
                _select = stage3;  //�X�e�[�W3
            }
            else if (_select == stage3)
            {
                _select = stage2;  //�X�e�[�W2
            }
            else if (_select == stage2)
            {
                _select = stage1;  //�X�e�[�W1
            }

            if(_select == option)
            {
                _select = shop;  //�V���b�v
            }
            else if(_select == shop)
            {
                _select = option;  //�ݒ�
            }
        }

        if(_lsv > 0)  //��ɍs��
        {
            if(_select == stage1)
            {
                _select = option;  //�ݒ�
            }
            else if (_select == option)
            {
                _select = stage4;  //�X�e�[�W4
            }
            else if (_select == stage4)
            {
                _select = stage1;  //�X�e�[�W1
            }
            else if (_select == stage3)
            {
                _select = shop;  //�V���b�v
            }
            else if (_select == shop)
            {
                _select = stage6;  //�X�e�[�W6
            }
            else if (_select == stage6)
            {
                _select = stage3;  //�X�e�[�W3
            }
            else if (_select == stage2)
            {
                _select = stage5;  //�X�e�[�W5
            }
            else if (_select == stage5)
            {
                _select = stage2;  //�X�e�[�W2
            }
        }
        if(_lsv < 0)  //���ɍs��
        {
            if(_select == stage1)
            {
                _select = stage4;  //�X�e�[�W4
            }
            else if (_select == stage4)
            {
                _select = option;  //�ݒ�
            }
            else if (_select == option)
            {
                _select = stage1;  //�X�e�[�W1
            }
            else if (_select == stage3)
            {
                _select = stage6;  //�X�e�[�W6
            }
            else if (_select == stage6)
            {
                _select = shop;  //�V���b�v
            }
            else if (_select == shop)
            {
                _select = stage3;  //�X�e�[�W3
            }
            else if (_select == stage2)
            {
                _select = stage5;  //�X�e�[�W5
            }
            else if (_select == stage5)
            {
                _select = stage2;  //�X�e�[�W2
            }
        }

    }
}
