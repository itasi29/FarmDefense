using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Setting : MonoBehaviour
{
    public void Change_Scene_Click()
    {
        SceneManager.LoadScene("OptionScene");  //�ݒ��ʂ֑J��
    }
}
