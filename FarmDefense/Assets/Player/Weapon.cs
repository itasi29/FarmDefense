using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//����̏�������
public abstract class Weapon : MonoBehaviour
{
    public enum WeaponStatus 
    {
        kAtk,
        kSpeed,
        kRange,
        kStatusNum
    };


    //����̃X�e�[�^�X�̏����܂Ƃ߂�
    protected struct StatusInfo
    {
        public int point;//�X�e�[�^�X�̒l

        public int level;
    }

    //�X�e�[�^�X�̏���ۑ�����z��
    protected StatusInfo[] statusInfo = new StatusInfo[(int)WeaponStatus.kStatusNum];

    /// <summary>
    /// ����̃X�e�[�^�X�̃��x����ݒ肷��
    /// </summary>
    /// <param name="status">�ǂ̃X�e�[�^�X�̃��x����ύX���邩</param>
    /// <param name="level">�ύX��̃��x��</param>
    public void SetLevel(WeaponStatus status,int level)
    {
        statusInfo[(int)status].level = level;
        statusInfo[(int)status].point = level;//TODO �O���t�@�C�������Ēl��ω��ł���悤�ɂ���
    }
    /// <summary>
    /// ����̃X�e�[�^�X�̃��x�����擾����
    /// </summary>
    /// <param name="status">�擾�������X�e�[�^�X</param>
    /// <returns></returns>
    public int GetLevel(WeaponStatus status)
    {
        return statusInfo[(int)status].level;
    }
    public int GetStatusPoint(WeaponStatus status)
    {
        return statusInfo[(int)status].point;
    }
    public virtual void Update()
    {

    }



}
