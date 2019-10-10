using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public int attackDamage = 200;
    public GameObject obstacleExplosion;

    GameObject player;
    PlayerHealth playerHealth;


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            Instantiate(obstacleExplosion, transform.position, transform.rotation);
            Attack();
        }
    }

    void Attack()
    {
        if (playerHealth.currentHealth > 0)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }
}
