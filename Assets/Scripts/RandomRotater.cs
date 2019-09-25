using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotater : MonoBehaviour
{
    public float rotspeed;


    void Update()
    {
        transform.Rotate(rotspeed, transform.rotation.y, transform.rotation.z);
    }
}
