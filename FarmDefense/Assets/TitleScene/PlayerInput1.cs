using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// �I��p�X�N���v�g(�R���g���[��)
/// </summary>
public class PlayerInput1 : PlayerSelectBase
{
    [SerializeField] public Image start;
    [SerializeField] public Image option;
    [SerializeField] public Image end;

   
    // Start is called before the first frame update
    public override void Start()
    {
        

        start = GameObject.Find("Canvas/GameObject/start").GetComponent<Image>();
        option = GameObject.Find("Canvas/GameObject/option").GetComponent<Image>();
        end = GameObject.Find("Canvas/GameObject/end").GetComponent<Image>();

        _select = start;  //�ŏ��Ɏ擾����Q�[���I�u�W�F�N�g
    }

    // Update is called once per frame
    public void Update()
    {
        GetComponent<PlayerSelectBase>().Update();

        GetComponent<PlayerSelectBase>().Select(start, "SaveScene");
        GetComponent<PlayerSelectBase>().Select(option, "OptionScene");

        if (_select == end)  //�I����I��ł����ꍇ
        {
            //�K���Ɂu�I��ł���v�Ƃ������Ƃ��킩��悤�ɂ��Ă���
            if (_A > 0)
            {
                //�I������
            }
        }


        if (_lsv > 0)  //��ɍs��
        {
            if(_select == start)
            {
                _select = end;  //�I���I��
            }
            else if(_select == end)
            {
                _select = option;  //�ݒ�I��
            }
            else if(_select == option)
            {
                _select = start;  //�X�^�[�g�I��
            }
        }

        if(_lsv < 0) //���ɍs��
        {
            if(_select == start)
            {
                _select = option;   //�ݒ�I��
            }
            else if(_select == option)
            {
                _select = end;   //�I���I��
            }
            else if(_select == end)
            {
                _select = start;  //�X�^�[�g�I��
            }
        }
    }
}
