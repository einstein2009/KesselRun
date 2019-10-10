using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{

    public float speed = 54f;

    private GameObject player;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Start()
    {
        IncreaseSpeed();
    }

    void IncreaseSpeed()
    {
        speed += 18f * player.GetComponent<PlayerMovement>().speedIncreaseCount;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.World);
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
