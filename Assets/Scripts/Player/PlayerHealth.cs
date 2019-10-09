using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PlayerHealth : MonoBehaviour
{
    public bool isImmune = false;

    public int startingHealth = 100;
    public int startingShields = 0;
    public int currentHealth;
    public int currentShields;

    public Slider healthSlider;
    public Slider shieldSlider;
    public Image damageImage;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);

    public GameObject playerShield;
    public GameObject playerExplosion;
    public GameObject player;

    public AudioSource backgroundAudio;
    public AudioSource deathAudio;
    public AudioSource damageAudio;

    PlayerMovement playerMovement;
    PlayerShooting playerShooting;
    bool isDead;
    bool damaged;
    bool shieldOn;
    

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerShooting = GetComponentInChildren<PlayerShooting>();
        currentHealth = startingHealth;
        currentShields = startingShields;
    }

    void Start()
    {
        backgroundAudio.volume = 0.5f;
        deathAudio.volume = 0.1f;
        damageAudio.volume = 0.1f;
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

        if (currentShields <= 0)
        {
            TurnShieldOff();
        }
    }


    public void TakeDamage(int amount)
    {
        if (isImmune)
            return;

        damaged = true;

        damageAudio.Play();

        //currentShields -= amount;
        //shieldSlider.value = currentShields;

        if(currentShields > 0)
        {
            currentShields -= amount;
            shieldSlider.value = currentShields;
        }
        else if (currentShields <= 0 && currentHealth > 0)
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
        if (other.CompareTag("Powerup"))
        {
            other.gameObject.SetActive(false);
            // Add Effect
            if (other.name.Contains("Heal"))
            {
                currentHealth += 50;
                healthSlider.value = currentHealth;
                if(currentHealth > 100)
                {
                    currentHealth = 100;
                }
                Debug.Log("Healing 50");
            } else if (other.name.Contains("Shield"))
            {
                currentShields += 100;
                shieldSlider.value = currentShields;
                if(currentShields > 100)
                {
                    currentShields = 100;
                }
                Debug.Log("Shield +100");
                if (shieldOn)
                    return;
                TurnShieldOn();

            } 
        }

    }

    void TurnShieldOn()
    {
        playerShield.SetActive(true);
        //playerHealth.isImmune = true;
        shieldOn = true;
    }

    void TurnShieldOff()
    {
        playerShield.SetActive(false);
        //playerHealth.isImmune = false;
        shieldOn = false;
    }


    public void Death()
    {
        deathAudio.Play();
        playerMovement.enabled = false;
        playerShooting.enabled = false;
        MeshRenderer m = player.GetComponent<MeshRenderer>();
        m.enabled = false;
        for (int i = 0; i < 3; i++)
        {
            Instantiate(playerExplosion, transform.position, transform.rotation);
        }

        StartCoroutine(FadeOutBackground());

        isDead = true;
    }


    IEnumerator FadeOutBackground()
    {
        while (backgroundAudio.volume > 0.001f)
        {
            backgroundAudio.volume -= Time.deltaTime / 0.5f;
            yield return null;
        }

    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }
}
