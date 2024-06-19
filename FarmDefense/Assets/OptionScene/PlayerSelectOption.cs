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
    [SerializeField] public Image ligth1;
    [SerializeField] public Image ligth2;
    [SerializeField] public Image ligth3;
    [SerializeField] public Image ligth4;
    [SerializeField] public Image ligth5;
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
        ligth1 = GameObject.Find("Canvas/���邳/1").GetComponent<Image>();
        ligth2 = GameObject.Find("Canvas/���邳/2").GetComponent<Image>();
        ligth3 = GameObject.Find("Canvas/���邳/3").GetComponent<Image>();
        ligth4 = GameObject.Find("Canvas/���邳/4").GetComponent<Image>();
        ligth5 = GameObject.Find("Canvas/���邳/5").GetComponent<Image>();
        title = GameObject.Find("Canvas/Title").GetComponent<Image>();
        back = GameObject.Find("Canvas/Back").GetComponent<Image>();

        _select = title;  //�ŏ��ɑ������
    }

    // Update is called once per frame
    public void Update()
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
        GetComponent<PlayerSelectBase>().Select(title, 0, "TitleScene");

        if(_back_to_scene == 1)  //�^�C�g���ɔ�΂�
        {
            GetComponent<PlayerSelectBase>().Select(back, 0, "TitleScene");
        }
        else if(_back_to_scene == 2) //�Z�[�u�V�[���ɔ�΂�
        {
            GetComponent<PlayerSelectBase>().Select(back, 0, "SaveScene");
        }
        else if(_back_to_scene == 3)  //�X�e�[�W�Z���N�g�ɔ�΂�
        {
            GetComponent<PlayerSelectBase>().Select(back, 0, "StageSelectScene");
        }



    }
}
