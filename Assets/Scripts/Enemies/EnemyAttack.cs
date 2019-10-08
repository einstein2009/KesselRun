using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int attackDamage = 50;
    private bool hasdonedamage = false;

    GameObject player;
    PlayerHealth playerHealth;

    float timer;


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player && !hasdonedamage)
        {
            Attack();
            hasdonedamage = true;
            this.gameObject.GetComponent<EnemyHealth>().Die();
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