using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Timeline;
using static UnityEditor.PlayerSettings;

public class CameraControl : MonoBehaviour
{
    /* �萔 */
    private const float kDistance = 2.2f;   // �^�[�Q�b�g�ƃJ�����Ƃ̋���
    private const float kShiftPosY   = 1.2f;   // �^�[�Q�b�g���S�����ɂ��炷��
    private const float kAxisMinThershold = 0.2f; // ���͏��̍ŏ��̂������l:�������銄��
    private const float kAxisMaxThershold = 0.8f; // ���͏��̍ő�̂������l:1.0�Ƃ݂Ȃ�����
    private const float kRotLimitUpdownSwing = 30.0f * Mathf.Deg2Rad;   // �㉺�̉�]����
    private const float kRotSpeedLeftright = 0.4f  * Mathf.Deg2Rad;  // ���E�̉�]�X�s�[�h(���W�A��)
    private const float kRotSpeedUpdown    = 0.25f * Mathf.Deg2Rad;  // �㉺�̉�]�X�s�[�h(���W�A��)

    /* �ϐ� */
    private GameObject _target;         // �^�[�Q�b�g�̃I�u�W�F�N�g���
    private Transform _targetTrs;       // �^�[�Q�b�g��Transform���
    private Vector3 _centerPos;         // ���S���W
    private Vector2 _frontDir;          // �J�����̐��ʕ���(Y���͖���)
    private float _rotLeftrightSwing;   // ���E�̃J�����̉�]��
    private float _rotUpdownSwing;      // �㉺�̃J�����̉�]��
    private bool _isUpdownSwing;        // �㉺�ɃJ������h�炵�Ă��邩
    private bool _isLeftrightSwing;     // ���E�ɓ��͂�����
    private bool _isUpdownInput;        // �㉺�ɓ��͂�����
    private bool _isReset;              // ���Z�b�g������

    private void Start()
    {
        // �^�[�Q�b�g(�v���C���[)������擾
        _target = GameObject.Find("Player");
        _targetTrs = _target.transform;

        /* �����ݒ� */
        // ���S���W
        _centerPos = _targetTrs.position;
        // ���ʕ���
        _frontDir.y = 1;
        _frontDir.Normalize();
        // ��]�ʖ�����
        _rotLeftrightSwing = 0.0f;
        _rotUpdownSwing = 0.0f;
        // ��]���Ă��Ȃ���
        _isUpdownSwing = false;
        _isLeftrightSwing = false;
        // ���͂��Ă��Ȃ���
        _isUpdownInput = false;
        // ���Z�b�g���Ă��Ȃ���
        _isReset = false;
    }

    private void Update()
    {
        RotLeftright();
        RotUpdown();

        ResetDirection();
    }

    private void FixedUpdate()
    {
        ReturnRotUpdown();

        Move();

        Debug.Log("Camera : Dir = " + _frontDir);
    }

    /// <summary>
    /// �J�����̐��ʕ������擾(Y���͖���)
    /// </summary>
    /// <returns>���ʕ���</returns>
    public Vector2 GetFrontDir()
    {
        return _frontDir;
    }

    /// <summary>
    /// ���E�̉�]
    /// </summary>
    private void RotLeftright()
    {
        // ���͒l�̎擾 
        float inputRate = Input.GetAxis("HorizontalRight");

        // ���͂���Ă��Ȃ��Ȃ�I��
        if (-kAxisMinThershold < inputRate && inputRate < kAxisMinThershold)
        {
            _isLeftrightSwing = false;
            return;
        }

        inputRate = LimitValue(inputRate, kAxisMinThershold, kAxisMaxThershold);

        // ���
        _rotLeftrightSwing += inputRate * kRotSpeedLeftright;
        // ���͂��Ă��邱�Ƃ�
        _isLeftrightSwing = true;
    }

    /// <summary>
    /// �㉺�̉�]
    /// </summary>
    private void RotUpdown()
    {
        // ���͒l�̎擾
        float inputRate = Input.GetAxis("VerticalRight");

        // ���͂���Ă��Ȃ��Ȃ�I��
        if (-kAxisMinThershold < inputRate && inputRate < kAxisMinThershold)
        {
            _isUpdownInput = false;
            return;
        }

        inputRate = LimitValue(inputRate, kAxisMinThershold, kAxisMaxThershold);

        // �㉺�̉�]�n�ɑ��
        _rotUpdownSwing += inputRate * kRotSpeedUpdown;
        // ��]�̐���
        _rotUpdownSwing = Mathf.Max(Mathf.Min(_rotUpdownSwing, kRotLimitUpdownSwing), -kRotLimitUpdownSwing);
        // �������Ă��邱�Ƃ�
        _isUpdownSwing = true;
        // ���͂������Ƃ�
        _isUpdownInput = true;
    }

