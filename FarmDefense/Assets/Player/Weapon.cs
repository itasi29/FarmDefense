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
        kRate,
        kSpeed,
        kRange = kSpeed,
        kStatusNum
    };


    //����̃X�e�[�^�X�̏����܂Ƃ߂�

    public int level;


    //���x���̏���ۑ�����z��
    protected int[] weaponLevel = new int[(int)WeaponStatus.kStatusNum];

    /// <summary>
    /// ����̃X�e�[�^�X�̃��x����ݒ肷��
    /// </summary>
    /// <param name="status">�ǂ̃X�e�[�^�X�̃��x����ύX���邩</param>
    /// <param name="level">�ύX��̃��x��</param>
    public void SetLevel(WeaponStatus status, int level)
    {
        weaponLevel[(int)status] = level;
    }
    /// <summary>
    /// ����̃X�e�[�^�X�̃��x�����擾����
    /// </summary>
    /// <param name="status">�擾�������X�e�[�^�X</param>
    /// <returns></returns>
    public int GetLevel(WeaponStatus status)
    {
        return weaponLevel[(int)status];
    }

    public virtual void Update()
    {

    }



}
