using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OptionSystem
{
    GameObject _option;
    GameObject _optionPrefab;
    Transform _canvas;

    public OptionSystem()
    {
        _optionPrefab = (GameObject)Resources.Load("Prefab/Option");
        _canvas = GameObject.Find("Canvas").transform;
    }

    public bool IsOpenOption()
    {
        return _option;
    }

    public void Create(OptionManager.ReturnScene returnScene, Func<GameObject, Transform, GameObject> instantiate)
    {
        _option = instantiate(_optionPrefab, _canvas);
        _option.GetComponent<OptionManager>().Init(returnScene);
    }
}
