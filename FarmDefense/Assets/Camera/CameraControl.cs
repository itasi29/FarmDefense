using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraControl : MonoBehaviour
{
    /* �萔 */
    private const float kDistance = 2.2f;   // �^�[�Q�b�g�ƃJ�����Ƃ̋���
    private const float kShiftPosY = 1.2f;   // �^�[�Q�b�g���S�����ɂ��炷��
    private const float kAxisMinThershold = 0.2f; // ���͏��̍ŏ��̂������l:�������銄��
    private const float kAxisMaxThershold = 0.8f; // ���͏��̍ő�̂������l:1.0�Ƃ݂Ȃ�����
    private const float kRotLimitUpdownSwing = 30.0f * Mathf.Deg2Rad;   // �㉺�̉�]����
    private const float kRotSpeedLeftright = 0.4f * Mathf.Deg2Rad;  // ���E�̉�]�X�s�[�h(���W�A��)
    private const float kRotSpeedUpdown = 0.25f * Mathf.Deg2Rad;  // �㉺�̉�]�X�s�[�h(���W�A��)
    // FIXME: ���O���������ɕύX
    private float kRangeCursorDot = Mathf.Cos(10 * Mathf.Deg2Rad);   // �J�[�\�����Ƃ�����ς͈̔�
    private const float kCursorLimitDistance = 30.0f;
    private const float kCursorLimitSqrDistance = kCursorLimitDistance * kCursorLimitDistance;

    /* �ϐ� */
    private GameObject _target;         // �^�[�Q�b�g�̃I�u�W�F�N�g���
    private Vector3 _centerPos;         // ���S���W
    private Vector3 _cameraPos;         // ���S���W
    private float _rotLeftrightSwing;   // ���E�̃J�����̉�]��
    private float _rotUpdownSwing;      // �㉺�̃J�����̉�]��
    private bool _isUpdownSwing;        // �㉺�ɃJ������h�炵�Ă��邩
    private bool _isLeftrightSwing;     // ���E�ɓ��͂�����
    private bool _isUpdownInput;        // �㉺�ɓ��͂�����
    private bool _isReset;              // ���Z�b�g������

    // FIXME: �Ȃ񂩂��������̕ϐ����ɕύX
    [SerializeField] private GameObject _canvas;    // �L�����o�X
    [SerializeField] private GameObject _barPrefab; // ���I�u�W�F�N�g
    private List<GameObject> _cursorObjs;    // �J�[�\���Ƃ����I�u�W�F�N�g�̏�񂽂�
    private GameObject _hpBarObj;            // HP�o�[�̑ΏۂƂȂ�I�u�W�F�N�g
    private GameObject _hpBar;               // HP�o�[���̂̃I�u�W�F�N�g

    private void Start()
    {
        // �^�[�Q�b�g(�v���C���[)������擾
        _target = GameObject.Find("Player");

        /* �����ݒ� */
        // ���S���W
        _centerPos = _target.transform.position;
        // �J�������W
        _cameraPos = _centerPos + new Vector3(0, kShiftPosY, -kDistance);
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

        _cursorObjs = new List<GameObject>();
        // �J�[�\���ɓ�������ɔ_���ǉ�
        for (int i = 0; i < FarmManager.kFarmNum; ++i)
        {
            _cursorObjs.Add(GameObject.Find("Farm" + i));
        }
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

        Cursor();
    }

    /// <summary>
    /// �J�����̐��ʕ������擾(Y���͖���)
    /// </summary>
    /// <returns>���ʕ���</returns>
    public Vector3 GetForward()
    {
        return transform.forward;
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
        Transform targetTrs = _target.transform;

        // ���S���X�V���邩
        bool isUpdateCenter = (_centerPos != targetTrs.position);
        Debug.Log(isUpdateCenter);
        // ���S�ʒu�̍X�V
        if (isUpdateCenter)
        {
            _centerPos = Vector3.Lerp(_centerPos, targetTrs.position, 0.5f);

            // ����Ȃ�0�ɋ߂Â���������d�Ȃ��Ă��邱�Ƃ�
            if ((_centerPos - targetTrs.position).sqrMagnitude < 0.001f)
            {
                _centerPos = targetTrs.position;
            }
        }

        // ��]�܂��̓��Z�b�g���Ă����
        if (_isReset || _isLeftrightSwing || _isUpdownSwing || isUpdateCenter)
        {
            _cameraPos = _centerPos;

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
                _cameraPos.x += sinLeftright * (kDistance * cosUpdown);
                _cameraPos.y += kShiftPosY + kDistance * sinUpdown * -1.0f;
                _cameraPos.z += cosLeftright * (kDistance * cosUpdown) * -1.0f;
            }
            // ���E��]�̂�
            else
            {
                // ��]�����ʒu�̓K�p
                _cameraPos.x += sinLeftright * kDistance;
                _cameraPos.y += kShiftPosY;
                _cameraPos.z += cosLeftright * kDistance * -1.0f;
            }
        }

        

        // �ʒu�̑�� 
        transform.position = _cameraPos;

        // �I�u�W�F�N�g�̌����ύX
        Vector3 lookPos = _centerPos;
        lookPos.y += kShiftPosY;
        transform.LookAt(lookPos);
    }

    // FIXME: ���������̕ϐ�����
    /// <summary>
    /// ��ԋ߂��ɂ��鐳�ʂɂ���I�u�W�F�N�g��HP����\������
    /// </summary>
    private void Cursor()
    {
        return;

        GameObject drawingObj = null;
        float nowSqrDist = 0.0f;

        foreach (var item in _cursorObjs)
        {
            // ���̃I�u�W�F�N�g���j��or���S���Ă���Ύ���
            if (!IsExist(item)) continue;

            // ���g����I�u�W�F�N�g�܂ł̃x�N�g���𐶐�
            Vector3 cameraToitemVec = item.transform.position - transform.position;

            // �������������ꂷ���Ă����玟��
            float sqrDist = cameraToitemVec.sqrMagnitude;
            if (sqrDist > kCursorLimitSqrDistance) continue;

            // ���K��
            cameraToitemVec.Normalize();
            // ����
            float dot = Vector3.Dot(cameraToitemVec, transform.forward);

            // �ݒ�͈͓����̔���
            if (dot > kRangeCursorDot)
            {
                // 1��ڂ̃q�b�g
                if (!drawingObj)
                {
                    // ���̂܂ܑ��
                    drawingObj = item;
                    nowSqrDist = sqrDist;
                }
                // 2��ڈȍ~�̃q�b�g
                else
                {
                    // ���݂̋�����菬�����Ȃ���
                    if (sqrDist < nowSqrDist)
                    {
                        drawingObj = item;
                        nowSqrDist = sqrDist;
                    }
                }
            }
        }

        // �N�ɂ��q�b�g���Ȃ�������I��
        if (!drawingObj)
        {
            // ����HP�o�[��`�悵�Ă���Ώ���
            if (_hpBar)
            {
                Destroy(_hpBar);
                // ���������Ă��Ȃ��Ƃ���
                _hpBarObj = null;
                _hpBar = null;
            }

            return;
        }

        // HP�o�[�̐���
        CreateHpBar(drawingObj);

        // HP�o�[�̏��X�V
        ChangeHpBarInfo();
    }

    private bool IsExist(GameObject item)
    {
        if (item.tag == "Farm")
        {
            // MEMO: �_��͔j�󂳂�Ă��邩���m�F���邽�ߌ��ʂ𔽓]���đ���
            return !item.GetComponent<Farm>().IsBreak;
        }
        else if (item.tag == "Enemy")
        {
            return item.GetComponent<EnemyBase>().IsExist;
        }

        return false;
    }

    private void CreateHpBar(GameObject item)
    {
        // �I�u�W�F�N�g���Ⴄ�ꍇ�͕ύX
        if (_hpBarObj != item)
        {
            _hpBarObj = item;
        }
        
        // HP�o�[����������Ă��Ȃ���΍쐬
        if (!_hpBar)
        {
            // TODO: �쐬
            _hpBar = Instantiate(_barPrefab, _canvas.transform);
        }
    }

    /// <summary>
    /// Hp�o�[�̏���ύX
    /// </summary>
    private void ChangeHpBarInfo()
    {
        int nowHp = 0;
        int deltaHp = 0;
        int maxHp = 100;

        // �_��̏ꍇ
        if (_hpBarObj.tag == "Farm")
        {
            Farm script = _hpBarObj.GetComponent<Farm>();
            nowHp = script.Hp;
            deltaHp = script.DeltaHp;
            maxHp = script.MaxHp;
        }
        // �G�̏ꍇ
        else if (_hpBarObj.tag == "Enemy")
        {
            // TODO: �G�̏ꍇ����Ɠ��������ɂȂ�悤�ɂ���
            EnemyBase script = _hpBarObj.GetComponent<EnemyBase>();
            nowHp = script.Hp;
            deltaHp = script.DeltaHp;
            maxHp = script.MaxHp;
        }

        // FIXME: �v�f��enum�Œ�`����悤��
        /* �e�L�X�g�̕ύX */
        Text text = _hpBar.transform.GetChild(2).GetComponent<Text>();
        // �`�掞 �� ��񖼁F������/������
        text.text = _hpBarObj.name + ":" + nowHp + " / " + maxHp;

        /* �X���C�_�[�̕ύX */
        Slider hpSlider = _hpBar.transform.GetChild(1).GetComponent<Slider>();
        Slider deltahpSlider = _hpBar.transform.GetChild(0).GetComponent<Slider>();
        // ����
        float nowRate = (float)nowHp / (float)maxHp;
        float deltaRate = (float)deltaHp / (float)maxHp;
        // �ύX
        hpSlider.value = nowRate;
        deltahpSlider.value = deltaRate;
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
