using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    private int speed;
    //private int radius;
    private Vector3 defPosition;
    float x;
    float z;
    float MoveX;
    float MoveZ;
    // Start is called before the first frame update
    [SerializeField] GameObject target;
    void Start()
    {
        speed = 2;
        //radius = 2;

        defPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //x = radius * Mathf.Sin(Time.time * speed);
        //z = radius * Mathf.Cos(Time.time * speed);

        //transform.position = new Vector3(x + defPosition.x, defPosition.y, z + defPosition.z);

        double sb, sbx, sbz;
        float fsbx, fsbz;

        sbx = target.transform.position.x - defPosition.x;
        sbz = target.transform.position.z - defPosition.z;

        fsbx = (float)sbx;
        fsbz = (float)sbz;

        sb = Mathf.Sqrt(fsbx * fsbx + fsbz + fsbz);

        MoveX = fsbx / (float)sb * speed;
        MoveZ = fsbz / (float)sb * speed;

        transform.position = new Vector3(defPosition.x + MoveX, defPosition.y, defPosition.z + MoveZ);

    }
}
