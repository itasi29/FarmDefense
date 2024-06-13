using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Start : MonoBehaviour
{
    public void Change_Scene_Click()
    {
        SceneManager.LoadScene("SaveScene");  //セーブシーンへ遷移
    }
}
