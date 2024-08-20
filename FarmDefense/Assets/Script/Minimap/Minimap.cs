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
        //�_��̉摜��\��
        for (int i = 0; i < kFarmNum; i++)
        {
            _farmHpSlider[i] = Instantiate(FarmSlider, new Vector3(1000, 1000, 1000), Quaternion.identity);
            _farmHpSlider[i].transform.SetParent(_canvas.transform);
            _farmHpSlider[i].transform.localScale = new Vector2(kFarmImageSize,kFarmImageSize);
        }
        //�_��̃~�j�}�b�v���W�ݒ�
        for (int i = 0; i < kFarmNum; i++)
        {
            GameObject farm = _farm.transform.GetChild(i).gameObject;

            _farmPos[i] = farm.transform.position;

            //�}�b�v�̑傫���ō��W������A�S�̂̍��W�̊��������߂�
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
            //�̗͂̊��������������ɂȂ�����X���C�_�[�̐F��ς���
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
        //�}�b�v�̑傫���ō��W������A�S�̂̍��W�̊��������߂�
        _playerPos.x = (_playerPos.x / kMapH) * kMiniMapScale;
        _playerPos.y = (_playerPos.z / kMapV) * kMiniMapScale;

        _playerPos.x += kMiniMapStartPosX;
        _playerPos.y += kMiniMapStartPosY;

        _playerImage.rectTransform.localPosition = _playerPos;

        //�����Ă���G���X�g�����ׂč폜���邩�ǂ���
        bool isAllDeath = true;
        //�������Ă���G���X�g�����ׂĉ�
        foreach (var item in _enemyList)
        {
            bool isDeath = false;
            //���񂾓G���X�g����
            foreach (var death in _deathEnemyList)
            {
                //�����Ă���G���X�g�Ɏ��񂾓G�������Ă�����
                if (item.enemy == death)
                {
                    //�\��������
                    Destroy(item.image);
                    //���ŏ�����image�ɃA�N�Z�X���Ȃ��悤�Ƀt���O�𗧂Ă�
                    isDeath = true;
                }
                //�����Ă��郊�X�g�Ǝ���ł���G���X�g�����S�Ɉ�v������
                else
                {
                    //���ׂĎ��񂾂Ɣ��肷��
                    isAllDeath = false;
                }
            }
            //��ŏ�����Ă��Ȃ�������
            if (!isDeath)
            {
                //�摜�̍��W�𓮂���
                item.image.rectTransform.localPosition = new Vector2((item.enemy.transform.position.x / kMapH) * kMiniMapScale + kMiniMapStartPosX,
                                                                     (item.enemy.transform.position.z / kMapV) * kMiniMapScale + kMiniMapStartPosY);
            }
        }
        //������G�����ׂĎ��񂾂�
        if (isAllDeath && _deathEnemyList.Count != 0)
        {
            //�����Ă���G���X�g�����ׂč폜����
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
