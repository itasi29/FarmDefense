using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    public struct EnemyInfo
    {
        public GameObject enemy;
        public Vector3 showPos;
        public Image image;
    }


    [SerializeField] private Image FarmImage;
    [SerializeField] private Image PlayerImage;
    [SerializeField] private Image EnemyImage;

    private const int kFarmNum = 6;
    private const int kMapH = 100;
    private const int kMapV = 150;

    private const int kMiniMapScale = 256;

    private const int kMiniMapStartPosX = 480;
    private const int kMiniMapStartPosY = 200;

    private GameObject _canvas;
    private GameObject _player;
    private GameObject _farm;

    private Image[] _farmImage = new Image[kFarmNum];
    private Vector3[] _farmPos = new Vector3[kFarmNum];

    private List<EnemyInfo> _enemyList = new List<EnemyInfo>();

    private Image _playerImage;
    private Vector3 _playerPos;
    // Start is called before the first frame update
    void Start()
    {
        _farm = GameObject.Find("Farm");
        _canvas = GameObject.Find("Canvas");
        _player = GameObject.Find("Player");
        _playerPos = _player.transform.position;
        //Instantiate(FarmImage,,Quaternion.identity);
        for (int i = 0; i < kFarmNum; i++)
        {
            _farmImage[i] = Instantiate(FarmImage, new Vector3(1000, 1000, 1000), Quaternion.identity);
            _farmImage[i].transform.SetParent(_canvas.transform);
        }
        _playerImage = Instantiate(PlayerImage, new Vector3(1000, 1000, 1000), Quaternion.identity);
        _playerImage.transform.SetParent(_canvas.transform);
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        for (int i = 0; i < kFarmNum; i++)
        {
            GameObject farm = _farm.transform.GetChild(i).gameObject;

            _farmPos[i] = farm.transform.position;

            //マップの大きさで座標を割り、全体の座標の割合を求める
            _farmPos[i].x = (_farmPos[i].x / kMapH) * kMiniMapScale;
            _farmPos[i].y = (_farmPos[i].z / kMapV) * kMiniMapScale;


            _farmPos[i].x += kMiniMapStartPosX;
            _farmPos[i].y += kMiniMapStartPosY;

            _farmImage[i].rectTransform.localPosition = _farmPos[i];
        }
        _playerPos = _player.transform.position;
        //マップの大きさで座標を割り、全体の座標の割合を求める
        _playerPos.x = (_playerPos.x / kMapH) * kMiniMapScale;
        _playerPos.y = (_playerPos.z / kMapV) * kMiniMapScale;

        _playerPos.x += kMiniMapStartPosX;
        _playerPos.y += kMiniMapStartPosY;

        _playerImage.rectTransform.localPosition = _playerPos;

        foreach (var item in _enemyList)
        {
            if (item.enemy.active == false)
            {
                _enemyList.Remove(item);
            }
        }


    }
    public void EntryMiniMapEnemy(GameObject gameObject)
    {
        EnemyInfo addEnemy;

        addEnemy.enemy = gameObject;

        addEnemy.image = EnemyImage;

        addEnemy.showPos = gameObject.transform.position;

        //マップの大きさで座標を割り、全体の座標の割合を求める
        addEnemy.showPos.x = (addEnemy.showPos.x / kMapH) * kMiniMapScale;
        addEnemy.showPos.y = (addEnemy.showPos.z / kMapV) * kMiniMapScale;


        addEnemy.showPos.x += kMiniMapStartPosX;
        addEnemy.showPos.y += kMiniMapStartPosY;

        addEnemy.image.rectTransform.localPosition = addEnemy.showPos;

        addEnemy.image = Instantiate(addEnemy.image, addEnemy.showPos, Quaternion.identity);

        addEnemy.image.transform.SetParent(_canvas.transform);

        _enemyList.Add(addEnemy);
    }
}
