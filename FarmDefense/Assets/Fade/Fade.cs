using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    private Image _image;
    private Color _color;
    private bool _fadeIn;
    private bool _fadeNow;
    // Start is called before the first frame update
    void Start()
    {
        _fadeNow = true;
        _fadeIn = false;
        _image = GameObject.Find("Panel").gameObject.GetComponent<Image>();
        _color = _image.color;
        _color.a = 0.9999f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("A"))
        {
            StartFadeOut();
        }
        
        if (Input.GetButton("B"))
        {
            StartFadeIn();
        }

        if (!_fadeIn)
        {
            Debug.Log(_color.a);
            _color.a-=0.01f;
            if (_color.a < 0)
            {
                _color.a = 0;
                _fadeNow = false;
            }
        }
        else
        {
            _color.a += 0.01f;
            if (_color.a > 1)
            {
                _color.a = 1;
                _fadeNow=false;
            }
        }
        _image.color = _color;
    }
    bool GetFadeEnd()
    {
        if (!_fadeNow && _color.a == 0 || _color.a == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void StartFadeOut()
    {
        _fadeIn=true;
        _fadeNow = true;
    }
    void StartFadeIn()
    {
        _fadeIn=false;
        _fadeNow = true;
    }
}