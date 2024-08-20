using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    private const string kNextSceneName = "StageSelectScene";

    private Fade _fade;

    [SerializeField] private string _victoryBgmID;
    [SerializeField] private string _loseBgmID;
    
    void Start()
    {
        _fade = GetComponent<Fade>();

        var director = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        var result = director.ResultData;
        var dataMgr = director.DataMgr;
        var stage = dataMgr.Spawner;
        var user = dataMgr.User;
        

        var rankTxt = GameObject.Find("rank").GetComponent<TextMeshProUGUI>();
        var moneyTxt = GameObject.Find("money").GetComponent<TextMeshProUGUI>();

        // S : 80
        // A : 60
        // B : 40
        // C : 20
        // D : 0
        // E : é∏îs
        if (result.IsCrear)
        {
            float par = result.TotalFarmHp / (float)(Farm.kMaxHp * FarmManager.kFarmNum);

            int money = stage.GetStaticMoney(result.StageNo) + (int)(par * stage.GetDynamicMoney(result.StageNo));

            if (par > 0.9f)
            {
                rankTxt.text = "S";
            }
            else if (par > 0.75f)
            {
                rankTxt.text = "A";
            }
            else if (par > 0.50f)
            {
                rankTxt.text = "B";
            }
            else if (par > 0.30f)
            {
                rankTxt.text = "C";
            }
            else
            {
                rankTxt.text = "D";
            }

            moneyTxt.text = "ïÒèVÅF" + money.ToString("D4");

            if (result.StageNo + 1 < 6)
            {
                user.ChangeStageClear(result.StageNo + 1);
            }
            user.AddMoney(money);

            _fade.SetBgmID(_victoryBgmID);
        }
        else
        {
            rankTxt.text = "E";
            moneyTxt.text = "";

            _fade.SetBgmID(_loseBgmID);
        }
    }

    
    void Update()
    {
        if (Input.GetButtonDown("A"))
        {
            _fade.StartFadeOut(kNextSceneName);
        }
    }
}
