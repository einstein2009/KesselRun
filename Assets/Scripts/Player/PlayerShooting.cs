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

    Text bombText;
    Text weaponText;

    public AudioSource shotSound;
    public ParticleSystem obstacleExplosion;

    public AudioSource bombAudio;
    public AudioSource rapidAudio;

    public GameObject WeaponGlow;
    public GameObject BombGlow;

    void Start()
    {
        bombText = GameObject.Find("BombReadyText").GetComponent<Text>();
        weaponText = GameObject.Find("WeaponReadyText").GetComponent<Text>();

    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire) //|| Input.GetKeyDown(KeyCode.Space) && Time.time > nextFire)
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
        //if (Input.GetKeyDown(KeyCode.B) && bombReady || Input.GetButton("Fire2") && bombReady)
        if(Input.GetButton("Fire2") && bombReady)
        {
            TriggerMegabombDestruction();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Powerup"))
        {
            other.gameObject.SetActive(false);
            // Add Effect
            if (other.name.Contains("Rapidfire"))
            {
                rapidAudio.Play();
                SetRapidfire();
                GainRapidfire();
                //Debug.Log("Rapidfire On");
            }
            else if (other.name.Contains("Megabomb"))
            {
                bombAudio.Play();
                bombReady = true;
                SetBombReadyText(true);
                //Debug.Log("Bomb Ready");
            }
        } 
    }

    void TriggerMegabombDestruction()
    {
        bombReady = false;
        SetBombReadyText(false);
        DestroyAllEnemiesAndObstacles();
        Debug.Log("Destroying Enemies");
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
            if(hp != null)
            {
                hp.Die();
            } else
            {
                enemy.SetActive(false);
            }
        }
        foreach (GameObject obstacle in obstacles)
        {
            Instantiate(obstacleExplosion, obstacle.transform.position, Quaternion.identity);
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
