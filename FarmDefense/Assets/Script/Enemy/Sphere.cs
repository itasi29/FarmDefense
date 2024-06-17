using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    private int speed;
    private int radius;
    private Vector3 defPosition;
    float x;
    float z;
    // Start is called before the first frame update
    void Start()
    {
        speed = 1;
        radius = 2;

        defPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        x = radius * Mathf.Sin(Time.time * speed);
        z = radius * Mathf.Cos(Time.time * speed);

        transform.position = new Vector3(x + defPosition.x, defPosition.y, z + defPosition.z);
    }
}