    /// <summary>
    /// �����Ă���������v���C���[�̌����Ă�������ɖ߂�
    /// </summary>
    private void ResetDirection()
    {
        // MEMO:�{�^���ԈႦ�Ă���\������
        // Y�{�^���������ƕ������Z�b�g
        if (Input.GetButtonDown("StickPushRight"))
        {
            // TODO:����Leftright�̉�]��0�ɂ��Ă��邾���Ȃ̂Ńv���C���[�̌����Ă�������������悤��
            _rotLeftrightSwing = 0.0f;
            _rotUpdownSwing = 0.0f;
            _isLeftrightSwing = false;
            _isUpdownSwing = false;

            _isReset = true;
        }
    }

    /// <summary>
    /// �㉺�����̉�]�ʂ����ɖ߂�
    /// </summary>
    private void ReturnRotUpdown()
    {
        // �㉺�����̓��͂����Ă���Ζ߂��Ȃ�
        if (_isUpdownInput) return;

        // 0�ɋ߂Â���
        _rotUpdownSwing = Mathf.Lerp(_rotUpdownSwing, 0.0f, 0.05f);

        // ����Ȃ�0�ɋ߂Â�����߂������Ƃɂ���
        if (-0.001f < _rotUpdownSwing && _rotUpdownSwing < 0.001f)
        {
            _rotUpdownSwing = 0.0f;
            _isUpdownSwing = false;
        }
    }

    /// <summary>
    /// �ʒu�̍X�V
    /// </summary>
    private void Move()
    {
        // ���S���X�V���邩
        bool isUpdateCenter = (_centerPos != _targetTrs.position);
        // ���Z�b�g���ĂȂ����ǂ����X�V���Ȃ���ΕύX�����Ȃ�
        if (!_isReset && !_isLeftrightSwing && !_isUpdownSwing && !isUpdateCenter) return;

        // ���S�ʒu�̍X�V
        if (isUpdateCenter)
        {
            _centerPos = Vector3.Lerp(_centerPos, _targetTrs.position, 0.5f);

            // ����Ȃ�0�ɋ߂Â���������d�Ȃ��Ă��邱�Ƃ�
            if ((_centerPos - _targetTrs.position).sqrMagnitude < 0.001f)
            {
                _centerPos = _targetTrs.position;
            }
        }

        Vector3 pos = _centerPos;

        // ���E�̉�]��
        float sinLeftright = Mathf.Sin(_rotLeftrightSwing);
        float cosLeftright = Mathf.Cos(_rotLeftrightSwing);

        // �㉺��]�̂�or���E+�㉺��]
        if (_isUpdownSwing)
        {
            // �㉺�̉�]��
            float sinUpdown = Mathf.Sin(_rotUpdownSwing);
            float cosUpdown = Mathf.Cos(_rotUpdownSwing);

            // ��]�����ʒu�̓K�p
            pos.x += sinLeftright * (kDistance * cosUpdown);
            pos.y += kShiftPosY + kDistance * sinUpdown * -1.0f;
            pos.z += cosLeftright * (kDistance * cosUpdown) * -1.0f;
        }
        // ���E��]�̂�
        else
        {
            // ��]�����ʒu�̓K�p
            pos.x += sinLeftright * kDistance;
            pos.y += kShiftPosY;
            pos.z += cosLeftright * kDistance * -1.0f;
        }

        // �ʒu�̑�� 
        transform.position = pos;

        // ���Z�b�g�܂��͉�]���Ă���Ε����̍X�V
        if (_isReset || _isLeftrightSwing || _isUpdownSwing)
        {
            // ���ʕ����ύX
            _frontDir.x = -pos.x;
            _frontDir.y = -pos.z;
            // ���K��
            _frontDir.Normalize();

            // �I�u�W�F�N�g�̌����ύX
            Vector3 lookPos = _centerPos;
            lookPos.y += kShiftPosY;
            transform.LookAt(lookPos);
        }

        // ���Z�b�g���Ă��Ȃ���
        _isReset = false;
    }


    /// <summary>
    /// �l�𐧌�����֐�
    /// </summary>
    /// <param name="val">�����������l</param>
    /// <param name="min">�ŏ��l</param>
    /// <param name="max">�ő�l</param>
    /// <returns>���������l</returns>
    private float LimitValue(float val, float min, float max)
    {
        if (val > 0.0f)
        {
            // ���̏ꍇ
            val = (val - min) / (max - min);
            val = Mathf.Min(1.0f, Mathf.Max(0.0f, val));
        }
        else
        {
            // ���̏ꍇ
            val = (val + min) / (max - min);
            val = Mathf.Min(0.0f, Mathf.Max(-1.0f, val));
        }

        return val;
    }
}
