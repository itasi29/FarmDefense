using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public struct User
{
    public int money;   // ����������
    public int time;    // �v���C����
    public Dictionary<string, int> weaponLv;    // ����̃��x��
    public Dictionary<string, int> item;        // �A�C�e��������
}

public class UserData
{
    // �t�@�C���̃p�X
    private const string kPath = "Bin/UserData.bin";
    // ���[�U�f�[�^�ێ���
    private const int kUserNum = 4;

    // �e���[�U�f�[�^�̏��
    private Dictionary<int, User> _data;

    /// <summary>
    /// �ǂݍ���
    /// </summary>
    public void Load(List<string> weaponIdList, List<string> itemIdList)
    {
        // kPath�̃t�@�C����ǂݍ��݃��[�h�ŊJ��(���݂��Ȃ��ꍇ�͐V�����쐬����)
        try
        {

        }
        catch 
        { 

        }
    }

    /// <summary>
    /// ��������
    /// </summary>
    public void Write()
    {

    }
}
