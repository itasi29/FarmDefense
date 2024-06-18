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

        _select = stage1;  //最初に値を入れておく
    }

    // Update is called once per frame
    public void Update()
    {
        GetComponent<PlayerSelectBase>().Update();

        //GetComponent<PlayerSelectBase>().Select(stage1);  //ステージ1に飛ぶ
        //GetComponent<PlayerSelectBase>().Select(stage2);  //ステージ2に飛ぶ
        //GetComponent<PlayerSelectBase>().Select(stage3);  //ステージ3に飛ぶ
        //GetComponent<PlayerSelectBase>().Select(stage4);  //ステージ4に飛ぶ
        //GetComponent<PlayerSelectBase>().Select(stage5);  //ステージ5に飛ぶ
        //GetComponent<PlayerSelectBase>().Select(stage6);  //ステージ6に飛ぶ
        GetComponent<PlayerSelectBase>().Select(option, 3, "OptionScene");  //設定に飛ぶ
        //GetComponent<PlayerSelectBase>().Select(shop, "");  //ショップに飛ぶ

        if(_lsh > 0)  //右に行く
        {
            //ステージ選択
            if(_select == stage1)
            {
                _select = stage2;  //ステージ2
            }
            else if (_select == stage2)
            {
                _select = stage3;  //ステージ3
            }
            else if (_select == stage3)
            {
                _select = stage4; //ステージ4
            }
            else if (_select == stage4)
            {
                _select = stage5;  //ステージ5
            }
            else if (_select == stage5)
            {
                _select = stage6;  //ステージ6
            }
            else if (_select == stage6)
            {
                _select = stage1;  //ステージ1
            }

            if(_select == option)
            {
                _select = shop;  //ショップ
            }
            else if(_select == shop)
            {
                _select = option;  //設定
            }
        }
        if(_lsh < 0)  //左に行く
        {
            //ステージ選択
            if (_select == stage1)
            {
                _select = stage6;  //ステージ6
            }
            else if (_select == stage6)
            {
                _select = stage5;  //ステージ5
            }
            else if (_select == stage5)
            {
                _select = stage4; //ステージ4
            }
            else if (_select == stage4)
            {
                _select = stage3;  //ステージ3
            }
            else if (_select == stage3)
            {
                _select = stage2;  //ステージ2
            }
            else if (_select == stage2)
            {
                _select = stage1;  //ステージ1
            }

            if(_select == option)
            {
                _select = shop;  //ショップ
            }
            else if(_select == shop)
            {
                _select = option;  //設定
            }
        }

        if(_lsv > 0)  //上に行く
        {
            if(_select == stage1)
            {
                _select = option;  //設定
            }
            else if (_select == option)
            {
                _select = stage4;  //ステージ4
            }
            else if (_select == stage4)
            {
                _select = stage1;  //ステージ1
            }
            else if (_select == stage3)
            {
                _select = shop;  //ショップ
            }
            else if (_select == shop)
            {
                _select = stage6;  //ステージ6
            }
            else if (_select == stage6)
            {
                _select = stage3;  //ステージ3
            }
            else if (_select == stage2)
            {
                _select = stage5;  //ステージ5
            }
            else if (_select == stage5)
            {
                _select = stage2;  //ステージ2
            }
        }
        if(_lsv < 0)  //下に行く
        {
            if(_select == stage1)
            {
                _select = stage4;  //ステージ4
            }
            else if (_select == stage4)
            {
                _select = option;  //設定
            }
            else if (_select == option)
            {
                _select = stage1;  //ステージ1
            }
            else if (_select == stage3)
            {
                _select = stage6;  //ステージ6
            }
            else if (_select == stage6)
            {
                _select = shop;  //ショップ
            }
            else if (_select == shop)
            {
                _select = stage3;  //ステージ3
            }
            else if (_select == stage2)
            {
                _select = stage5;  //ステージ5
            }
            else if (_select == stage5)
            {
                _select = stage2;  //ステージ2
            }
        }

    }
}
