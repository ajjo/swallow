using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UnityBoolEvent : UnityEvent<bool>
{

}

public class GameUI : MonoBehaviour
{
    public Text timer;
    public Text lives;
    public Text score;

    private int numLives = 3;
    private float t = 5.0f;
    private bool start = false;
    private int scorePerLevel = 10;
    private int totScore = 0;

    public UnityBoolEvent timerEvent = new UnityBoolEvent();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!start)
            return;

        t -= Time.deltaTime;
        if(t < 0.0f)
        {
            t = 0.0f;
            start = false;
            timerEvent.Invoke(true);
            Debug.Log("Timer ran out");
        }
        timer.text = t.ToString("0");
    }

    public void StartTimer()
    {
        t = 5.0f;
        start = true;
    }

    public void StopTimer()
    {
        start = false;
    }

    public void LoseLife()
    {
        numLives -= 1;
        if (numLives < 0)
            numLives = 0;

        lives.text = numLives.ToString();
    }

    public void AddScore()
    {
        totScore += scorePerLevel;
        score.text = totScore.ToString();
    }
}
