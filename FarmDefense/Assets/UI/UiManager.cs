using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public struct EnemyPopNum
    {
        public Image tens;
        public Image ones;
    }


    [SerializeField] Sprite[] _sprites;
    private Player.WeaponType _weaponType;
    private GameObject _player;
    private Image _useWeapon;
    private EnemyPopNum _enemyPopNum;
    private SpawnerManager _spawnerManager;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").gameObject;
        _useWeapon = GameObject.Find("UseWeaponUI").GetComponent<Image>();
        _enemyPopNum.tens = GameObject.Find("TensPlace").GetComponent<Image>();
        _enemyPopNum.ones = GameObject.Find("OnesPlace").GetComponent<Image>();
        _spawnerManager = GameObject.Find("SpawnerManager").GetComponent<SpawnerManager>();
    }

    // Update is called once per frame
    void Update()
    {
         _weaponType = _player.GetComponent<Player>().NowWeaponType;
        if(_weaponType == Player.WeaponType.kNear)
        {
            _useWeapon.sprite = _sprites[10];
        }
        else 
        {
            _useWeapon.sprite= _sprites[11];
        }
        int enemyMaxPopNum = _spawnerManager.WaveEnemyNum;
        _enemyPopNum.tens.sprite = _sprites[(int)Math.Floor(enemyMaxPopNum * 0.1)];
        _enemyPopNum.ones.sprite = _sprites[enemyMaxPopNum % 10];
    }
}
