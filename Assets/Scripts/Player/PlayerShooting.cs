using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject shot;
    public Transform[] shotSpawns;
    public float fireRate = 2;

    private float nextFire;
    private bool rapidfire;

    public AudioSource shotSound;

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            /*if (rapidfire)
            {
                Instantiate(shot, shotSpawns[0].position, shotSpawns[0].rotation);
                Instantiate(shot, shotSpawns[1].position, shotSpawns[1].rotation);
                Instantiate(shot, shotSpawns[2].position, shotSpawns[2].rotation);
            } else
            {
                Instantiate(shot, shotSpawns[0].position, shotSpawns[0].rotation);
            }*/
            Instantiate(shot, shotSpawns[0].position, shotSpawns[0].rotation);
            Instantiate(shot, shotSpawns[1].position, shotSpawns[1].rotation);
            Instantiate(shot, shotSpawns[2].position, shotSpawns[2].rotation);
            shotSound.Play();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Powerup"))
        {
            other.gameObject.SetActive(false);
            // Add Effect
            if (other.gameObject.name.Contains("Rapidfire"))
            {
                // Include a timer UI element
                SetRapidfire();
            }
            else if (other.gameObject.name.Contains("Megabomb"))
            {
                DestroyAllEnemies();
            }
        } 
    }

    public void DestroyAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemies)
        {
            // Instantiate an explosion effect
            Destroy(enemy);
        }
    }

    public void SetRapidfire()
    {
        if (!rapidfire)
        {
            fireRate /= 10;
            rapidfire = true;
            Invoke("RemoveRapidfire", 4);
        } else
        {
            if (IsInvoking("RemoveRapidfire"))
            {
                CancelInvoke("RemoveRapidfire");
            }
            Invoke("RemoveRapidfire", 4);
        }
        
    }

    void RemoveRapidfire()
    {
        fireRate *= 10;
        rapidfire = false;
    }
}
