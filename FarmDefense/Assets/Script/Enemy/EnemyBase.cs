using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public abstract class EnemyBase : MonoBehaviour
{
    /* �萔 */
    private const int kDecreaseHpSpeed = 1;   // _deltaHp�̌������x

    /* �ϐ� */
    [SerializeField] private EnemyStatus _status;    // �X�e�[�^�X
    private int _hp;                // ���݂�HP
    private int _deltaHp;           // �����𕪂���₷�����邽�߂̕ϐ�
    private int _watiAttackFrame;   // �U���ҋ@�t���[���p
    private bool _isExist;          // �����t���O
    private bool _isDeltaHp;        // �����t���O
    protected bool _isStopAttack;     // �U����~�t���O
    protected bool _isFindPlayer;     // �v���C���[�����t���O
    private GameObject _farmBase;   // �_��S���������Ă���e�I�u�W�F�N�g
    protected GameObject _farm;     // �U������_��
    protected Farm _farmScript;     // ��̂̃X�N���v�g
    protected GameObject _player;   // �v���C���[
    private CameraControl _camera;  // �J�������
    private SpawnerManager _spawnerMgr; // �X�|�i�[�}�l�[�W���[

    /* �v���p�e�B */
    public int Hp { get { return _hp; } }
    public int DeltaHp { get { return _deltaHp; } }
    public int MaxHp { get { return _status.maxHp; } }
    public bool IsExist { get { return  IsExist; } }

    /// <summary>
    /// ������
    /// </summary>
    /// <param name="pos">�����ʒu</param>
    /// <param name="enemyNo">�G�̔ԍ�</param>
    public virtual void Init(Vector3 pos, int enemyNo)
    {
        _farmBase = GameObject.Find("Farm").gameObject;
        _player = GameObject.Find("Player").gameObject;
        _camera = GameObject.Find("Main Camera").GetComponent<CameraControl>();
        _spawnerMgr = GameObject.Find("SpawnerManager").GetComponent<SpawnerManager>();

        // �X�e�[�^�X�擾
        EnemyData data = GameObject.Find("DataManager").GetComponent<EnemyData>();
        _status = data.GetStatus(enemyNo);

        // �e�평����
        _hp = _status.maxHp;
        _deltaHp = _status.maxHp;
        _watiAttackFrame = 0;
        _isExist = true;
        _isDeltaHp = false;
        _isStopAttack = false;
        _isFindPlayer = false;
        transform.position = pos;
    }

    /// <summary>
    /// �_���[�W����
    /// </summary>
    /// <param name="damage">�_���[�W��</param>
    public void OnDamage(int damage)
    {
        _hp -= damage;
        _isDeltaHp = true;

        // HP��0�ȉ��ɂȂ����玀�S����
        if (_hp <= 0)
        {
            _hp = 0;
            _isExist = false;
            // �X�|�i�[�}�l�[�W���[�Ɏ��S�������Ƃ�`����
            _spawnerMgr.AddKilledEnemy();
            // �J�����Ɏ��S�������Ƃ�`����
            _camera.SubHpBarInfo(this.gameObject);
            // �j��
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// �_��Ɍ������Ă̈ړ�
    /// </summary>
    protected void MoeveToFarm()
    {
        Vector3 pos = transform.position;
        Vector3 farmPos = _farm.transform.position;

        Vector3 velocity = (farmPos - pos).normalized * _status.speed;

        transform.position = pos + velocity;
    }

    /// <summary>
    /// �v���C���[�Ɍ������Ă̈ړ�
    /// </summary>
    protected void MoveToPlayer()
    {
        Vector3 pos = transform.position;
        Vector3 playerPos = _player.transform.position;

        Vector3 velocity = (playerPos - pos).normalized * _status.speed;

        transform.position = pos + velocity;
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

        _farmScript.OnDamage(_status.attack);
        _isStopAttack = true;
    }

    protected void AttackPlayer()
    {
        // �U���ҋ@��ԂȂ炵�Ȃ�
        if (_isStopAttack) return;
        // �v���C���[�񔭌����Ȃ�U�����Ȃ�
        if (!_isFindPlayer) return;

        _player.GetComponent<Player>().OnDamage(_status.attack);
        _isStopAttack = true;
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
