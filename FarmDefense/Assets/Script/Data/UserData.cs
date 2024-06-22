using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class User
{
    public int money;   // ����������
    public int time;    // �v���C����
    public Dictionary<string, int> weapon;    // ����̃��x��
    public Dictionary<string, int> item;        // �A�C�e��������
}

public class UserData
{
    // ���[�U�f�[�^�ێ���
    private const int kUserNum = 4;

    // �e���[�U�f�[�^�̏��
    private Dictionary<int, User> _data = new Dictionary<int, User>();

    /// <summary>
    /// �ǂݍ���
    /// </summary>
    public void Load(List<string> weaponIdList, List<string> itemIdList)
    {
        try
        {
            // �t�@�C�����J��
            // using(){}����Ə����Close���Ă����
            using (var reader = new BinaryReader(new FileStream(Application.dataPath + DataManager.kUserFileName, FileMode.Open)))
            {
                // �t�@�C�������݂���ꍇ
                for (int i = 0; i < kUserNum; ++i)
                {
                    User user = new User();
                    // �������ǂݍ���
                    user.money = reader.ReadInt32();
                    // �v���C���ԓǂݍ���
                    user.time = reader.ReadInt32();
                    // ����̃��x���ǂݍ���
                    Dictionary<string, int> weapon = new Dictionary<string, int>();
                    for (int j = 0; j < weaponIdList.Count; ++j)
                    {
                        weapon.Add(weaponIdList[j], reader.ReadInt32());
                    }
                    user.weapon = weapon;
                    // �e��A�C�e���������ǂݍ���
                    Dictionary<string, int> item = new Dictionary<string, int>();
                    for (int j = 0; j < itemIdList.Count; ++j)
                    {
                        item.Add(itemIdList[j], reader.ReadInt32());
                    }
                    user.item = item;

                    // �f�[�^�}��
                    _data.Add(i, user);
                }
            }
        }
        catch
        {
            // �t�@�C�������݂��Ȃ��ꍇ
            for (int i = 0; i < kUserNum; ++i)
            {
                User user = new User();
                // ������
                user.money = 0;
                // �v���C����
                user.time = 0;
                // ����̃��x��
                Dictionary<string, int> weapon = new Dictionary<string, int>();
                for (int j = 0; j < weaponIdList.Count; ++j)
                {
                    weapon.Add(weaponIdList[j], 1);
                }
                user.weapon = weapon;
                // �A�C�e��������
                Dictionary<string, int> item = new Dictionary<string, int>();
                for (int j = 0; j < itemIdList.Count; ++j)
                {
                    item.Add(weaponIdList[j], 0);
                }
                user.item = item;

                // �f�[�^�}��
                _data.Add(i, user);
            }
        }

#if true
        // �f�[�^�̒��g����p
        int x = 0;
        foreach (var user in _data)
        {
            ++x;
            Debug.Log(x + "��ځF moeny = " + user.Value.money + ", time = " + user.Value.time);
            foreach (var weapon in user.Value.weapon)
            {
                Debug.Log(weapon.Key + ": " + weapon.Value);
            }
            foreach (var item in user.Value.item)
            {
                Debug.Log(x + "��ځF " + item.Key + " = " + item.Value);
            }
        }
#endif
    }

    /// <summary>
    /// �ۑ�
    /// </summary>
    public void Save()
    {
        try
        {
            // �t�@�C�����J��(�Ȃ��ꍇ�͍쐬)
            using (var writer = new BinaryWriter(new FileStream(Application.dataPath + DataManager.kUserFileName, FileMode.OpenOrCreate)))
            {
                foreach (var user in _data)
                {
                    // ��������������
                    writer.Write((Int32)user.Value.money);
                    // �v���C���ԏ�������
                    writer.Write((Int32)user.Value.time);
                    // ����̃��x����������
                    foreach (var weapon in user.Value.weapon)
                    {
                        writer.Write((Int32)weapon.Value);
                    }
                    // �A�C�e����������������
                    foreach (var item in user.Value.item)
                    {
                        writer.Write((Int32)item.Value);
                    }
                }
            }
        }
        catch
        {
            Debug.Log("���ł�");
        }
    }

    // MEMO: �v���p�e�B�����������̂܂܂��������c
    /// <summary>
    /// ���ݏ������Ă��邨��
    /// </summary>
    /// <param name="userNo">���[�U�ԍ�</param>
    /// <returns>������</returns>
    public int GetMoney(int userNo)
    {
        return _data[userNo].money;
    }
    /// <summary>
    /// ���݂̃v���C����
    /// </summary>
    /// <param name="userNo">���[�U�ԍ�</param>
    /// <returns>�v���C����</returns>
    public int GetTime(int userNo) 
    {
        return _data[userNo].time;
    }
    /// <summary>
    /// ���݂̕���̃��x��
    /// </summary>
    /// <param name="userNo">���[�U�ԍ�</param>
    /// <param name="id">�����ID</param>
    /// <returns>���x��</returns>
    public int GetWeaponLv(int userNo, string id)
    {
        return _data[userNo].weapon[id];
    }
    /// <summary>
    /// ���ݏ������Ă���A�C�e���̐�
    /// </summary>
    /// <param name="userNo">���[�U�ԍ�</param>
    /// <param name="id">�A�C�e����ID</param>
    /// <returns>������</returns>
    public int GetHasItemNum(int userNo, string id)
    {
        return _data[userNo].item[id];
    }
    /// <summary>
    /// �������𑝂₷
    /// </summary>
    /// <param name="userNo">���[�U�ԍ�</param>
    /// <param name="add">���₷�����̗�</param>
    public void AddMoney(int userNo, int add)
    {
        _data[userNo].money += add;
    }
    /// <summary>
    /// ���炷���z��0�����ɂȂ�Ȃ��ꍇ�͏����������炷
    /// </summary>
    /// <param name="userNo">���[�U�ԍ�</param>
    /// <param name="sub">���炷�����̗�</param>
    /// <returns>true: ���点�� / false: ���点�Ȃ�</returns>
    public bool SubMoney(int userNo, int sub)
    {
        int temp = _data[userNo].money;
        temp -= sub;
        if (temp < 0)
        {
            return false;
        }
        else
        {
            _data[userNo].money = temp;
            return true;
        }
    }
    /// <summary>
    /// �v���C���Ԃ̑���
    /// </summary>
    /// <param name="userNo">���[�U�ԍ�</param>
    public void AddTime(int userNo)
    {
        ++_data[userNo].time;
    }
    /// <summary>
    /// ����̃��x�����グ��
    /// </summary>
    /// <param name="userNo">���[�U�ԍ�</param>
    /// <param name="id">�����ID</param>
    public void LvUpWeapon(int userNo, string id)
    {
        ++_data[userNo].weapon[id];
    }
    /// <summary>
    /// �����A�C�e�����𑝂₷
    /// </summary>
    /// <param name="userNo">���[�U�ԍ�</param>
    /// <param name="id">�A�C�e����ID</param>
    public void AddHasItemNum(int userNo, string id)
    {
        ++_data[userNo].item[id];
    }
    /// <summary>
    /// �������Ă���ꍇ�A�C�e�����g�p����
    /// </summary>
    /// <param name="userNo">���[�U�ԍ�</param>
    /// <param name="id">�A�C�e����ID</param>
    /// <returns>true: �g�p�\ / false: �g�p�s�\</returns>
    public bool UseItem(int userNo, string id)
    {
        int temp = _data[userNo].item[id];
        --temp;
        if (temp < 0)
        {
            return false;
        }
        else
        {
            _data[userNo].item[id] = temp;
            return true;
        }
    }
}
