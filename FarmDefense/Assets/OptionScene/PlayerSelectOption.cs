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
        ligth1 = GameObject.Find("Canvas/明るさ/1").GetComponent<Image>();
        ligth2 = GameObject.Find("Canvas/明るさ/2").GetComponent<Image>();
        ligth3 = GameObject.Find("Canvas/明るさ/3").GetComponent<Image>();
        ligth4 = GameObject.Find("Canvas/明るさ/4").GetComponent<Image>();
        ligth5 = GameObject.Find("Canvas/明るさ/5").GetComponent<Image>();
        title = GameObject.Find("Canvas/Title").GetComponent<Image>();
        back = GameObject.Find("Canvas/Back").GetComponent<Image>();

        _select = title;  //最初に代入する
    }

    // Update is called once per frame
    public void Update()
    {
        GetComponent<PlayerSelectBase>().Update();

        //GetComponent<PlayerSelectBase>().Select(bgm1);  //Bgmを1にするとき
        //GetComponent<PlayerSelectBase>().Select(bgm2);  //Bgmを2にするとき
        //GetComponent<PlayerSelectBase>().Select(bgm3);  //Bgmを3にするとき
        //GetComponent<PlayerSelectBase>().Select(bgm4);  //Bgmを4にするとき
        //GetComponent<PlayerSelectBase>().Select(bgm5);  //Bgmを5にするとき
        //GetComponent<PlayerSelectBase>().Select(se1);   //Seを1にするとき
        //GetComponent<PlayerSelectBase>().Select(se2);   //Seを2にするとき
        //GetComponent<PlayerSelectBase>().Select(se3);   //Seを3にするとき
        //GetComponent<PlayerSelectBase>().Select(se4);   //Seを4にするとき
        //GetComponent<PlayerSelectBase>().Select(se5);   //Seを5にするとき
        //GetComponent<PlayerSelectBase>().Select(ligth1);//Ligthを1にするとき
        //GetComponent<PlayerSelectBase>().Select(ligth2);//Ligthを2にするとき
        //GetComponent<PlayerSelectBase>().Select(ligth3);//Ligthを3にするとき
        //GetComponent<PlayerSelectBase>().Select(ligth4);//Ligthを4にするとき
        //GetComponent<PlayerSelectBase>().Select(ligth5);//Ligthを5にするとき
        GetComponent<PlayerSelectBase>().Select(title, 0, "TitleScene");

        if(_back_to_scene == 1)  //タイトルに飛ばす
        {
            GetComponent<PlayerSelectBase>().Select(back, 0, "TitleScene");
        }
        else if(_back_to_scene == 2) //セーブシーンに飛ばす
        {
            GetComponent<PlayerSelectBase>().Select(back, 0, "SaveScene");
        }
        else if(_back_to_scene == 3)  //ステージセレクトに飛ばす
        {
            GetComponent<PlayerSelectBase>().Select(back, 0, "StageSelectScene");
        }



    }
}
