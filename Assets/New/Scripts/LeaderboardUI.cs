using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardUI : MonoBehaviour
{
    public Transform cloneItem;
    public Transform parent;

    // Start is called before the first frame update
    void Start()
    {
        for (int i=0;i<15;i++)
        {
            Transform obj = Instantiate(cloneItem, parent);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
