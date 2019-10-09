using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public GameObject explosion;

    public void Die()
    {
        Instantiate(explosion, transform.position, transform.rotation);
        gameObject.SetActive(false);
    }
}
