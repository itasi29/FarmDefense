using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    float lsh;
    float lsv;
    //float dph;
    //float dpv;
    Button end;
    Button setting;
    Button start;

    GameObject A;
    void Start()
    {
        end = GameObject.Find("Canvas/GameObject/end/EndButton").GetComponent<Button>();
        setting = GameObject.Find("Canvas/GameObject/setting/SettingButton").GetComponent<Button>();
        start = GameObject.Find("Canvas/GameObject/start/StartButton").GetComponent<Button>();

        start.Select();
    }

    private void Update()
    {
        //L Stick
        lsh = Input.GetAxis("Horizontal");
        lsv = Input.GetAxis("Vertical");

        //D-Pad
        //dph = Input.GetAxis("D_Pad_H");
        //dpv = Input.GetAxis("D_Pad_V");

        if ((lsh != 0) || (lsv != 0))
        {
            Debug.Log("L stick:" + lsh + "," + lsv);
        }
        //if ((dph != 0) || (dpv != 0))
        //{
        //    Debug.Log("D Pad:" + dph + "," + dpv);
        //}

        //start�̏�ɍs���ꍇ
        if (lsv > 0 && this == start)
        {
            end.Select();
        }
        //end�̉��ɍs���ꍇ
        if(lsv < 0 && this == end)
        {
            start.Select();
        }

        /*�o�O�ɂ���*/
        //lsv��0���瓮���ƂɂȂ��Ă��邽�߁A���̃I�u�W�F�N�g�����擾�����u�Ԃɏu�Ԉړ��̂悤�ɑI��ł���{�^�����ς��
    }
}
