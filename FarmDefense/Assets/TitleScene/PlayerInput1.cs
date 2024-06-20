using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 選択用スクリプト(コントローラ)
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

        _select = start;  //最初に取得するゲームオブジェクト
    }

    // Update is called once per frame
    public void Update()
    {
        GetComponent<PlayerSelectBase>().Update();

        GetComponent<PlayerSelectBase>().Select(start, "SaveScene");
        GetComponent<PlayerSelectBase>().Select(option, "OptionScene");

        if (_select == end)  //終了を選んでいた場合
        {
            //適当に「選んでいる」ということがわかるようにしてくれ
            if (_A > 0)
            {
                //終了処理
            }
        }


        if (_lsv > 0)  //上に行く
        {
            if(_select == start)
            {
                _select = end;  //終了選択
            }
            else if(_select == end)
            {
                _select = option;  //設定選択
            }
            else if(_select == option)
            {
                _select = start;  //スタート選択
            }
        }

        if(_lsv < 0) //下に行く
        {
            if(_select == start)
            {
                _select = option;   //設定選択
            }
            else if(_select == option)
            {
                _select = end;   //終了選択
            }
            else if(_select == end)
            {
                _select = start;  //スタート選択
            }
        }
    }
}
