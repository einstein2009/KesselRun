using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{

    public float speed;

    private GameObject player;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Start()
    {
        InvokeRepeating("IncreaseSpeed", 5f, 30f);
    }

    void IncreaseSpeed()
    {
        speed += 0.5f * player.GetComponent<PlayerMovement>().speedIncreaseCount;
    }

    void Update()
    {
        transform.position += transform.forward * speed;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth hp = other.gameObject.GetComponent<EnemyHealth>();
            if (hp != null)
            {
                hp.Die();
            } else
            {
                Destroy(other.gameObject);
            }
            Destroy(gameObject);
        }
    }

}
