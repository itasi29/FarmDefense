using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    [SerializeField] private Image FarmImage;
    [SerializeField] private Image PlayerImage;
    //[SerializeField] private Image FarmImage2;
    //[SerializeField] private Image FarmImage3;
    //[SerializeField] private Image FarmImage4;
    //[SerializeField] private Image FarmImage5;
    //[SerializeField] private Image FarmImage6;

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
    private Image _playerImage;
    private Vector3[] _farmPos = new Vector3[kFarmNum];
    private Vector3 _playerPos;
    private Vector3[] _showFarmPos = new Vector3[kFarmNum];
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
            _farmImage[i].transform.parent = _canvas.transform;
        }
        _playerImage = Instantiate(PlayerImage,new Vector3(1000,1000,1000),Quaternion.identity);
        _playerImage.transform.parent = _canvas.transform;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < kFarmNum; i++)
        {
            GameObject farm = _farm.transform.GetChild(i).gameObject;

            _farmPos[i] = farm.transform.position;

            //マップの大きさで座標を割り、全体の座標の割合を求める
            _showFarmPos[i].x = (_farmPos[i].x / kMapH) * kMiniMapScale;
            _showFarmPos[i].y = (_farmPos[i].z / kMapV) * kMiniMapScale;


            _showFarmPos[i].x += kMiniMapStartPosX;
            _showFarmPos[i].y += kMiniMapStartPosY;

            _farmImage[i].rectTransform.localPosition = _showFarmPos[i];
        }
        _playerPos = _player.transform.position;
        //マップの大きさで座標を割り、全体の座標の割合を求める
        _playerPos.x = (_playerPos.x / kMapH) * kMiniMapScale;
        _playerPos.y = (_playerPos.z / kMapV) * kMiniMapScale;

        _playerPos.x += kMiniMapStartPosX;
        _playerPos.y += kMiniMapStartPosY;

        _playerImage.rectTransform.localPosition = _playerPos;

    }
}
