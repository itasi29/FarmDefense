using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    struct SpawnerData
    { 
        private int _no;
        private int _createFrame;
        private int _spawnerNo;
    }

    /* �ϐ� */
    // �X�e�[�W��
    [SerializeField] private string _stageName;
    // ���݂�wave�ԍ�
    private int _nowWaveNo;
    // wave�J�n����̌o�߃t���[��
    private int _elapsFrame;
    // �����I�����Ă��邩
    private bool _isEndCreate;

    void Start()
    {
        
    }

    
    void FixedUpdate()
    {
        
    }
}
