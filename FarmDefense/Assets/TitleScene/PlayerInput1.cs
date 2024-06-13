using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 選択用スクリプト(コントローラ)
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

        select = start;  //最初に取得するゲームオブジェクト
    }

    // Update is called once per frame
    void Update()
    {
        //L Stick
        lsh = Input.GetAxis("Horizontal");
        lsv = Input.GetAxis("Vertical");
        A = Input.GetKeyDown(KeyCode.A);


        if(select == start) //スタートを選んでいた場合
        {
            //適当に「選んでいる」ということがわかるようにしてくれ
            if(A)
            {
                SceneManager.LoadScene("SaveScene");  //セーブシーンへ遷移
            }
        }
        else if(select == setting)  //設定を選んでいた場合
        {
            //適当に「選んでいる」ということがわかるようにしてくれ
            if (A)
            {
                SceneManager.LoadScene("OptionScene");  //設定画面へ遷移
            }
        }
        else if (select == end)  //終了を選んでいた場合
        {
            //適当に「選んでいる」ということがわかるようにしてくれ
            if (A)
            {
                //終了処理
            }
        }


        if (lsv > 0)  //上に行く
        {
            if(select == start)
            {
                select = end;  //終了選択
            }
            else if(select == end)
            {
                select = setting;  //設定選択
            }
            else if(select == setting)
            {
                select = start;  //スタート選択
            }
        }

        if(lsv < 0) //下に行く
        {
            if(select == start)
            {
                select = setting;   //設定選択
            }
            else if(select == setting)
            {
                select = end;   //終了選択
            }
            else if(select == end)
            {
                select = start;  //スタート選択
            }
        }

        Debug.Log(select);  //選択してるゲームオブジェクトの名前
    }
}
