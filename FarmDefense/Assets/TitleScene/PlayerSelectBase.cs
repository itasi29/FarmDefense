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
            if(_A > 0)
            {
                SceneManager.LoadScene(scene);
            }
        }
    }
}
