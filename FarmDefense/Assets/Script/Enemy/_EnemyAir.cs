using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class _EnemyAir : _EnemyBase
{
    [SerializeField] private const float kAngleSpeed = 180 / Mathf.PI * 0.2f;

    public int m_enemyAirPosY;  //��̃G�l�~�[��Y���W�̃|�W�V����
    private bool _enemyAirMove;  //�_��ɂނ����s���t���O
    private bool _circularmotion;  //�~�^��������t���O
    private bool _airAttak;  //�G���U������t���O
    private bool _airReturn;  //�G���󒆂ɖ߂�t���O
    private float _airAttakTime; //�G���U������܂ł̊Ԋu
    float x;  //X��
    float z;  //Z��
    private Vector3 pos;

    private Vector3 _center;  //���S�_

    private Vector3 _axis = Vector3.up;   //��]��

    private float _period = 5; //�~�^������

    /// <summary>
    /// �X�^�[�g�֐�
    /// </summary>
    void Start()
    {
#if false
        targetBase = GameObject.Find("Farm").gameObject;
        player = GameObject.Find("Player").gameObject;

        _airAttakTime = 0.0f;
        _enemyAirMove = false;
        _airAttak = false;
        _circularmotion = false;
        m_enemyAirPosY = 5;

        _airReturn = false;

        FindFarm();
#endif
        Init(this.transform.position);
    }

    /// <summary>
    /// �X�V����
    /// </summary>
    new void Update()
    {
    }

    /// <summary>
    /// ����������
    /// </summary>
    public override void Init(Vector3 pos)
    {
        targetBase = GameObject.Find("Farm").gameObject;
        player = GameObject.Find("Player").gameObject;

        _airAttakTime = 0.0f;
        _enemyAirMove = false;
        _airAttak = false;
        _circularmotion = false;
        m_enemyAirPosY = 5;

        _airReturn = false;

        base.Init(pos);
        FindFarm();
    }

    protected override void FindFarm()
    {
        int childIdx = 0;

        bool isFirst = true;
        float dis = 0.0f;

        Vector3 pos = this.transform.position;

        for (int i = 0; i < FarmManager.kFarmNum; ++i)
        {
            Farm tempFarm = targetBase.transform.GetChild(i).gameObject.GetComponent<Farm>();
            if (tempFarm.IsBreak) continue;

            if (isFirst)
            {
                Vector3 childPos = targetBase.transform.GetChild(i).transform.position;
                dis = (pos - childPos).sqrMagnitude;
                childIdx = i;

                isFirst = false;
            }
            else
            {
                Vector3 childPos = targetBase.transform.GetChild(i).transform.position;
                var childSqrLen = (pos - childPos).sqrMagnitude;

                if (dis < childSqrLen)
                {
                    dis = childSqrLen;
                    childIdx = i;
                }
            }

        }

        target = targetBase.transform.GetChild(childIdx).gameObject;
        farm = target.GetComponent<Farm>();
    }

    /// <summary>
    /// ���������̍X�V����
    /// </summary>
    public override void FixedUpdate()
    {
        if (farm.IsBreak)
        {
            FindFarm();
            _circularmotion = false;
            _enemyAirMove = false;
        }

        _isFindPlayer = farm.IsInPlayer;

        if (_isFindPlayer == true)   //m_player��true�Ȃ�v���C���[�֌�����
        {
            Transform transform = this.transform;  //�I�u�W�F�N�g���擾

            //            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, _speed * Time.deltaTime);  //�v���C���[�Ɍ�����
            Vector3 pos = transform.position;
            Vector3 tarPos = player.transform.position;

            Vector3 move = (tarPos - pos).normalized * _speed;

            transform.position = pos + move;
        }
        else if(_isFindPlayer == false)   //m_player��false�Ȃ�
        {
            if (_circularmotion == false && _enemyAirMove == false)    //_circularmotion��false�A_enemyAirMove��false�Ȃ�_��֌������s��(�ŏ�����)
            {
                pos = transform.position;

                Vector3 farm = new Vector3(target.transform.position.x, m_enemyAirPosY, target.transform.position.z);

                float timer = 0;

                timer += Time.deltaTime;

                Vector3 move = (farm - pos).normalized  * _speed;

                transform.position = pos + move;

            }
            if (_airReturn == true)  //�󒆂ɖ߂�t���O��true�Ȃ�
            {
                pos = transform.position;

                Vector3 farm = new Vector3(target.transform.position.x - 2.0f, m_enemyAirPosY, target.transform.position.z);

                float timer = 0;

                timer += Time.deltaTime;

                Vector3 move = (farm - pos).normalized * _speed;

                transform.position = pos + move;

                if (pos.y >= 4.9f)
                {
                    _circularmotion = true;  //����s����������

                    _airReturn = false; //�󒆂ɖ߂��Ă���
                }
            }
            else if (_circularmotion == true)   //_circularmotion��true�Ȃ����s��
            {
                _airAttakTime += Time.deltaTime;  //�U���܂ł̊Ԋu��i�߂�


                _center = target.transform.position;  //���S�_���^�[�Q�b�g�̍��W�ɂ���

                var tr = transform;

                var angleAxis = Quaternion.AngleAxis(kAngleSpeed * _period * Time.deltaTime, _axis);  //��]�̃N�H�[�^�j�I���쐬

                var circlepos = tr.position;  //�~�^���̈ʒu�v�Z

                Debug.Log("center" + _center);
                Debug.Log("befor" + circlepos);
                circlepos -= _center;
                Debug.Log("-" + circlepos);
                circlepos = angleAxis * circlepos;
                Debug.Log("q" + circlepos);
                circlepos += _center;
                Debug.Log("+" + circlepos);

                tr.position = circlepos;


                if (_airAttakTime >= 10.0f)
                {
                    _airAttak = true;  //�U���t���O���\�ɂ���

                    _circularmotion = false;  //����s������߂�

                }
            }
            else if (_airAttak == true) //�U���t���O��true�Ȃ�
            {
                base.FixedUpdate();   //�_��ɍU������
            }
        }

        
        
    }

    /// <summary>
    /// �_��̓����蔻��
    /// </summary>
    /// <param name="collision"></param>
    public override void OnCollisionStay(Collision collision)
    {
        base.OnCollisionStay(collision);

        if (collision.gameObject.tag == "Farm")
        {
            _airAttak = false;  //�U����������false�ɖ߂�

            _airReturn = true;  //�󒆂ɖ߂�

            _airAttakTime = 0;  //�����ɖ߂�
        }
        else if(collision.gameObject.tag == "Player")  //Player�ɓ���������
        {
            _airReturn = true;  //�󒆂ɖ߂�
        }
    }

    private new void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == target.name)  //���̂ɓ���������
        {
            if(_circularmotion == false�@&& _airAttak == false) //_circularmotion��false��_airAttack��false�Ȃ�
            {
                _circularmotion = true;  //_circularmotion��true�ɂ���

                _enemyAirMove = true;  //�_��ɓ��B����
            }
        }

        farm.OnTriggerEnter(collision);  //�_��̍��G�͈�
    }

    /// <summary>
    /// �_��̍��G�͈�
    /// </summary>
    /// <param name="collision"></param>
    private new void OnTriggerExit(Collider collision)
    {
        farm.OnTriggerExit(collision);  //�_��̍��G�͈�
    }

}
