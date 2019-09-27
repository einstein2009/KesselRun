using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public float gameOverDelay = 3f;

    Animator anim;
    float gameOverTimer;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        if (playerHealth.currentHealth <= 0)
        {
            anim.SetTrigger("GameOver");

            gameOverTimer += Time.deltaTime;

            if (gameOverTimer >= gameOverDelay)
            {
                SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
            }
        }
    }
}
