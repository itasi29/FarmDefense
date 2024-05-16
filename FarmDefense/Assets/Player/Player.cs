using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//ÉvÉåÉCÉÑÅ[ÇÃèàóùÇ‹Ç∆ÇﬂÇÈÇÊ
public class Player : MonoBehaviour
{
    private const float kSpeed = 0.2f;

    private Vector3 _moveDir;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        this.transform.position += _moveDir;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Debug.Log(_moveDir);
    }
    private void Move()
    {
        
        Vector3 dirVec = new Vector3(0,0,0);

        dirVec.x = Input.GetAxis("Horizontal");
        dirVec.z = Input.GetAxis("Vertical");

        dirVec.Normalize();

        _moveDir = dirVec * kSpeed;

        _moveDir.Normalize();


        Debug.Log(_moveDir);
    }
}
