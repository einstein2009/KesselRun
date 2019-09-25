using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{

    public float speed;

    void Update()
    {
        transform.position += transform.forward * speed;
    }

}
