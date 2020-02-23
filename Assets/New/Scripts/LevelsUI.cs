using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelsUI : MonoBehaviour
{
    public Transform cloneItem;
    public Transform parent;
    public GameManager gameManager;

    public void Init(int [] levels)
    {
        Text txt = cloneItem.GetComponentInChildren<Text>();
        Button but = cloneItem.GetComponent<Button>();
        but.onClick.AddListener(() => LevelSelected(1));
        txt.text = "1";

        LevelButtons lb = cloneItem.GetComponent<LevelButtons>();
        lb.SetState(levels[0]);
        txt.enabled = true;

        for (int i = 1; i < levels.Length; i++)
        {
            Transform obj = Instantiate(cloneItem, parent);
            but = obj.GetComponent<Button>();
            txt = obj.GetComponentInChildren<Text>();
            txt.text = (i + 1).ToString();

            lb = obj.GetComponent<LevelButtons>();

            if (levels[i] > 0)
                lb.SetState(levels[i]);
            else if (levels[i - 1] > 0)
                lb.SetState(4);
            else
                lb.SetState(0);

            but.enabled = (levels[i - 1] > 0);
            txt.enabled = (levels[i-1] > 0);

            int index = i + 1; // Assign to local variable fix the closure problem.
            but.onClick.AddListener(delegate { LevelSelected(index); });
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LevelSelected(int level)
    {
        Debug.Log("Level is " + level.ToString());
        gameObject.SetActive(false);
        gameManager.Play(level);
    }
}
