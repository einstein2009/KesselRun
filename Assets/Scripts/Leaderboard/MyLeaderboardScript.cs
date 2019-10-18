using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public class MyLeaderboardScript : MonoBehaviour {

    public Text[] names;
    public Text[] scores;
    public Text addScoreBtnText;
	public Text playerName;

    public GameObject leaderboardUI1;
    public GameObject leaderboardUI2;

    List<dreamloLeaderBoard.Score> scoreList;

    int score = 0;

    private bool used = false;

    // Reference to the dreamloLeaderboard prefab in the scene
    dreamloLeaderBoard dl;

	void Start () 
	{
		dl = dreamloLeaderBoard.GetSceneDreamloLeaderboard();
        //dl.LoadScores();
        score = (int)PlayerPrefs.GetFloat("Score") + 1;
    }

    void DisplayLeaderboardUI()
    {
        leaderboardUI1.SetActive(true);
        leaderboardUI2.SetActive(true);
    }

    void HideLeaderboardUI()
    {
        leaderboardUI1.SetActive(false);
        leaderboardUI2.SetActive(false);
    }

    public void AddNewScore()
    {
        if (dl.publicCode == "") Debug.LogError("You forgot to set the publicCode variable");
        if (dl.privateCode == "") Debug.LogError("You forgot to set the privateCode variable");
        
        //if (scoreList == null)
        //{
            addScoreBtnText.text = "(loading...)";
        //}
        if (used)
        {
            addScoreBtnText.text = "Saved";
        }
        else if (playerName.text != "")
        {
            StartCoroutine(AddScoreRoutine());
        } else
        {
            addScoreBtnText.text = "Save";
        }
            
    }

    System.Collections.IEnumerator AddScoreRoutine()
    {
        dl.AddScore(playerName.text, score);
        //yield return new WaitForSeconds(2.1f);
        //dl.LoadScores();
        used = true;
        yield return new WaitForSeconds(2.1f);
        scoreList = dl.ToListHighToLow();
        yield return new WaitForSeconds(2.1f);
        DisplayScores();
        addScoreBtnText.text = "Saved";
        yield return new WaitForSeconds(2.1f);
        scoreList = dl.ToListHighToLow();
        yield return new WaitForSeconds(2.1f);
        DisplayScores();
        
    }

    void DisplayScores()
    {
        DisplayLeaderboardUI();
        int maxToDisplay = 10;
        int count = 0;
        foreach (dreamloLeaderBoard.Score currentScore in scoreList)
        {
            try
            {
                names[count].text = currentScore.playerName;
                scores[count].text = currentScore.score.ToString();
                count++;
                if (count >= maxToDisplay) break;
            } catch(ArgumentOutOfRangeException e)
            {
                Debug.Log(e.ToString());
                break;
            }
            
        }
    }

}
