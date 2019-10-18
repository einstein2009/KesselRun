using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    public static float realScore;
    public static int score;
    public PlayerHealth playerHealth;
    public int warpingMultiplier;

    private GameObject player;
    Text text;

    void Awake()
    {
        text = GetComponent<Text>();
        score = 0;
        //DontDestroyOnLoad(this);
        player = GameObject.FindGameObjectWithTag("Player");
    }


    void Update()
    {
        if (player.GetComponent<PlayerMovement>().warping)
        {
            warpingMultiplier = 5;
        } else
        {
            warpingMultiplier = 1;
        }

        if (playerHealth.currentHealth > 0)
        {
            realScore += Time.deltaTime * player.GetComponent<PlayerMovement>().speed * warpingMultiplier;
            score = (int)realScore;
            text.text = score.ToString("##,#") + " Light Years";

            PlayerPrefs.SetInt("Score", score);
        }

    }
}
