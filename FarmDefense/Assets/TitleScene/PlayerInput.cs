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
        if (Input.GetKeyDown(KeyCode.Tab)) //���L�[������
        {
            OnSelected();
        }
        else if(Input.GetKeyUp(KeyCode.UpArrow))  //��L�[������
        {

        }
    }

    public void OnSelected()
    {
        GameObject nowObj = EventSystem.current.currentSelectedGameObject; //�I�𒆂̃I�u�W�F�N�g�擾

        if (nowObj.TryGetComponent<Selectable>(out var sr))
        {
            Selectable mySelectable = sr;  //Selectable������ꍇ

            EventSystem.current.SetSelectedGameObject(mySelectable.navigation.selectOnDown.gameObject); //����̃I�u�W�F�N�g�I��
        }
    }
}
