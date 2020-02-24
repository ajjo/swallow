using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompleteUI : MonoBehaviour
{
    public Text levelCompleted;
    public Text message;
    public Button levelButton;
    public Button quitButton;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show(bool outOfTime)
    {
        levelCompleted.text = outOfTime ? "You ran out of time" : "Level Complete";
        message.text = outOfTime ? "Stars = 0" : "Stars = 3";
        gameObject.SetActive(true);

        Text buttonText = levelButton.GetComponentInChildren<Text>();
        buttonText.text = outOfTime ? "Play Again" : "Next Level";
        gameManager.LevelOver();
    }

    public void PlayAgain()
    {
        Text buttonText = levelButton.GetComponentInChildren<Text>();
        if (buttonText.text == "Play Again")
            gameManager.Retry();
        else
            gameManager.NextLevel();
    }

    public void QuitLevel()
    {
        gameManager.GotoHome();
    }
}
