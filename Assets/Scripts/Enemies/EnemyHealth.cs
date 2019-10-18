using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public GameObject explosion;

    public void Die(bool lowVolume)
    {
        ParticleSystem explosionEffect = Instantiate(explosion.GetComponent<ParticleSystem>(), transform.position, transform.rotation);
        if (lowVolume)
        {
            explosionEffect.GetComponent<AudioSource>().volume = 0.1f;
        } 
        gameObject.SetActive(false);
    }
}
