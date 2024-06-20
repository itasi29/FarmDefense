using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectOption : PlayerSelectBase
{
    [SerializeField] public Image bgm1;
    [SerializeField] public Image bgm2;
    [SerializeField] public Image bgm3;
    [SerializeField] public Image bgm4;
    [SerializeField] public Image bgm5;
    [SerializeField] public Image se1;
    [SerializeField] public Image se2;
    [SerializeField] public Image se3;
    [SerializeField] public Image se4;
    [SerializeField] public Image se5;
    [SerializeField] public Image light1;
    [SerializeField] public Image light2;
    [SerializeField] public Image light3;
    [SerializeField] public Image light4;
    [SerializeField] public Image light5;
    [SerializeField] public Image title;
    [SerializeField] public Image back;



    // Start is called before the first frame update
    public override void Start()
    {
        bgm1 = GameObject.Find("Canvas/BGM/1").GetComponent<Image>();
        bgm2 = GameObject.Find("Canvas/BGM/2").GetComponent<Image>();
        bgm3 = GameObject.Find("Canvas/BGM/3").GetComponent<Image>();
        bgm4 = GameObject.Find("Canvas/BGM/4").GetComponent<Image>();
        bgm5 = GameObject.Find("Canvas/BGM/5").GetComponent<Image>();
        se1 = GameObject.Find("Canvas/SE/1").GetComponent<Image>();
        se2 = GameObject.Find("Canvas/SE/2").GetComponent<Image>();
        se3 = GameObject.Find("Canvas/SE/3").GetComponent<Image>();
        se4 = GameObject.Find("Canvas/SE/4").GetComponent<Image>();
        se5 = GameObject.Find("Canvas/SE/5").GetComponent<Image>();
        light1 = GameObject.Find("Canvas/���邳/1").GetComponent<Image>();
        light2 = GameObject.Find("Canvas/���邳/2").GetComponent<Image>();
        light3 = GameObject.Find("Canvas/���邳/3").GetComponent<Image>();
        light4 = GameObject.Find("Canvas/���邳/4").GetComponent<Image>();
        light5 = GameObject.Find("Canvas/���邳/5").GetComponent<Image>();
        title = GameObject.Find("Canvas/Title").GetComponent<Image>();
        back = GameObject.Find("Canvas/Back").GetComponent<Image>();

        _select = title;  //�ŏ��ɑ������
    }

    // Update is called once per frame
    public new void Update()
    {
        GetComponent<PlayerSelectBase>().Update();

        //GetComponent<PlayerSelectBase>().Select(bgm1);  //Bgm��1�ɂ���Ƃ�
        //GetComponent<PlayerSelectBase>().Select(bgm2);  //Bgm��2�ɂ���Ƃ�
        //GetComponent<PlayerSelectBase>().Select(bgm3);  //Bgm��3�ɂ���Ƃ�
        //GetComponent<PlayerSelectBase>().Select(bgm4);  //Bgm��4�ɂ���Ƃ�
        //GetComponent<PlayerSelectBase>().Select(bgm5);  //Bgm��5�ɂ���Ƃ�
        //GetComponent<PlayerSelectBase>().Select(se1);   //Se��1�ɂ���Ƃ�
        //GetComponent<PlayerSelectBase>().Select(se2);   //Se��2�ɂ���Ƃ�
        //GetComponent<PlayerSelectBase>().Select(se3);   //Se��3�ɂ���Ƃ�
        //GetComponent<PlayerSelectBase>().Select(se4);   //Se��4�ɂ���Ƃ�
        //GetComponent<PlayerSelectBase>().Select(se5);   //Se��5�ɂ���Ƃ�
        //GetComponent<PlayerSelectBase>().Select(ligth1);//Ligth��1�ɂ���Ƃ�
        //GetComponent<PlayerSelectBase>().Select(ligth2);//Ligth��2�ɂ���Ƃ�
        //GetComponent<PlayerSelectBase>().Select(ligth3);//Ligth��3�ɂ���Ƃ�
        //GetComponent<PlayerSelectBase>().Select(ligth4);//Ligth��4�ɂ���Ƃ�
        //GetComponent<PlayerSelectBase>().Select(ligth5);//Ligth��5�ɂ���Ƃ�
        GetComponent<PlayerSelectBase>().Select(title, "TitleScene");

        //GetComponent<PlayerSelectBase>().Select(back);  //�߂�

        if(_lsh > 0)  //�E�ɍs��
        {
            //BGM�I��
            if(_select == bgm1)
            {
                _select = bgm2;  //BGM2
            }
            else if (_select == bgm2)
            {
                _select = bgm3;  //BGM3
            }
            else if (_select == bgm3)
            {
                _select = bgm4;  //BGM4
            }
            else if (_select == bgm4)
            {
                _select = bgm5;  //BGM5
            }
            //SE�I��
            if (_select == se1)
            {
                _select = se2;  //se2
            }
            else if (_select == se2)
            {
                _select = se3;  //se3
            }
            else if (_select == se3)
            {
                _select = se4;  //se4
            }
            else if (_select == se4)
            {
                _select = se5;  //se5
            }
            //���邳�I��
            if (_select == light1)
            {
                _select = light2;  //light2
            }
            else if (_select == light2)
            {
                _select = light3;  //light3
            }
            else if (_select == light3)
            {
                _select = light4;  //light4
            }
            else if (_select == light4)
            {
                _select = light5;  //light5
            }
        }
        if(_lsh < 0)  //���ɂ���
        {
            //BGM�I��
            if (_select == bgm5)
            {
                _select = bgm4;  //BGM4
            }
            else if (_select == bgm4)
            {
                _select = bgm3;  //BGM3
            }
            else if (_select == bgm3)
            {
                _select = bgm2;  //BGM2
            }
            else if (_select == bgm2)
            {
                _select = bgm1;  //BGM1
            }
            //SE�I��
            if (_select == se5)
            {
                _select = se4;  //se4
            }
            else if (_select == se4)
            {
                _select = se3;  //se3
            }
            else if (_select == se3)
            {
                _select = se2;  //se2
            }
            else if (_select == se2)
            {
                _select = se1;  //se1
            }
            //���邳�I��
            if (_select == light5)
            {
                _select = light4;  //ligth4
            }
            else if (_select == light4)
            {
                _select = light3;  //ligth3
            }
            else if (_select == light3)
            {
                _select = light2;  //ligth2
            }
            else if (_select == light2)
            {
                _select = light1;  //ligth1
            }
        }
        if(_lsv > 0)  //��ɍs��
        {
            if (_select == title)
            {
                _select = light1;  //���C�g1
            }
            else if (_select == back)
            {
                _select = title;   //�^�C�g��
            }
            else if(_select == light1)
            {
                _select = se1;   //SE1
            }
            else if (_select == se1)
            {
                _select = bgm1;   //bgm1
            }
            else if(_select == bgm1)
            {
                _select = back;  //�o�b�N
            }
            
            
            if(_select == light2)
            {
                _select = se2;  //se2
            }
            else if(_select == se2)
            {
                _select = bgm2;  //bgm2
            }
            else if(_select == bgm2)
            {
                _select = light2;  //ligth2
            }

            if (_select == light3)
            {
                _select = se3;  //se3
            }
            else if (_select == se3)
            {
                _select = bgm3;  //bgm3
            }
            else if (_select == bgm3)
            {
                _select = light3;  //ligth3
            }

            if (_select == light4)
            {
                _select = se4;  //se4
            }
            else if (_select == se4)
            {
                _select = bgm4;  //bgm4
            }
            else if (_select == bgm4)
            {
                _select = light4;  //ligth4
            }

            if (_select == light5)
            {
                _select = se5;  //se5
            }
            else if (_select == se5)
            {
                _select = bgm5;  //bgm5
            }
            else if (_select == bgm5)
            {
                _select = light5;  //ligth5
            }
        }
        if(_lsv < 0)  //���ɍs��
        {
            if(_select == bgm1)
            {
                _select = se1;  //se1
            }
            else if(_select == se1)
            {
                _select = light1;  //ligth1
            }
            else if(_select == light1)
            {
                _select = title;   //title
            }
            else if(_select == title)
            {
                _select = back;   //�o�b�N
            }
           
            if(_select == bgm2)
            {
                _select = se2;  //se2
            }
            else if(_select == se2)
            {
                _select = light2;  //ligth2
            }
            else if(_select == light2)
            {
                _select = bgm2;   //bgm2
            }

            if (_select == bgm3)
            {
                _select = se3;  //se3
            }
            else if (_select == se3)
            {
                _select = light3;  //ligth3
            }
            else if (_select == light3)
            {
                _select = bgm3;   //bgm3
            }

            if (_select == bgm4)
            {
                _select = se4;  //se4
            }
            else if (_select == se4)
            {
                _select = light4;  //ligth4
            }
            else if (_select == light4)
            {
                _select = bgm4;   //bgm4
            }

            if (_select == bgm5)
            {
                _select = se5;  //se5
            }
            else if (_select == se5)
            {
                _select = light5;  //ligth5
            }
            else if (_select == light5)
            {
                _select = bgm5;   //bgm5
            }
        }

    }
}
