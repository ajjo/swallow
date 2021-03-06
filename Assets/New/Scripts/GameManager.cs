﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject introUI;
    public Transform[] _Levels;
	public InputManager inputManager;
    public LevelCompleteUI levelCompleteUI;

    private float time;
    private GameObject currentLevelGameObject;
	private int currentLevel = 1;

	private TileFireball fireball;
    private bool nextLevel;

    public RegisterSignin registerSigninUI;
    public LevelsUI levelUI;
    public GameUI gameUI;
    public Transform particles;
    private bool gameStarted = false;
    public bool devMode = false;

    public GameOverUI gameOverUI = null;

    // number of levels
    private int[] levels = new int[20];

	// Start is called before the first frame update
	void Start()
    {
		inputManager.OnSwipeLeft.AddListener(SwipeLeft);
		inputManager.OnSwipeRight.AddListener(SwipeRight);
		inputManager.OnSwipeUp.AddListener(SwipeUp);
		inputManager.OnSwipeDown.AddListener(SwipeDown);

        gameUI.timerEvent.AddListener(LevelFinished);
	}

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Retry()
    {
        Debug.Log("Doing the retry");
        gameStarted = true;
        Destroy(currentLevelGameObject);
        CreateLevel();
        levelCompleteUI.gameObject.SetActive(false);
        gameUI.StartTimer();
    }

    public void LevelOver()
    {
        gameStarted = false;
        gameUI.StopTimer();
    }

    public void Swipe(Vector3 direction,float x,float y)
    {
        if (!gameStarted)
            return;

        Vector3 origin = fireball.transform.position;
        RaycastHit info;
        bool hit = Physics.Raycast(origin, direction, out info);
        if (hit)
        {
            Vector3 p = info.transform.position;
            p.x += x;
            p.y += y;
            fireball.MoveTo(p);

            nextLevel = info.transform.name.Contains("TileHole");
        }
    }

    void LevelFinished(bool outOfTime = false)
    {
        if (!gameStarted)
            return;

        Debug.Log("NL" + nextLevel + " : " + currentLevel);
        if ((outOfTime || nextLevel) && currentLevel < _Levels.Length)
        {
            ClearLevel();
            // Hard coded to 3
            levels[currentLevel-1] = 3;
            if(!devMode)
                PlayFabManager.instance.SetLevelInfo(levels);

            levelCompleteUI.Show(outOfTime);
            Tile[] tiles = currentLevelGameObject.GetComponentsInChildren<Tile>();
            foreach (Tile t in tiles)
            {
                t.LevelComplete();
            }
        }

        if(currentLevel >= _Levels.Length)
        {
            ClearLevel();
            gameUI.AddScore();
            Debug.Log("Finished");
            gameOverUI.gameObject.SetActive(true);
        }
    }

    public void NextLevel()
    {
        gameUI.StartTimer();
        gameStarted = true;
        levelCompleteUI.gameObject.SetActive(false);
        fireball.nextEvent.RemoveAllListeners();
        currentLevel++;
        ClearLevel();
        CreateLevel();
        gameUI.AddScore();
    }

    void SwipeUp()
	{
        Debug.Log("Swipe up");
        Swipe(Vector3.up, 0.0f, -1.0f);
	}

    void SwipeDown()
	{
		Debug.Log("Swipe down");
        Swipe(Vector3.down, 0.0f, 1.0f);
    }

    void SwipeRight()
	{
		Debug.Log("Swipe right");
        Swipe(Vector3.right, -1.0f, 0.0f);
    }

    void SwipeLeft()
	{
		Debug.Log("Swipe left");
        Swipe(Vector3.left, 1.0f, 0.0f);
    }

    public void GotoHome()
    {
        introUI.SetActive(true);
        gameUI.gameObject.SetActive(false);
        levelCompleteUI.gameObject.SetActive(false);
        ClearLevel();
    }

    private void ClearLevel()
    {
        if (currentLevelGameObject)
        {
            Destroy(currentLevelGameObject);
        }
    }

    void CreateLevel()
    {
        time = Time.realtimeSinceStartup;
        Debug.Log("Creeating level");

        ClearLevel();

        currentLevelGameObject = new GameObject("CurrentLevel");
        LevelData levelData = _Levels[currentLevel - 1].GetComponent<LevelData>();

        foreach (LevelData.LevelInfo levelInfo in levelData._LevelInfo)
        {
            GameObject obj = (GameObject)Instantiate(levelInfo._Transform.gameObject);
            obj.name = levelInfo._Transform.name;
            Vector3 pos = obj.transform.position;
            pos.x = levelInfo._Pos.x;
            pos.y = levelInfo._Pos.y;
            pos.z = 0;
            obj.transform.position = pos;

            obj.transform.parent = currentLevelGameObject.transform;

            levelInfo.SetGameObject(obj);

            TileFireball fb = obj.GetComponent<TileFireball>();
            if (fb != null)
            {
                fireball = fb;
                fireball.nextEvent.AddListener(LevelFinished);
            }
        }

        // Set the custom data .. we need a for loop here to make sure all gameobjects have been initialized
        foreach (LevelData.LevelInfo levelInfo in levelData._LevelInfo)
        {
            Tile tile = levelInfo.GetTile();
            tile.SetCustomData(levelInfo._CustomData);
        }

        foreach (LevelData.LevelInfo levelInfo in levelData._LevelInfo)
        {
            Tile tile = levelInfo.GetTile();
            tile.UpdateCustomData();
        }
    }

    public void LoginOrRegister()
    {
        if(devMode)
        {
            StartGame();
            return;
        }

        if (registerSigninUI.canLogin())
        {
            registerSigninUI.Login();
        }
        else
        {
            introUI.SetActive(false);
            registerSigninUI.gameObject.SetActive(true);
        }
    }

    public void ShowLevel()
    {
        registerSigninUI.gameObject.SetActive(false);
        levelUI.gameObject.SetActive(true);
        particles.gameObject.SetActive(false);
        introUI.SetActive(false);
    }

    public void Play(int level)
    {
        gameStarted = true;
        registerSigninUI.gameObject.SetActive(false);
        currentLevel = level;
        CreateLevel();
        particles.gameObject.SetActive(false);
        gameUI.gameObject.SetActive(true);
        gameUI.StartTimer();
    }

    public void StartGame()
    {
        // Wait until you show the levels...
        if (devMode)
        {
            DisplayLevel(null, new int[20]);
        }
        else
        {
            PlayFabManager.instance.GetLevelsInfo(DisplayLevel);
        }
	}

    private void DisplayLevel(object obj, int [] lvls)
    {
        levelUI.Init(lvls);
        levels = lvls;
        ShowLevel();
    }
}
