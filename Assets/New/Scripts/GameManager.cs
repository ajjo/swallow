using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject introUI;
    public Transform[] _Levels;
	public InputManager inputManager;

    private float time;
    private GameObject currentLevelGameObject;
	private int currentLevel = 1;

	private TileFireball fireball;
    private bool nextLevel;

    public GameObject registerSigninUI;
    public GameUI gameUI;
    public Transform particles;

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

    void NextLevel()
    {
        if (nextLevel)
        {
            fireball.nextEvent.RemoveAllListeners();
            currentLevel++;
            Destroy(currentLevelGameObject);
            CreateLevel();
            gameUI.AddScore();
        }
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
                fireball.nextEvent.AddListener(NextLevel);
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
        introUI.SetActive(false);
        registerSigninUI.SetActive(true);
    }

    public void StartGame()
    {
        registerSigninUI.SetActive(false);
        CreateLevel();
        particles.gameObject.SetActive(false);
        gameUI.gameObject.SetActive(true);
        gameUI.StartTimer();
    }
}
