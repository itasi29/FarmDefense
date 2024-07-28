using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    /* �^ */
    protected enum AnimParm
    {
        kAttack,
        kHit,
        kDeath,
        kMove,
    };

    /* �萔 */
    private const int kDecreaseHpSpeed = 1;   // _deltaHp�̌������x
    // �A�j���[�V�����̓����𐧌�
    private Dictionary<AnimParm, string> kAnimParmInfo = new Dictionary<AnimParm, string>()
    {
        { AnimParm.kAttack, "Attack" },
        { AnimParm.kHit,    "Hit" },
        { AnimParm.kDeath,  "Death" },
        { AnimParm.kMove,   "IsMove" },
    };

    /* �ϐ� */
    [SerializeField] protected EnemyStatus _status;    // �X�e�[�^�X
    private int _hp;                // ���݂�HP
    private int _deltaHp;           // �����𕪂���₷�����邽�߂̕ϐ�
    private int _watiAttackFrame;   // �U���ҋ@�t���[���p
    private bool _isExist;          // �����t���O
    private bool _isDeltaHp;        // �����t���O
    protected bool _isStopAttack;     // �U����~�t���O
    protected bool _isFindPlayer;     // �v���C���[�����t���O
    protected bool _isStopMove;       // ��~�t���O
    protected bool _isColPlayer;
    protected bool _isColFarm;
    private bool _isDeathAnim;
    protected Rigidbody _rb;
    private GameObject _farmBase;   // �_��S���������Ă���e�I�u�W�F�N�g
    protected GameObject _farm;     // �U������_��
    protected Farm _farmScript;     // ��̂̃X�N���v�g
    protected GameObject _player;   // �v���C���[
    private CameraControl _camera;  // �J�������
    private Minimap _minimap;       // �~�j�}�b�v
    private SpawnerManager _spawnerMgr; // �X�|�i�[�}�l�[�W���[
    protected Animator _anim;

    private SoundManager _soundMgr;

    /* �v���p�e�B */
    public int Hp { get { return _hp; } }
    public int DeltaHp { get { return _deltaHp; } }
    public int MaxHp { get { return _status.maxHp; } }
    public bool IsExist { get { return  _isExist; } }
    public bool IsFindPlayer { get { return _isFindPlayer; } set { _isFindPlayer = value; } }

    /// <summary>
    /// ������
    /// </summary>
    /// <param name="pos">�����ʒu</param>
    /// <param name="enemyNo">�G��ID</param>
    public virtual void Init(Vector3 pos, string enemyID)
    {
        _rb = GetComponent<Rigidbody>();

        _farmBase = GameObject.Find("Farm").gameObject;
        _player = GameObject.Find("Player").gameObject;
        _camera = GameObject.Find("Main Camera").GetComponent<CameraControl>();
        var stageMgr = GameObject.Find("StageManager");
        _minimap = stageMgr.GetComponent<Minimap>();
        _spawnerMgr = stageMgr.GetComponent<SpawnerManager>();
        _anim = GetComponent<Animator>();
        _anim.speed = 0.2f;
        _anim.SetBool(kAnimParmInfo[AnimParm.kMove], true);

        // �X�e�[�^�X�擾
        var director = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        _soundMgr = director.SoundMgr;
        EnemyData data = director.DataMgr.Enemy;
        _status = data.GetStatus(enemyID);

        // �e�평����
        _hp = _status.maxHp;
        _deltaHp = _status.maxHp;
        _watiAttackFrame = 0;
        _isExist = true;
        _isDeltaHp = false;
        _isStopAttack = false;
        _isFindPlayer = false;
        _isStopMove = false;
        _isColPlayer = false;
        _isColFarm = false;
        _isDeathAnim = false;
        transform.position = pos;
    }

    protected void OnCollisionEnter(Collision collision)
    {
        bool isPlayer = collision.gameObject.tag == "Player";
        bool isFarm = collision.gameObject.tag == "Farm";

        if (isPlayer || isFarm)
        {
            // �ǂ���Ƃ������������Ƃ��Ȃ���Έړ��A�j���[�V�����I��
            if (!_isColPlayer && !_isColFarm)
            {
                _anim.SetBool(kAnimParmInfo[AnimParm.kMove], false);
            }

            if (isPlayer)
            {
                _isStopMove = true;
                _isColPlayer = true;
            }
            else if (isFarm)
            {
                _isColFarm = true;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        bool isPlayer = collision.gameObject.tag == "Player";
        bool isFarm = collision.gameObject.tag == "Farm";

        if (isPlayer || isFarm) 
        {
            if (isPlayer)
            {
                _isStopMove = false;
                _isColPlayer = false;
            }
            else if (isFarm)
            {
                _isColFarm = false;
            }

            // �ǂ���Ƃ����ꂽ��ړ����J�n������
            if (!_isColPlayer && !_isColFarm)
            {
                _anim.SetBool(kAnimParmInfo[AnimParm.kMove], true);
            }
        }
    }

    protected void FrontUpdate()
    {
        if (_rb.velocity.sqrMagnitude > 0.0f)
        {
            Vector3 dir = _rb.velocity.normalized;

            transform.forward = dir;
        }
    }

    protected bool DeathAfterUpdate()
    {
        // �������͖���
        if (_isExist) return false;

        if (IsNowPlayClipName("Death"))
        {
            _isDeathAnim = true;
        }
        else if (_isDeathAnim)
        {
            // �J�����Ɏ��S�������Ƃ�`����
            _camera.SubHpBarInfo(this.gameObject);
            // �X�|�i�[�}�l�[�W���[�Ɏ��S�������Ƃ�`����
            _spawnerMgr.AddKilledEnemy();
            //�~�j�}�b�v�Ɏ��S�������Ƃ�`����
            _minimap.EntryDeathEnemyList(this.gameObject);
            // ���g��j��
            Destroy(this.gameObject);
        }

        return true;
    }

    protected bool IsNowPlayClipName(string clipName)
    {
        return _anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == clipName;
    }

    /// <summary>
    /// �_���[�W����
    /// </summary>
    /// <param name="damage">�_���[�W��</param>
    public void OnDamage(int damage)
    {
        _hp -= damage;
        Debug.Log("hp" + _hp);
        _isDeltaHp = true;
        _anim.SetTrigger(kAnimParmInfo[AnimParm.kHit]);

        _soundMgr.PlaySe("SE_10");

        // HP��0�ȉ��ɂȂ����玀�S����
        if (_hp <= 0)
        {
            _hp = 0;
            _isExist = false;
            _anim.SetTrigger(kAnimParmInfo[AnimParm.kDeath]);
            _anim.SetBool("Is" + kAnimParmInfo[AnimParm.kDeath], true);
        }
    }

    /// <summary>
    /// �_��Ɍ������Ă̈ړ�
    /// </summary>
    protected void MoveToFarm()
    {
        Vector3 pos = transform.position;
        Vector3 farmPos = _farm.transform.position;

        Vector3 velocity = (farmPos - pos).normalized * _status.speed;

        _rb.velocity = velocity;
    }

    /// <summary>
    /// �v���C���[�Ɍ������Ă̈ړ�
    /// </summary>
    protected void MoveToPlayer()
    {
        Vector3 pos = transform.position;
        Vector3 playerPos = _player.transform.position;

        Vector3 velocity = (playerPos - pos).normalized * _status.speed;

        _rb.velocity = velocity;
    }

    /// <summary>
    /// �_��ւ̍U������
    /// </summary>
    protected void AttackFarm()
    {
        // �U���ҋ@��ԂȂ炵�Ȃ�
        if (_isStopAttack) return;
        // �v���C���[�������Ȃ�_����U�����Ȃ�
        if (_isFindPlayer) return;

        _soundMgr.PlaySe("SE_9");
        _farmScript.OnDamage(_status.attack);
        _isStopAttack = true;
        _watiAttackFrame = _status.attackInterval;
        _anim.SetTrigger(kAnimParmInfo[AnimParm.kAttack]);
    }

    protected void AttackPlayer()
    {
        // �U���ҋ@��ԂȂ炵�Ȃ�
        if (_isStopAttack) return;
        // �v���C���[�񔭌����Ȃ�U�����Ȃ�
        if (!_isFindPlayer) return;

        _soundMgr.PlaySe("SE_9");
        _player.GetComponent<Player>().OnDamage(_status.attack);
        _isStopAttack = true;
        _watiAttackFrame = _status.attackInterval;
        _anim.SetTrigger(kAnimParmInfo[AnimParm.kAttack]);
    }

    /// <summary>
    /// �U���ҋ@����
    /// </summary>
    protected void AttackInterval()
    {
        // �U����~���ĂȂ��Ȃ�I��
        if (!_isStopAttack) return;

        --_watiAttackFrame;
        // �U���ҋ@���Ԃ��I��������ĂэU���ł���悤��
        if (_watiAttackFrame < 0)
        {
            _isStopAttack = false;
        }
    }

    /// <summary>
    /// _deltaHp��_hp�܂Ō��炷����
    /// </summary>
    protected void ReduceDeltaHp()
    {
        // �������łȂ��Ȃ�I��
        if (!_isDeltaHp) return;

        // ����
        _deltaHp -= kDecreaseHpSpeed;
        // ���݂�HP�����ɂȂ�����I��
        if (_deltaHp < _hp)
        {
            _deltaHp = _hp;
            _isDeltaHp = false;
        }
    }

    /// <summary>
    /// �U���_��̐ݒ�
    /// </summary>
    /// <param name="isNear">true : �߂��̔_����U���Ώۂ� / false : �����̔_����U���Ώۂ�</param>
    protected void FindFarm(bool isNear)
    {
        int childIdx = 0;

        bool isFirst = true;
        float dis = 0.0f;

        Vector3 pos = this.transform.position;

        for (int i = 0; i < FarmManager.kFarmNum; ++i)
        {
            Farm tempFarm = _farmBase.transform.GetChild(i).gameObject.GetComponent<Farm>();
            if (tempFarm.IsBreak) continue;

            if (isFirst)
            {
                Vector3 childPos = _farmBase.transform.GetChild(i).transform.position;
                dis = (pos - childPos).sqrMagnitude;
                childIdx = i;

                isFirst = false;
            }
            else
            {
                Vector3 childPos = _farmBase.transform.GetChild(i).transform.position;
                var childSqrLen = (pos - childPos).sqrMagnitude;

                bool isUpdate = false;
                if (isNear)
                {
                    isUpdate = (dis > childSqrLen);
                }
                else
                {
                    isUpdate = (dis < childSqrLen);
                }

                if (isUpdate)
                {
                    dis = childSqrLen;
                    childIdx = i;
                }
            }
        }

        _farm = _farmBase.transform.GetChild(childIdx).gameObject;
        _farmScript = _farm.GetComponent<Farm>();
    }
}
