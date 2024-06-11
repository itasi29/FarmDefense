using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
 
    void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) //下キーを押す
        {
            OnSelected();
        }
        else if(Input.GetKeyUp(KeyCode.UpArrow))  //上キーを押す
        {

        }
    }

    public void OnSelected()
    {
        GameObject nowObj = EventSystem.current.currentSelectedGameObject; //選択中のオブジェクト取得

        if (nowObj.TryGetComponent<Selectable>(out var sr))
        {
            Selectable mySelectable = sr;  //Selectableがある場合

            EventSystem.current.SetSelectedGameObject(mySelectable.navigation.selectOnDown.gameObject); //一つ下のオブジェクト選択
        }
    }
}
