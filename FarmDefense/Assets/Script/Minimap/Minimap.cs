using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using TMPro;
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


    [SerializeField] private Slider FarmSlider;
    [SerializeField] private Image PlayerImage;
    [SerializeField] private Image EnemyImage;

    private const float kEnemyImageSize = 0.4f;
    private const float kFarmImageSize = 0.75f;
    private const float kPlayerImageSize = 0.5f;
    private const int kFarmNum = 6;
    private const float kViewHeight = 1.0f;

    private const int kMapH = (int)(110 * kViewHeight);
    private const int kMapV = (int)(150 * kViewHeight);

    private const int kMiniMapScale = 768;

    private const int kMiniMapStartPosX = 480;
    private const int kMiniMapStartPosY = 200;

    private const float kChangeColorFarmHpRate = 0.4f;

    private GameObject _canvas;
    private GameObject _player;
    private GameObject _farm;

    private Slider[] _farmHpSlider = new Slider[kFarmNum];
    private Slider _farmTotalHPSlider;
    private Vector3[] _farmPos = new Vector3[kFarmNum];

    private List<EnemyInfo> _enemyList = new List<EnemyInfo>();
    private List<GameObject> _deathEnemyList = new List<GameObject>();

    private Image _playerImage;
    private Vector3 _playerPos;
    // Start is called before the first frame update
    void Start()
    {
        _farm = GameObject.Find("Farm");
        _farmTotalHPSlider = GameObject.Find("FarmTotalHpBar").GetComponent<Slider>();
        _canvas = GameObject.Find("Canvas");
        _player = GameObject.Find("Player");
        _playerPos = _player.transform.position;
        //Instantiate(FarmImage,,Quaternion.identity);
        //農場の画像を表示
        for (int i = 0; i < kFarmNum; i++)
        {
            _farmHpSlider[i] = Instantiate(FarmSlider, new Vector3(1000, 1000, 1000), Quaternion.identity);
            _farmHpSlider[i].transform.SetParent(_canvas.transform);
            _farmHpSlider[i].transform.localScale = new Vector2(kFarmImageSize,kFarmImageSize);
        }
        //農場のミニマップ座標設定
        for (int i = 0; i < kFarmNum; i++)
        {
            GameObject farm = _farm.transform.GetChild(i).gameObject;

            _farmPos[i] = farm.transform.position;

            //マップの大きさで座標を割り、全体の座標の割合を求める
            _farmPos[i].x = (_farmPos[i].x / kMapH) * kMiniMapScale;
            _farmPos[i].y = (_farmPos[i].z / kMapV) * kMiniMapScale;


            _farmPos[i].x += kMiniMapStartPosX;
            _farmPos[i].y += kMiniMapStartPosY;

            _farmHpSlider[i].transform.localPosition = _farmPos[i];
            _farmHpSlider[i].transform.localRotation = Quaternion.AngleAxis(90, Vector3.forward);
        }
        _playerImage = Instantiate(PlayerImage, new Vector3(1000, 1000, 1000), Quaternion.identity);
        _playerImage.transform.SetParent(_canvas.transform);
        _playerImage.transform.localScale = new Vector2(kPlayerImageSize,kPlayerImageSize);
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {

        float farmTotalHp = 0;
        float farmTotalMaxHp = 0;
        for (int i = 0; i < kFarmNum; i++)
        {
            Farm farmScript = _farm.transform.GetChild(i).gameObject.GetComponent<Farm>();

            farmTotalHp += farmScript.Hp;
            farmTotalMaxHp += farmScript.MaxHp;

            float hpRate = (float)farmScript.Hp /(float)farmScript.MaxHp;

            if(hpRate < 0)
            {
                hpRate = 0;
            }

            _farmHpSlider[i].value = hpRate;
            //体力の割合が一定よりも下になったらスライダーの色を変える
            if(hpRate < kChangeColorFarmHpRate)
            {
                Image sliderImage = _farmHpSlider[i].transform.GetChild(1).GetChild(0).GetComponent<Image>();

                sliderImage.color = Color.yellow;
            }
        }

        float farmTotalHpRate = farmTotalHp / farmTotalMaxHp;

        _farmTotalHPSlider.value = farmTotalHpRate;

        if (farmTotalHpRate < kChangeColorFarmHpRate)
        {
            Image sliderImage = _farmTotalHPSlider.transform.GetChild(1).GetChild(0).GetComponent<Image>();

            sliderImage.color = Color.yellow;
        }

        _playerPos = _player.transform.position;
        //マップの大きさで座標を割り、全体の座標の割合を求める
        _playerPos.x = (_playerPos.x / kMapH) * kMiniMapScale;
        _playerPos.y = (_playerPos.z / kMapV) * kMiniMapScale;

        _playerPos.x += kMiniMapStartPosX;
        _playerPos.y += kMiniMapStartPosY;

        _playerImage.rectTransform.localPosition = _playerPos;

        //生きている敵リストをすべて削除するかどうか
        bool isAllDeath = true;
        //今生きている敵リストをすべて回す
        foreach (var item in _enemyList)
        {
            bool isDeath = false;
            //死んだ敵リストを回す
            foreach (var death in _deathEnemyList)
            {
                //生きている敵リストに死んだ敵が入っていたら
                if (item.enemy == death)
                {
                    //表示を消す
                    Destroy(item.image);
                    //下で消したimageにアクセスしないようにフラグを立てる
                    isDeath = true;
                }
                //生きているリストと死んでいる敵リストが完全に一致したら
                else
                {
                    //すべて死んだと判定する
                    isAllDeath = false;
                }
            }
            //上で消されていなかったら
            if (!isDeath)
            {
                //画像の座標を動かす
                item.image.rectTransform.localPosition = new Vector2((item.enemy.transform.position.x / kMapH) * kMiniMapScale + kMiniMapStartPosX,
                                                                     (item.enemy.transform.position.z / kMapV) * kMiniMapScale + kMiniMapStartPosY);
            }
        }
        //今いる敵がすべて死んだら
        if (isAllDeath && _deathEnemyList.Count != 0)
        {
            //生きている敵リストをすべて削除する
            _enemyList.Clear();
        }

    }
    public void EntryMiniMapEnemy(GameObject gameObject)
    {
        EnemyInfo addEnemy;

        addEnemy.enemy = gameObject;

        addEnemy.image = EnemyImage;

        addEnemy.showPos = gameObject.transform.position;

        addEnemy.image = Instantiate(addEnemy.image, addEnemy.showPos, Quaternion.identity);

        addEnemy.image.transform.SetParent(_canvas.transform);

        addEnemy.image.transform.localScale = new Vector2(kEnemyImageSize,kEnemyImageSize);

        _enemyList.Add(addEnemy);
    }
    public void EntryDeathEnemyList(GameObject gameObject)
    {
        _deathEnemyList.Add(gameObject);
    }
}
