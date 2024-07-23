using System.Collections;
using System.Collections.Generic;
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


    [SerializeField] private Image FarmImage;
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

    private GameObject _canvas;
    private GameObject _player;
    private GameObject _farm;

    private Image[] _farmImage = new Image[kFarmNum];
    private Vector3[] _farmPos = new Vector3[kFarmNum];

    private List<EnemyInfo> _enemyList = new List<EnemyInfo>();
    private List<GameObject> _deathEnemyList = new List<GameObject>();

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
            _farmImage[i].transform.localScale = new Vector2(kFarmImageSize,kFarmImageSize);

        }
        _playerImage = Instantiate(PlayerImage, new Vector3(1000, 1000, 1000), Quaternion.identity);
        _playerImage.transform.SetParent(_canvas.transform);
        _playerImage.transform.localScale = new Vector2(kPlayerImageSize,kPlayerImageSize);
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        for (int i = 0; i < kFarmNum; i++)
        {
            GameObject farm = _farm.transform.GetChild(i).gameObject;

            _farmPos[i] = farm.transform.position;

            //�}�b�v�̑傫���ō��W������A�S�̂̍��W�̊��������߂�
            _farmPos[i].x = (_farmPos[i].x / kMapH) * kMiniMapScale;
            _farmPos[i].y = (_farmPos[i].z / kMapV) * kMiniMapScale;


            _farmPos[i].x += kMiniMapStartPosX;
            _farmPos[i].y += kMiniMapStartPosY;

            _farmImage[i].rectTransform.localPosition = _farmPos[i];
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
            Debug.Log("�S���폜");
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
