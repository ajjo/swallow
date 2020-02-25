using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompleteUI : MonoBehaviour
{
    public Text levelCompleted;
    public Text score;
    public Button levelButton;
    public Button quitButton;
    public GameManager gameManager;
    public Image[] stars;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show(bool outOfTime, int starsEarned = -1, int levelScore = 0)
    {
        levelCompleted.text = outOfTime ? "You ran out of time" : "Level Complete";
        gameObject.SetActive(true);

        Text buttonText = levelButton.GetComponentInChildren<Text>();
        buttonText.text = outOfTime ? "Retry" : "Next";
        gameManager.LevelOver();

        for(int i=0;i<3;i++)
        {
            stars[i].enabled = i <= starsEarned;
        }

        score.text = levelScore.ToString();
    }

    public void PlayAgain()
    {
        Text buttonText = levelButton.GetComponentInChildren<Text>();
        if (buttonText.text == "Retry")
            gameManager.Retry();
        else
            gameManager.NextLevel();
    }

    public void QuitLevel()
    {
        gameManager.GotoHome();
    }
}
