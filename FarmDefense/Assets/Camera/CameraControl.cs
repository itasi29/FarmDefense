using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    /* �萔 */
    private const float kDistance = 2.2f;   // �^�[�Q�b�g�ƃJ�����Ƃ̋���
    private const float kShiftY   = 1.2f;   // �^�[�Q�b�g���S�����ɂ��炷��

    /* �ϐ� */
    private GameObject target;  // �^�[�Q�b�g�̏��
    private Vector3 pos;  // �J�����̍��W

    void Start()
    {
        // �^�[�Q�b�g�̎擾
        target = GameObject.Find("Player");

        // �����ʒu�̐ݒ�
        pos = new Vector3(0.0f, kShiftY, kDistance);
    }

    private void Update()
    {
        Vector3 vel = Vector3.zero; // ���x�p�ϐ�



        if ()
        {

        }
    }

    private void FixedUpdate()
    {
        
    }
}
