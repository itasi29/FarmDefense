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
    private Slider _hpBar;
    private Slider _staminaBar;
    private EnemyPopNum _enemyPopNum;
    private EnemyPopNum _enemyKillNum;
    private SpawnerManager _spawnerManager;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").gameObject;
        _useWeapon = GameObject.Find("UseWeaponUI").GetComponent<Image>();
        _enemyPopNum.tens = GameObject.Find("MaxTensPlace").GetComponent<Image>();
        _enemyPopNum.ones = GameObject.Find("MaxOnesPlace").GetComponent<Image>();
        _enemyKillNum.tens = GameObject.Find("KillTensPlace").GetComponent<Image>();
        _enemyKillNum.ones = GameObject.Find("KillOnesPlace").GetComponent<Image>();
        _spawnerManager = GameObject.Find("SpawnerManager").GetComponent<SpawnerManager>();
        _hpBar = GameObject.Find("HpBar").GetComponent<Slider>();
        _staminaBar = GameObject.Find("StaminaBar").GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
         _weaponType = _player.GetComponent<Player>().NowWeaponType;
        //�����g���Ă��邩�e���g���Ă��邩�\��
        if(_weaponType == Player.WeaponType.kNear)
        {
            _useWeapon.sprite = _sprites[10];
        }
        else 
        {
            _useWeapon.sprite= _sprites[11];
        }
        //�E�F�[�u�̓G�̐��\��
        int enemyMaxPopNum = _spawnerManager.WaveEnemyNum;
        _enemyPopNum.tens.sprite = _sprites[(int)Math.Floor(enemyMaxPopNum * 0.1)];
        _enemyPopNum.ones.sprite = _sprites[enemyMaxPopNum % 10];
        //�|�����G�̐��\��
        int enemyKillNum = _spawnerManager.KillEnemyNum;
        _enemyKillNum.tens.sprite = _sprites[(int)Math.Floor(enemyKillNum * 0.1)];
        _enemyKillNum.ones.sprite = _sprites[enemyKillNum % 10];


    }
}
