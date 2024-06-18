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
    [SerializeField] protected int _back_to_scene;

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

        Debug.Log(_select);  //デバッグ表示
    }

    /// <summary>
    /// Aボタンを押したときの反応
    /// </summary>
    public void Select(Image image, int backscene, string scene)
    {
        if(_select == image)
        {
            if(_A > 0)
            {
                _back_to_scene = backscene;  //設定画面から戻るSceneを選別するための変数

                SceneManager.LoadScene(scene);
            }
        }
    }
}
