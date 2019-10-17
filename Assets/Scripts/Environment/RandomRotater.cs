using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotater : MonoBehaviour
{
    public float rotspeedx = 0;
    public float rotspeedy = 0;
    public float rotspeedz = 0;


    void Update()
    {
        transform.Rotate(rotspeedx * Time.deltaTime, rotspeedy * Time.deltaTime, rotspeedz * Time.deltaTime);
    }
}
