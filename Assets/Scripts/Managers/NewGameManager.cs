using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class NewGameManager : MonoBehaviour
{
    public ApplicationManager applicationManager;
    public float startDelay = 5f;

    Animator anim;
    float startTimer;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (applicationManager.newGameBool == true)
        {
            anim.SetTrigger("NewGame");

            startTimer += Time.deltaTime;

            if (startTimer >= startDelay)
            {
                SceneManager.LoadScene("MainLevel", LoadSceneMode.Single);
            }
        }
    }
}
