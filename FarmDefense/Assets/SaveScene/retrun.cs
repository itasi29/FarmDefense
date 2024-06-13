using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class retrun : MonoBehaviour
{
   public void Change_Scene_Click()
    {
        SceneManager.LoadScene("TitleScene");  //タイトルシーンへ遷移
    }
}
