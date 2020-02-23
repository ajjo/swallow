using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtons : MonoBehaviour
{
	public Sprite[] sprites;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetState(int state)
    {
        Image img = GetComponent<Image>();
        img.sprite = sprites[state];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
