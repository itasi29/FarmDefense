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

        _select = save1;  //最初に値を入れる

    }

    // Update is called once per frame
    public void Update()
    {
        GetComponent<PlayerSelectBase>().Update();

        //GetComponent<PlayerSelectBase>().Select(save1);  //セーブ1に飛ぶ
        //GetComponent<PlayerSelectBase>().Select(save2);  //セーブ2に飛ぶ
        //GetComponent<PlayerSelectBase>().Select(save3);  //セーブ3に飛ぶ
        //GetComponent<PlayerSelectBase>().Select(save4);  //セーブ4に飛ぶ

        GetComponent<PlayerSelectBase>().Select(option, 2, "OptionScene");  //設定に飛ぶ
        GetComponent<PlayerSelectBase>().Select(back, 0, "TitleScene");  //タイトル画面に飛ぶ

        if(_lsh > 0)  //右に行く
        {
            //セーブ選択
            if(_select == save1)
            {
                _select = save2;  //セーブ2
            }
            else if(_select == save2)
            {
                _select = save3;  //セーブ3
            }
            else if(_select == save3)
            {
                _select = save4;  //セーブ4
            }
            else if(_select == save4)
            {
                _select = save1; //セーブ1
            }

            //他の選択
            if(_select == option)
            {
                _select = back;
            }
            else if(_select == back)
            {
                _select = option;
            }
        }
        if (_lsh < 0) //左に行く
        {
            //セーブ選択
            if (_select == save1)
            {
                _select = save4;  //セーブ4
            }
            else if (_select == save4)
            {
                _select = save3;  //セーブ3
            }
            else if (_select == save3)
            {
                _select = save2;  //セーブ2
            }
            else if (_select == save2)
            {
                _select = save1; //セーブ1
            }

            //他の選択
            if(_select == option)
            {
                _select = back;
            }
            else if (_select == back)
            {
                _select = option;
            }
        }

        if(_lsv > 0) //上に行く
        {
            if(_select == save1)
            {
                _select = option;  //設定画面
            }
            else if(_select == save2)
            {
                _select = back;  //タイトル画面
            }
            else if(_select == option)
            {
                _select = save3;  //セーブ3
            }
            else if(_select == back)
            {
                _select = save4;  //セーブ4
            }
            else if(_select == save3)
            {
                _select = save1;  //セーブ1
            }
            else if(_select == save4)
            {
                _select = save2;  //セーブ2
            }
        }

        if(_lsv < 0) //下に行く
        {
            if(_select == save1)
            {
                _select = save3;  //セーブ3
            }
            else if(_select == save2)
            {
                _select = save4; //セーブ4
            }
            else if(_select == save3)
            {
                _select = option;  //設定画面
            }
            else if(_select == save4)
            {
                _select = back;  //タイトル画面
            }
            else if(_select == option)
            {
                _select = save1;  //セーブ1
            }
            else if(_select == back)
            {
                _select = save2;  //セーブ2
            }
        }
       
    }
}
