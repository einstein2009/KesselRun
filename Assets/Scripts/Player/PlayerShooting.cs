using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{
    public GameObject shot;
    public Transform[] shotSpawns;
    public float fireRate = 2;

    private float nextFire;
    private bool rapidfire;
    private bool bombReady;
    private bool bombBeingUsed;

    Text bombText;
    Text weaponText;

    public AudioSource shotSound;
    public ParticleSystem obstacleExplosion;

    public AudioSource[] audioToBeLoweredByBomb;
    public AudioSource bombAftermanAudio;
    public AudioSource bombAudio;
    public AudioSource rapidAudio;

    public GameObject WeaponGlow;
    public GameObject BombGlow;
    public GameObject bombEffect;

    void Start()
    {
        bombText = GameObject.Find("BombReadyText").GetComponent<Text>();
        weaponText = GameObject.Find("WeaponReadyText").GetComponent<Text>();
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire) //|| Input.GetKeyDown(KeyCode.Space) && Time.time > nextFire)
        {

            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawns[0].position, shotSpawns[0].rotation);
            Instantiate(shot, shotSpawns[1].position, shotSpawns[1].rotation);
            Instantiate(shot, shotSpawns[2].position, shotSpawns[2].rotation);
            shotSound.Play();
        }
        //if (Input.GetKeyDown(KeyCode.B) && bombReady || Input.GetButton("Fire2") && bombReady)
        if(Input.GetButton("Fire2") && bombReady && !bombBeingUsed)
        {
            bombBeingUsed = true;
            TriggerMegabombDestruction();
            Invoke("ResetBombBeingUsed", 7);
        }

        if (bombBeingUsed)
        {
            bombText.text = "OVERHEAT";
            bombText.color = Color.yellow;
        } else if (bombReady)
        {
            SetBombReadyText(true);
        } else
        {
            SetBombReadyText(false);
        }
    }

    void ResetBombBeingUsed()
    {
        bombBeingUsed = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Powerup"))
        {
            other.gameObject.SetActive(false);
            // Add Effect
            if (other.name.Contains("Rapidfire"))
            {
                WeaponGlow.GetComponent<Animation>().Play();
                rapidAudio.Play();
                SetRapidfire();
                GainRapidfire();
                //Debug.Log("Rapidfire On");
            }
            else if (other.name.Contains("Megabomb"))
            {
                BombGlow.GetComponent<Animation>().Play();
                bombAudio.Play();
                bombReady = true;
                //Debug.Log("Bomb Ready");
            }
        } 
    }

    void TriggerMegabombDestruction()
    {
        bombAftermanAudio.Play();
        Vector3 relativePos = transform.position;
        Instantiate(bombEffect, transform.position, transform.rotation);
        LowerAudioSources();
        Invoke("ResetAudioSources", 4);
        bombReady = false;
        SetBombReadyText(false);
        InvokeRepeating("DestroyAllEnemiesAndObstacles", 0, 0.1f);
        //DestroyAllEnemiesAndObstacles();
        Debug.Log("Destroying Enemies");
    }

    void LowerAudioSources()
    {
        foreach(AudioSource audio in audioToBeLoweredByBomb)
        {
            audio.volume /= 10f;
        }
    }

    void ResetAudioSources()
    {
        CancelInvoke("DestroyAllEnemiesAndObstacles");
        /*foreach (AudioSource audio in audioToBeLoweredByBomb)
        {
            audio.volume *= 10f;
        }*/
        StartCoroutine(GraduallyResetVolume());
    }
    
    IEnumerator GraduallyResetVolume()
    {
        foreach (AudioSource audio in audioToBeLoweredByBomb)
        {
            audio.volume *= 2f;
        }
        yield return new WaitForSeconds(0.5f);
        foreach (AudioSource audio in audioToBeLoweredByBomb)
        {
            audio.volume *= 2f;
        }
        yield return new WaitForSeconds(0.5f);
        foreach (AudioSource audio in audioToBeLoweredByBomb)
        {
            audio.volume *= 2f;
        }
        yield return new WaitForSeconds(0.5f);
        foreach (AudioSource audio in audioToBeLoweredByBomb)
        {
            audio.volume *= 1.25f;
        }
        yield return new WaitForSeconds(0.5f);
    }

    void SetBombReadyText(bool ready)
    {
        if (ready)
        {
            bombText.text = "READY";
            bombText.color = Color.green;
        } else
        {
            bombText.text = "NOT READY";
            bombText.color = Color.red;
        }
    }

    public void DestroyAllEnemiesAndObstacles()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        EnemyHealth hp;
        foreach (GameObject enemy in enemies)
        {
            hp = enemy.GetComponent<EnemyHealth>();
            if (hp != null)
                hp.Die(true);
            else
                enemy.SetActive(false);
        }
        foreach (GameObject obstacle in obstacles)
        {
            ParticleSystem explosionEffect = Instantiate(obstacleExplosion, obstacle.transform.position, Quaternion.identity);
            explosionEffect.GetComponent<AudioSource>().volume = 0.1f;
            Destroy(obstacle);
        }
    }

    public void SetRapidfire()
    {
        if (!rapidfire)
        {
            fireRate /= 3;
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

    // RAPIDFIRE TEXT BEHAVIOR

    private Coroutine rapidfireCoroutine;
    private bool CR_RUNNING;

    public void GainRapidfire()
    {
        if (IsInvoking("clearPowerupText"))
        {
            CancelInvoke("clearPowerupText");
        }
        weaponText.text = "RAPIDFIRE! (5)";

        if (CR_RUNNING)
        {
            StopCoroutine(rapidfireCoroutine);
        }
        rapidfireCoroutine = StartCoroutine(RapidfireCoroutine());

    }

    IEnumerator RapidfireCoroutine()
    {
        CR_RUNNING = true;
        yield return new WaitForSeconds(1);
        for (int i = 4; i > 0; i--)
        {
            if (IsInvoking("clearPowerupText"))
            {
                CancelInvoke("clearPowerupText");
            }
            weaponText.text = "RAPIDFIRE! (" + i + ")";
            yield return new WaitForSeconds(1);
        }
        clearPowerupText();
        CR_RUNNING = false;
    }

    void clearPowerupText()
    {
        weaponText.text = "ACTIVE";
    }

    void RemoveRapidfire()
    {
        fireRate *= 3;
        rapidfire = false;
    }
}
