using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    /* �萔 */
    private const float kDistance = 2.2f;   // �^�[�Q�b�g�ƃJ�����Ƃ̋���
    private const float kShiftPosY   = 1.2f;   // �^�[�Q�b�g���S�����ɂ��炷��
    private const float kAxisMinThershold = 0.2f; // ���͏��̍ŏ��̂������l:�������銄��
    private const float kAxisMaxThershold = 0.8f; // ���͏��̍ő�̂������l:1.0�Ƃ݂Ȃ�����
    private const float kSpeedRotation = 0.25f * Mathf.Deg2Rad;  // ��]�X�s�[�h(���W�A��)

    /* �ϐ� */
    private GameObject _target;     // �^�[�Q�b�g�̃I�u�W�F�N�g���
    private Transform _targetTrs;   // �^�[�Q�b�g��Transform���
    private Vector3 _pos;           // �J�����̒��S���W
    private float _rot;

    void Start()
    {
        // �^�[�Q�b�g(�v���C���[)������擾
        _target = GameObject.Find("Player");
        _targetTrs = _target.transform;

        /* �����ݒ� */
        _pos = _targetTrs.position;   // ���W
        _rot = 0.0f;
    }

    private void Update()
    {
        /* ��] */
        // ���͒l�̎擾 
        float inputRate = Input.GetAxis("HorizontalRight");  

        // ���͒l�̐���
        if (inputRate > 0.0f)
        {
            // ���̏ꍇ
            inputRate = (inputRate - kAxisMinThershold) / (kAxisMaxThershold - kAxisMinThershold);
            inputRate = Mathf.Min(1.0f, Mathf.Max(0.0f, inputRate));
        }
        else
        {
            // ���̏ꍇ
            inputRate = (inputRate + kAxisMinThershold) / (kAxisMaxThershold - kAxisMinThershold);
            inputRate = Mathf.Min(0.0f, Mathf.Max(-1.0f, inputRate));
        }

        // ���
        _rot += inputRate * kSpeedRotation;
    }

    private void FixedUpdate()
    {
        /* �ʒu�X�V */
        _pos = Vector3.Lerp(_pos, _targetTrs.position, 0.25f);

        /* �����̔��f */
        Vector3 pos = _pos;
        pos.x += Mathf.Sin(_rot) * kDistance;
        pos.y += kShiftPosY;
        pos.z += Mathf.Cos(_rot) * kDistance * -1.0f;

        /* �ʒu�̑�� */
        transform.position = pos;
    }
}
