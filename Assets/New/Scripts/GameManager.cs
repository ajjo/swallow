using System.Collections;
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
	}

    // Update is called once per frame
    void Update()
    {
        
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

    void LevelFinished()
    {
        Debug.Log("NL");
        if (nextLevel && currentLevel < _Levels.Length)
        {
            // Hard coded to 3
            levels[currentLevel-1] = 3;
            PlayFabManager.instance.SetLevelInfo(levels);

            levelCompleteUI.gameObject.SetActive(true);
        }

        if(currentLevel >= _Levels.Length)
        {
            Destroy(currentLevelGameObject);
            gameUI.AddScore();
            Debug.Log("Finished");
            gameOverUI.gameObject.SetActive(true);
        }
    }

    void NextLevel()
    {
        fireball.nextEvent.RemoveAllListeners();
        currentLevel++;
        Destroy(currentLevelGameObject);
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

    void CreateLevel()
    {
        time = Time.realtimeSinceStartup;

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

    public void Play()
    {
        gameStarted = true;
        registerSigninUI.gameObject.SetActive(false);
        CreateLevel();
        particles.gameObject.SetActive(false);
        gameUI.gameObject.SetActive(true);
        gameUI.StartTimer();
    }

    public void StartGame()
    {
        // Wait until you show the levels...
        PlayFabManager.instance.GetLevelsInfo(DisplayLevel);
	}

    private void DisplayLevel(object obj, int [] levels)
    {
        levelUI.Init(levels);
        ShowLevel();
    }
}
