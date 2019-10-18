using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScoreSetter : MonoBehaviour
{
    public Text theScore = null;

    public Button saveScoreButton;
    public InputField saveScoreInputField;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;

        int temp = PlayerPrefs.GetInt("Score");

        theScore.text = temp.ToString("##,#") + " Light Years";

    }

    public void SaveScoreButtonPress()
    {
        saveScoreButton.GetComponent<Button>().enabled = false;
        saveScoreButton.transform.Find("SaveScoreText").gameObject.SetActive(false);
        saveScoreInputField.gameObject.SetActive(enabled);
        saveScoreButton.transform.Find("SaveButton").gameObject.SetActive(true);
    }

}
