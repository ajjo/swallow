using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsUI : MonoBehaviour
{
    public Transform cloneItem;
    public Transform parent;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 15; i++)
        {
            Transform obj = Instantiate(cloneItem, parent);
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
