using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class User
{
    public int money;   // ����������
    public int time;    // �v���C����
    public Dictionary<string, int> weapon;    // ����̃��x��
    public Dictionary<string, List<int>> item;        // �A�C�e��������
    public Dictionary<int, bool> clearInfo; // �N���A���
}

public class UserData
{
    // ���[�U�f�[�^�ێ���
    private const int kUserNum = 4;

    // �e���[�U�f�[�^�̏��
    private Dictionary<int, User> _data = new Dictionary<int, User>();
    // ���ʃ��[�U�f�[�^
    private int _bgmVolLv;
    private int _seVolLv;
    // ���݂̃��[�U�ԍ�
    private int _nowUserNo;

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
                _bgmVolLv = reader.ReadInt32();
                _seVolLv = reader.ReadInt32();

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
                    Dictionary<string, List<int>> item = new Dictionary<string, List<int>>();
                    for (int j = 0; j < itemIdList.Count; ++j)
                    {
                        List<int> lv = new List<int>();
                        for (int k  = 0; k < 3; ++k)
                        {
                            lv.Add(reader.ReadInt32());
                        }
                        item.Add(itemIdList[j], lv);
                    }
                    user.item = item;
                    // �N���A�f�[�^�ǂݍ���
                    Dictionary<int, bool> clearInfo = new Dictionary<int, bool>();
                    for (int j = 0; j < 6; ++j)
                    {
                        clearInfo.Add(j, reader.ReadBoolean());
                    }
                    user.clearInfo = clearInfo;

                    // �f�[�^�}��
                    _data.Add(i, user);
                }
            }
        }
        catch
        {
            // �t�@�C�������݂��Ȃ��ꍇ
            _bgmVolLv = SoundManager.kVolumeLvMax;
            _seVolLv = SoundManager.kVolumeLvMax;

            for (int i = 0; i < kUserNum; ++i)
            {
                User user = new User();
                // ������
                user.money = 100000;
                // �v���C����
                user.time = 0;
                // ����̃��x��
                Dictionary<string, int> weapon = new Dictionary<string, int>();
                for (int j = 0; j < weaponIdList.Count; ++j)
                {
                    weapon.Add(weaponIdList[j], 0);
                }
                user.weapon = weapon;
                // �A�C�e��������
                Dictionary<string, List<int>> item = new Dictionary<string, List<int>>();
                for (int j = 0; j < itemIdList.Count; ++j)
                {
                    List<int> lv = new List<int>();
                    for (int k = 0; k < 3; ++k)
                    {
                        lv.Add(0);
                    }
                    item.Add(itemIdList[j], lv);
                }
                user.item = item;
                // �N���A�f�[�^�ǂݍ���
                Dictionary<int, bool> clearInfo = new Dictionary<int, bool>();
                for (int j = 0; j < 6; ++j)
                {
                    if (j == 0)
                    {
                        clearInfo.Add(j, true);
                    }
                    else
                    {
                        clearInfo.Add(j, false);
                    }
                }
                user.clearInfo = clearInfo;

                // �f�[�^�}��
                _data.Add(i, user);
            }
        }

#if false
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
                writer.Write((Int32)_bgmVolLv);
                writer.Write((Int32)_seVolLv);

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
                        foreach (var lv in item.Value)
                        {
                            writer.Write((Int32)lv);
                        }
                    }
                    // �N���A�f�[�^��������
                    foreach (var clear in user.Value.clearInfo)
                    {
                        writer.Write((Boolean)clear.Value);
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
    public int GetBgmVolLv()
    {
        return _bgmVolLv;
    }
    public int GetSeVolLv() 
    {
        return _seVolLv;
    }
    public void SetUserNo(int userNo)
    {
        _nowUserNo = userNo;
    }
    public bool IsStageClear(int no)
    {
        return _data[_nowUserNo].clearInfo[no];
    }
    public void ChangeStageClear(int no)
    {
        _data[_nowUserNo].clearInfo[no] = true;
    }
    /// <summary>
    /// ���ݏ������Ă��邨��
    /// </summary>
    /// <param name="userNo">���[�U�ԍ� def:_nowUserNo</param>
    /// <returns>������</returns>
    public int GetMoney(int userNo = -1)
    {
        if (userNo < 0)
        {
            userNo = _nowUserNo;
        }
        return _data[userNo].money;
    }
    /// <summary>
    /// ���݂̃v���C����
    /// </summary>
    /// <param name="userNo">���[�U�ԍ� def:_nowUserNo</param>
    /// <returns>�v���C����</returns>
    public int GetTime(int userNo = -1) 
    {
        if (userNo < 0)
        {
            userNo = _nowUserNo;
        }
        return _data[userNo].time;
    }
    /// <summary>
    /// ���݂̕���̃��x��
    /// </summary>
    /// <param name="id">�����ID</param>
    /// <returns>���x��</returns>
    public int GetWeaponLv(string id)
    {
        return _data[_nowUserNo].weapon[id];
    }
    /// <summary>
    /// ���ݏ������Ă���A�C�e���̐�
    /// </summary>
    /// <param name="id">�A�C�e����ID</param>
    /// <param name="lv">�A�C�e���̃��x��</param>
    /// <returns>������</returns>
    public int GetHasItemNum(string id, int lv)
    {
        return _data[_nowUserNo].item[id][lv];
    }
    public void SetBgmVolLv(int lv)
    {
        _bgmVolLv = lv;
    }
    public void SetSeVolLv(int lv)
    {
        _seVolLv = lv;
    }
    /// <summary>
    /// �������𑝂₷
    /// </summary>
    /// <param name="add">���₷�����̗�</param>
    public void AddMoney(int add)
    {
        _data[_nowUserNo].money += add;
    }
    /// <summary>
    /// ���炷���z��0�����ɂȂ�Ȃ��ꍇ�͏����������炷
    /// </summary>
    /// <param name="sub">���炷�����̗�</param>
    /// <returns>true: ���点�� / false: ���点�Ȃ�</returns>
    public bool SubMoney(int sub)
    {
        int temp = _data[_nowUserNo].money;
        temp -= sub;
        if (temp < 0)
        {
            return false;
        }
        else
        {
            _data[_nowUserNo].money = temp;
            return true;
        }
    }
    /// <summary>
    /// �v���C���Ԃ̑���
    /// </summary>
    public void AddTime()
    {
        ++_data[_nowUserNo].time;
    }
    /// <summary>
    /// ����̃��x�����グ��
    /// </summary>
    /// <param name="id">�����ID</param>
    public void LvUpWeapon(string id)
    {
        ++_data[_nowUserNo].weapon[id];
    }
    /// <summary>
    /// �����A�C�e�����𑝂₷
    /// </summary>
    /// <param name="id">�A�C�e����ID</param>
    /// <param name="lv">�A�C�e���̃��x��</param>
    public void AddHasItemNum(string id, int lv)
    {
        ++_data[_nowUserNo].item[id][lv];
    }
    /// <summary>
    /// �������Ă���ꍇ�A�C�e�����g�p����
    /// </summary>
    /// <param name="id">�A�C�e����ID</param>
    /// <param name="lv">�A�C�e���̃��x��</param>
    /// <returns>true: �g�p�\ / false: �g�p�s�\</returns>
    public bool UseItem(string id, int lv)
    {
        int temp = _data[_nowUserNo].item[id][lv];
        --temp;
        if (temp < 0)
        {
            return false;
        }
        else
        {
            _data[_nowUserNo].item[id][lv] = temp;
            return true;
        }
    }
}
