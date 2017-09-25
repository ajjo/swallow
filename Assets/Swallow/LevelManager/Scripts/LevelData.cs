using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class LevelData : MonoBehaviour 
{
	[System.Serializable]
	public class LevelInfo
	{
		public Vector2 _Pos;
		public Transform _Transform;
		public string _CustomData;

		private Tile mTile;
		private GameObject mGameObject = null;

		public void SetGameObject(GameObject obj)
		{
			mGameObject = obj;
			mTile = obj.GetComponent<Tile>();
		}

		public GameObject GetGameObject()
		{
			return mGameObject;
		}

		public Tile GetTile()
		{
			return mTile;
		}
	}

	public LevelInfo [] _LevelInfo;

	public void Start()
	{
		if(Application.isPlaying)
			return;

		if(GameObject.Find("LevelParent") != null)
			return;

		GameObject obj = new GameObject("LevelParent");

		foreach(LevelInfo levelInfo in _LevelInfo)
		{
			GameObject newGameObject = (GameObject)GameObject.Instantiate(levelInfo._Transform.gameObject);
			newGameObject.name = levelInfo._Transform.name;
			newGameObject.transform.parent = obj.transform;
			
			Vector3 worldPoint = new Vector3(levelInfo._Pos.x, levelInfo._Pos.y, 0);
			newGameObject.transform.position = worldPoint;
		}
	}
}
