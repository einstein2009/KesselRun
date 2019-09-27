using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScoreSetter : MonoBehaviour
{
    public Text theScore = null;

    // Start is called before the first frame update
    void Start()
    {
        float temp = PlayerPrefs.GetFloat("Score");

        theScore.text = temp.ToString("F2");
    }

}
