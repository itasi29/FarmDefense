using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

/// <summary>
/// �G�l�~�[�̐e
/// </summary>
public class EnemyBase : MonoBehaviour
{
    /* �c�オ��������� */
    // �萔�̒ǉ�
    // �ϐ��̒ǉ�
    // �v���p�e�B�̒ǉ�
    // �֐���̒ǉ�
    // �R�[�f�B���O�K��ɑ����ĕϐ����̕ύX
    // �A�N�Z�X�w��q�̕ύX
    /* �����܂� */

    [SerializeField] private int _hp;  //�G��HP

    [SerializeField] protected float _speed;  //�G�̃X�s�[�h

    [SerializeField] private int _attack;  //�G�̍U����

    [SerializeField] private float _attackTime;  //�G�̍U�����ԊԊu

    [SerializeField] private bool _attackinterval;  //�U���������̃t���O

    [SerializeField] protected bool _isFindPlayer;  //�v���C���[�𔭌��������ǂ����̃t���O

    [SerializeField] protected GameObject target; //�^�[�Q�b�g�̃I�u�W�F�N�g�l��

    [SerializeField] protected GameObject player; //Player�̃I�u�W�F�N�g�l��

    public Farm farm;  //�_��̃X�N���v�g�Ăяo��

    /* �c�オ�ǉ������Ƃ��� */
    // �萔�̒ǉ�
    private const int kDecreaseSpeed = 1;   // _deltaHp�̌��炷�X�s�[�h

    // �ϐ��̒ǉ�
    private int _maxHp;     // �ő�HP(�e�L�����ň���Ă��邽�ߕϐ��Ƃ��Ď����Ă���)
    private int _deltaHp;   // HP�̐ԕ\�L����
    private bool _isDelat;  // DeltaHP���������Ă��邩
    private bool _isExist;  // �������Ă��邩

    // �v���p�e�B�̍쐬
    public int Hp { get { return _hp; } }
    public int DeltaHp { get { return _deltaHp; } }
    public int MaxHp { get { return _maxHp; } }
    public bool IsExist { get { return _isExist; } }
    /* �����܂� */

    /// <summary>
    /// �X�V����
    /// </summary>
    public virtual void Update()
    {
    }

    /// <summary>
    /// ����������
    /// </summary>
    public virtual void Init(Vector3 pos)
    {
        transform.position = pos;  //Enemy�̏����ʒu������

        _hp = 0;
        _speed = 0;
        _attack = 0;
        _attackTime = 0;

        /* �c�オ�ǉ����Ƃ��� */
        // TODO: �ő�g�o���擾����֐��͌�X���̂ł�����g���Ă��
        _deltaHp = _hp;
        _isDelat = false;
        _isExist = true;
        /* �����܂� */

        _attackinterval = false;
        _isFindPlayer = false;
    }

    /// <summary>
    /// ���������̍X�V����
    /// </summary>
    public virtual void FixedUpdate()
    {
        Transform transform = this.transform; //�I�u�W�F�N�g���擾

        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, _speed * Time.deltaTime);  //�^�[�Q�b�g�̃I�u�W�F�N�g�Ɍ�����

        ReduceDeltaHp();
    }

    /// <summary>
    /// �_��Ƃ̓����蔻��
    /// </summary>
    public virtual void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.name == "Farm") //Farm�ɓ���������U��
        {
            if(_attackinterval == false)    //�t���O��false�Ȃ�U���J�n
            {
                farm.OnDamage(_attack);  //Farm��Hp�����炷

                _attackinterval = true;  //1�񂾂��U���\
            }
            else if(_attackinterval == true)  //�t���O��true�Ȃ�U�����~
            {
                _attackTime++;  //���Ԍo��

                if(_attackTime >= 60.0f)  //���Ԃ����Ă�
                {
                    _attackTime = 0;   //0�b�ɖ߂�

                    _attackinterval = false; //�t���O��false�ɖ߂�
                }
            }

        }
        else if(collision.gameObject.name == "Player")  //Plyaer�ɓ���������U��
        {
            if (_attackinterval == false)    //�t���O��false�Ȃ�U���J�n
            {
                //farm.m_farmHp -= m_enemyAttack; //�������Ă�Ƃ�HP�����炷

                //Debug.Log(farm.m_farmHp -= (int)m_enemyAttack);

                _attackinterval = true;  //1�񂾂��U���\
            }
            else if (_attackinterval == true)  //�t���O��true�Ȃ�U�����~
            {
                _attackTime++;  //���Ԍo��

                if (_attackTime >= 60.0f)  //���Ԃ����Ă�
                {
                    _attackTime = 0;   //0�b�ɖ߂�

                    _attackinterval = false; //�t���O��false�ɖ߂�
                }
            }
        }
    }

    /// <summary>
    /// �v���C���[���U��������
    /// </summary>
    /// <param name="collision"></param>
    //public virtual void OnCollisionEnter(Collider collision)
    //{
    //    //�v���C���[�ɍU�����ꂽ��_���[�W���󂯂�
    //}

    /// <summary>
    /// �v���C���[�����G�͈͂ɓ��������ǂ���
    /// </summary>
    /// <param name="collision"></param>
    public virtual void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.name == "Player")  //Player�����G�͈͂ɓ�������Player��ǂ�������
        {
            _isFindPlayer = true;  //m_player��true�ɂ���

            Debug.Log("������");
        }
    }

    /// <summary>
    /// �v���C���[�����G�͈͂��ł����ǂ���
    /// </summary>
    /// <param name="collision"></param>
    public virtual void OnTriggerExit(Collider collision)
    {
        if(collision.gameObject.name == "Player")
        {
            _isFindPlayer = false; //m_player��false�ɂ���

            Debug.Log("������");

        }
    }



    /* �c�オ�ǉ������Ƃ��� */
    /// <summary>
    /// �_���[�W����
    /// </summary>
    /// <param name="damage">�_���[�W��</param>
    public void OnDamage(int damage)
    {
        _hp -= damage;
        _isDelat = true;

        // HP�������Ȃ����玀�S�Ƃ���
        if (_hp <= 0)
        {
            _hp = 0;
            _isExist = false;
        }
    }

    /// <summary>
    /// _deltaHp��_hp�܂Ō��炷����
    /// </summary>
    private void ReduceDeltaHp()
    {
        // �������łȂ��Ȃ�I��
        if (!_isDelat) return;

        // ����
        _deltaHp -= kDecreaseSpeed;
        // ���݂�HP�����ɂȂ�����I��
        if (_deltaHp < _hp)
        {
            _deltaHp = _hp;
            _isDelat = false;
        }
    }
    /* �����܂� */
}
