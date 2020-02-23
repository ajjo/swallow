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
        txt.text = "1";

        LevelButtons lb = cloneItem.GetComponent<LevelButtons>();
        lb.SetState(levels[0]);
        txt.enabled = true;

        for (int i = 1; i < levels.Length; i++)
        {
            Transform obj = Instantiate(cloneItem, parent);
            Button but = obj.GetComponent<Button>();
            txt = obj.GetComponentInChildren<Text>();
            txt.text = (i + 1).ToString();

            lb = obj.GetComponent<LevelButtons>();
            lb.SetState(levels[i]);

            but.enabled = (levels[i] > 0);
            txt.enabled = (levels[i] > 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LevelSelected()
    {
        gameObject.SetActive(false);
        gameManager.Play();
    }
}
