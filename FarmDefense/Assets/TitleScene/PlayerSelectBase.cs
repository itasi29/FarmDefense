using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerSelectBase : MonoBehaviour
{

    [SerializeField] public float _lsh;
    [SerializeField] public float _lsv;
    [SerializeField] public float _A;
    [SerializeField] public Image _select;

    // Start is called before the first frame update
    public virtual void Start()
    {
    }

    // Update is called once per frame
    public void Update()
    {
        _lsh = Input.GetAxis("Horizontal");
        _lsv = Input.GetAxis("Vertical");
        _A = Input.GetAxis("A");

        Debug.Log(_select);  //�f�o�b�O�\��
    }

    /// <summary>
    /// A�{�^�����������Ƃ��̔���
    /// </summary>
    public void Select(Image image, string scene)
    {
        if(_select == image)
        {
            //�K���Ɂu�I��ł���v�Ƃ������Ƃ��킩��悤�ɂ��Ă���
            if (_A > 0)
            {

                SceneManager.LoadScene(scene);
            }
        }
    }

    /// <summary>
    /// �ݒ�ł̍��E�Z���N�g
    /// </summary>
    /// <param name="image1"></param>
    /// <param name="image2"></param>
    /// <param name="image3"></param>
    /// <param name="image4"></param>
    /// <param name="image5"></param>
    //public void OptionSelect(Image image1,Image image2,Image image3,Image image4,Image image5)
    //{
    //    if(_selectAssignment == image1)
    //    {
    //        _selectAssignment = image2;
    //    }
    //    else if(_selectAssignment == image2)
    //    {
    //        _selectAssignment = image3;
    //    }
    //    else if(_selectAssignment == image3)
    //    {
    //        _selectAssignment = image4;
    //    }
    //    else if(_selectAssignment == image4)
    //    {
    //        _selectAssignment = image5;
    //    }
    //}
}
