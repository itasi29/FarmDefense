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
    [SerializeField] public Image setting;
    [SerializeField] public Image back;
    // Start is called before the first frame update
    public override void Start()
    {
        save1 = GameObject.Find("Canvas/SaveSelect/Save1").GetComponent<Image>();
        save2 = GameObject.Find("Canvas/SaveSelect/Save2").GetComponent<Image>();
        save3 = GameObject.Find("Canvas/SaveSelect/Save3").GetComponent<Image>();
        save4 = GameObject.Find("Canvas/SaveSelect/Save4").GetComponent<Image>();
        setting = GameObject.Find("Canvas/Setting").GetComponent<Image>();
        back = GameObject.Find("Canvas/back").GetComponent<Image>();

        _select = save1;  //�ŏ��ɒl������

    }

    // Update is called once per frame
    public void Update()
    {
        GetComponent<PlayerSelectBase>().Update();

        if(_select == save1)  //�Z�[�u1�ɔ��
        {
            if(_A > 0)
            {
                
            }
        }
        else if(_select == save2)  //�Z�[�u2�ɔ��
        {
            if(_A > 0)
            {

            }
        }
        else if(_select == save3) //�Z�[�u3�ɔ��
        {
            if(_A > 0)
            {

            }
        }
    }
}
