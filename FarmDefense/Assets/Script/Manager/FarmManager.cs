using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FarmManager : MonoBehaviour
{
    /* �萔 */
    public const int kFarmNum = 6; // �_��̐�
    private string kResultSceneName = "ResultScene";

    /* �ϐ� */
    private GameObject[] _farmList = new GameObject[kFarmNum];   // �_��̃I�u�W�F�N�g���
    private Farm[] _farmScript = new Farm[kFarmNum];             // �_��̃X�N���v�g���
    private int _totalHp;
    private int _totalMaxHp;

    private Fade _fade;
    private StageRusultData _resultData;

    public int Hp { get { return _totalHp; } }
    public int MaxHp {  get { return _totalMaxHp; } }

    private void Start()
    {
        _totalHp = 0;
        _fade = GetComponent<Fade>();

        _resultData = GameObject.Find("GameDirector").GetComponent<GameDirector>().ResultData;

        GameObject parent = GameObject.Find("Farm");
        for (int i = 0; i < kFarmNum; ++i)
        {
            _farmList[i] = parent.transform.GetChild(i).gameObject;
            _farmScript[i] = _farmList[i].GetComponent<Farm>();
            _totalHp += _farmScript[i].Hp;
        }

        _totalMaxHp = _totalHp;
    }

    private void FixedUpdate()
    {
        // ��HP�̍X�V
        _totalHp = 0;
        foreach (var farm in _farmScript)
        {
            _totalHp += farm.Hp;
            _resultData.TotalFarmHp = _totalHp;
        }

        // ��HP��0�ɂȂ�����I��
        if (_totalHp <= 0)
        {
            _resultData.IsCrear = false;
            _resultData.TotalFarmHp = 0;
            _fade.StartFadeOut(kResultSceneName);
            return;
        }
    }

    /// <summary>
    /// �n���ꂽ�ꏊ����߂��_��̍��W��Ԃ�
    /// </summary>
    /// <param name="pos">���g�̏ꏊ</param>
    /// <returns>�߂��_��̍��W</returns>
    public Vector3 GetNearPos(Vector3 pos)
    {
        Vector3 nearPos = pos;
        bool isNeverIn = true;

        for (int i = 0; i < kFarmNum; ++i)
        {
            // �j�󂳂�Ă����玟��
            if (_farmScript[i].IsBreak) continue;

            // ��x��������Ă��Ȃ���΂��̂܂ܑ��
            if (isNeverIn)
            {
                nearPos = _farmList[i].transform.position;
                isNeverIn = false;
            }
            // �����ꂽ���Ƃ�����Όv�Z
            else
            {
                // ���ݑI��ł���_��܂ł̋���
                float nowSqLength = (nearPos - pos).sqrMagnitude;
                // �m�F����_��܂ł̋���
                float checkSqLength = (_farmList[i].transform.position - pos).sqrMagnitude;
                // ���ݕۑ�����Ă���_��܂ł̋������߂���ΕύX
                if (nowSqLength > checkSqLength)
                {
                    nearPos = _farmList[i].transform.position;
                }
            }
        }

        return nearPos;
    }

    /// <summary>
    /// �n���ꂽ�ꏊ���牓���_��̍��W��Ԃ�
    /// </summary>
    /// <param name="pos">���g�̏ꏊ</param>
    /// <returns>�_��̍��W</returns>
    public Vector3 GetFarPos(Vector3 pos)
    {
        Vector3 farPos = pos;
        bool isNeverIn = true;

        for (int i = 1; i < kFarmNum; ++i)
        {
            // �j�󂳂�Ă����玟��
            if (_farmScript[i].IsBreak) continue;

            // ��x��������Ă��Ȃ���΂��̂܂ܑ��
            if (isNeverIn)
            {
                farPos = _farmList[i].transform.position;
                isNeverIn = false;
            }
            // �����ꂽ���Ƃ�����Όv�Z
            else
            {
                // ���ݑI��ł���_��܂ł̋���
                float nowSqLength = (farPos - pos).sqrMagnitude;
                // �m�F����_��܂ł̋���
                float checkSqLength = (_farmList[i].transform.position - pos).sqrMagnitude;
                // ���ݕۑ�����Ă���_��܂ł̋�����艓����ΕύX
                if (nowSqLength < checkSqLength)
                {
                    farPos = _farmList[i].transform.position;
                }
            }
        }

        return farPos;
    }
}
