using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// �I��p�X�N���v�g(�R���g���[��)
/// </summary>
public class PlayerInput1 : MonoBehaviour
{
    float lsh;
    float lsv;
    bool A;
    Image start;
    Image setting;
    Image end;

    Image select;
    // Start is called before the first frame update
    void Start()
    {
        start = GameObject.Find("Canvas/GameObject/start").GetComponent<Image>();
        setting = GameObject.Find("Canvas/GameObject/setting").GetComponent<Image>();
        end = GameObject.Find("Canvas/GameObject/end").GetComponent<Image>();

        select = start;  //�ŏ��Ɏ擾����Q�[���I�u�W�F�N�g
    }

    // Update is called once per frame
    void Update()
    {
        //L Stick
        lsh = Input.GetAxis("Horizontal");
        lsv = Input.GetAxis("Vertical");
        A = Input.GetKeyDown(KeyCode.A);


        if(select == start) //�X�^�[�g��I��ł����ꍇ
        {
            //�K���Ɂu�I��ł���v�Ƃ������Ƃ��킩��悤�ɂ��Ă���
            if(A)
            {
                SceneManager.LoadScene("SaveScene");  //�Z�[�u�V�[���֑J��
            }
        }
        else if(select == setting)  //�ݒ��I��ł����ꍇ
        {
            //�K���Ɂu�I��ł���v�Ƃ������Ƃ��킩��悤�ɂ��Ă���
            if (A)
            {
                SceneManager.LoadScene("OptionScene");  //�ݒ��ʂ֑J��
            }
        }
        else if (select == end)  //�I����I��ł����ꍇ
        {
            //�K���Ɂu�I��ł���v�Ƃ������Ƃ��킩��悤�ɂ��Ă���
            if (A)
            {
                //�I������
            }
        }


        if (lsv > 0)  //��ɍs��
        {
            if(select == start)
            {
                select = end;  //�I���I��
            }
            else if(select == end)
            {
                select = setting;  //�ݒ�I��
            }
            else if(select == setting)
            {
                select = start;  //�X�^�[�g�I��
            }
        }

        if(lsv < 0) //���ɍs��
        {
            if(select == start)
            {
                select = setting;   //�ݒ�I��
            }
            else if(select == setting)
            {
                select = end;   //�I���I��
            }
            else if(select == end)
            {
                select = start;  //�X�^�[�g�I��
            }
        }

        Debug.Log(select);  //�I�����Ă�Q�[���I�u�W�F�N�g�̖��O
    }
}
