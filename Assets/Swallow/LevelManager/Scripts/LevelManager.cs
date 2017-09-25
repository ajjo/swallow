using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TileType
{
	BLOCKER,
	HOLE,
	FIRE_BALL,
	BLOCKER_OSCILLATOR,
	BOMB,
	GLASS,
	SPAWN,
	TELEPORT,
	TOGGLE
}

public enum TileMoveDirection
{
	UP,
	RIGHT,
	DOWN,
	LEFT
}

public class LevelManager : MonoBehaviour 
{
	public Transform [] _Levels = null;
	public UIGame _GameUI = null;
	public int _LevelToPlay = -1;

	public AudioClip [] _AudioClips = null;

	private TileMoveDirection mMoveDirection;
	private int mCurrentLevel = 1;
	private Tile mFireBall = null;
	private GameObject mCurrentLevelObject = null;
	private float mTime = 0.0f;
	private AudioSource mAudioSource = null;

	static LevelManager mInstance;
	public static LevelManager pInstance
	{
		get { return mInstance; }
	}

	public List<Tile> GetTileList()
	{
		return null;
	}

	public Tile GetTile(int x, int y)
	{
		return null;
	}

	public void Start()
	{
		mInstance = this;
		mAudioSource = GetComponent<AudioSource>();
	}

	public void BeginGame()
	{
		_GameUI.SetVisibility(true);
		_GameUI.SetScore(100);

		if(_LevelToPlay == -1)
			mCurrentLevel = 1;
		else
			mCurrentLevel = _LevelToPlay;
		
		CreateLevel ();
	}

	public void ResetGame()
	{
		GameObject.Destroy(mCurrentLevelObject);
		GameObject.Destroy(mFireBall.gameObject);
		CreateLevel();
	}

	void Update () 
	{
		float timeElapsed = Time.realtimeSinceStartup - mTime;

		if(_GameUI.IsVisible())
		{
			_GameUI.SetTime(timeElapsed);

			if(timeElapsed <= 100)
				_GameUI.SetScore(100 - Mathf.CeilToInt(timeElapsed));
			else
				_GameUI.SetScore(0);
		}

		if(mFireBall != null)
		{
			if (mFireBall.IsStationary() && Input.GetKeyUp (KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
			{
				mMoveDirection = TileMoveDirection.UP;
				mFireBall.AddForce (TileMoveDirection.UP);
			}
			else if (mFireBall.IsStationary() && Input.GetKeyUp (KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
			{
				mMoveDirection = TileMoveDirection.LEFT;
				mFireBall.AddForce (TileMoveDirection.LEFT);
			}
			else if (mFireBall.IsStationary() && Input.GetKeyUp (KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
			{
				mMoveDirection = TileMoveDirection.RIGHT;
				mFireBall.AddForce (TileMoveDirection.RIGHT);
			}
			else if (mFireBall.IsStationary() && Input.GetKeyUp (KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
			{
				mMoveDirection = TileMoveDirection.DOWN;
				mFireBall.AddForce (TileMoveDirection.DOWN);
			}	
		}
	}

	void CreateLevel()
	{
		_GameUI.SetLevel(mCurrentLevel);
		mTime = Time.realtimeSinceStartup;

		mCurrentLevelObject = new GameObject("CurrentLevel");
		LevelData levelData = _Levels[mCurrentLevel - 1].GetComponent<LevelData>();

		foreach(LevelData.LevelInfo levelInfo in levelData._LevelInfo)
		{
			GameObject obj = (GameObject)GameObject.Instantiate(levelInfo._Transform.gameObject);
			obj.name = levelInfo._Transform.name;
			Vector3 pos = obj.transform.position;
			pos.x = levelInfo._Pos.x;
			pos.y = levelInfo._Pos.y;
			pos.z = 0;
			obj.transform.position = pos;

			obj.transform.parent = mCurrentLevelObject.transform;

			levelInfo.SetGameObject(obj);
		}

		// Set the custom data .. we need a for loop here to make sure all gameobjects have been initialized
		foreach(LevelData.LevelInfo levelInfo in levelData._LevelInfo)
		{
			Tile tile = levelInfo.GetTile();
			tile.SetCustomData(levelInfo._CustomData);
		}

		foreach(LevelData.LevelInfo levelInfo in levelData._LevelInfo)
		{
			Tile tile = levelInfo.GetTile();
			tile.UpdateCustomData();
		}

		Tile [] tiles = mCurrentLevelObject.GetComponentsInChildren<Tile>();
		foreach(Tile tile in tiles)
		{
			if(tile._Type == TileType.FIRE_BALL)
			{
				mFireBall = tile;
				break;
			}
		}
	}

	public void NextLevel()
	{
		mCurrentLevel++;
		if (mCurrentLevel > _Levels.Length)
			mCurrentLevel = 1;

		GameObject.Destroy(mCurrentLevelObject);
		GameObject.Destroy(mFireBall.gameObject);
		CreateLevel();

		mAudioSource.clip = _AudioClips[2];
		mAudioSource.Play();
	}

	public TileMoveDirection GetMoveDirection()
	{
		return mMoveDirection;
	}
}
