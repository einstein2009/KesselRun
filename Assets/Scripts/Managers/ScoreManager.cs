using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    public static float score;
    public PlayerHealth playerHealth;

    Text text;


    void Awake()
    {
        text = GetComponent<Text>();
        score = 0;
        DontDestroyOnLoad(this);
    }


    void Update()
    {
        if(playerHealth.currentHealth > 0)
        {
            score += Time.deltaTime;
            text.text = score.ToString("F2") + " Light Years";
        }
    }
}
