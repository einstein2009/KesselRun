using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int startingShields = 0;
    public int currentHealth;
    public int currentShields;
    public Slider healthSlider;
    public Slider shieldSlider;
    public Image damageImage;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);

    PlayerMovement playerMovement;
    PlayerShooting playerShooting;
    bool isDead;
    bool damaged;


    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerShooting = GetComponentInChildren<PlayerShooting>();
        currentHealth = startingHealth;
        currentShields = startingShields;
    }


    void Update()
    {
        if (damaged)
        {
            damageImage.color = flashColour;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
    }


    public void TakeDamage(int amount)
    {
        damaged = true;

        currentShields -= amount;

        shieldSlider.value = currentShields;

        if (currentShields <= 0 && currentHealth > 0)
        {
            currentHealth -= amount;
            healthSlider.value = currentHealth;
        }

        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Health Pickup"))
        {
            other.gameObject.SetActive(false);
            currentHealth += 50;
            healthSlider.value = currentHealth;
        }

        if (other.gameObject.CompareTag("Shield Pickup"))
        {
            other.gameObject.SetActive(false);
            currentShields += 100;
            shieldSlider.value = currentShields;
        }
    }


    void Death()
    {
        isDead = true;

        playerMovement.enabled = false;
        playerShooting.enabled = false;
    }


    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }
}
