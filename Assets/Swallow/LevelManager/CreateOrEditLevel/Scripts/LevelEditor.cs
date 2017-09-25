using UnityEngine;
using UUEX.UI;
using System.Collections.Generic;

public class LevelEditor : MonoBehaviour 
{
	[System.Serializable]
	public class TileData
	{
		public string _TileName;
		public Transform _TileObject;
	}
	
	public UIMenu _DropDownMenu;
	public TileData[] _TileData;
	private Vector3 mClickPos = Vector3.zero;
	private int mCurrentFrame = -1;
	private Transform mLevelTransformParent = null;

	// Use this for initialization
	void Start () 
	{
		List<UIButton> itemList = new List<UIButton>();
		UIButton item = (UIButton)_DropDownMenu.GetItem("DupItem");
		itemList.Add(item);

		for(int i=0;i<9;i++)
		{
			UIButton newItem = (UIButton)item.Clone();
			_DropDownMenu.AddItem(newItem, item.transform.parent);
			itemList.Add(newItem);
		}

		for(int i=0;i<itemList.Count-1;i++)
		{
			itemList[i].SetText(_TileData[i]._TileName);
			itemList[i].SetName(_TileData[i]._TileName);

			itemList[i].SetItemData(_TileData[i]);
		}

		// Export
		itemList[itemList.Count - 1].SetName("Export");
		itemList[itemList.Count - 1].SetText("Export");

		_DropDownMenu.AddItemClickListener(ItemClicked);
		_DropDownMenu.SetVisibility(false);

		GameObject levelParent = GameObject.Find("LevelParent");

		if(levelParent == null)
			levelParent = new GameObject("LevelParent");

		mLevelTransformParent = levelParent.transform;
	}

	void ItemClicked(UIItem item)
	{
		_DropDownMenu.SetVisibility(false);

		if(item.GetName() != "Export")
		{
			TileData itemData = (TileData)item.GetItemData();

			GameObject newGameObject = (GameObject)GameObject.Instantiate(itemData._TileObject.gameObject);
			newGameObject.name = itemData._TileObject.name;
			newGameObject.transform.parent = mLevelTransformParent;

			Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mClickPos);
			worldPoint.z = 0;
			worldPoint.x = Mathf.Round(worldPoint.x);
			worldPoint.y = Mathf.Round(worldPoint.y);
			newGameObject.transform.position = worldPoint;
		}
		else
		{
			GameObject obj = new GameObject("NewData");
			LevelData levelData = obj.AddComponent<LevelData>();

			int childCount = mLevelTransformParent.childCount;
			levelData._LevelInfo = new LevelData.LevelInfo[childCount];

			for(int i=0;i<childCount;i++)
			{
				Transform child = mLevelTransformParent.GetChild(i);

				for(int j=0;j<_TileData.Length;j++)
				{
					if(child.name == _TileData[j]._TileObject.name)
					{
						levelData._LevelInfo[i] = new LevelData.LevelInfo();
						levelData._LevelInfo[i]._Transform = _TileData[j]._TileObject;
						levelData._LevelInfo[i]._Pos = new Vector2(child.position.x, child.position.y);
						break;
					}
				}
			}

		}

		mCurrentFrame = Time.frameCount;
	}

	// Update is called once per frame
	void Update () 
	{
		if(Time.frameCount != mCurrentFrame && Input.GetMouseButtonUp(0) && !_DropDownMenu.IsVisible())
		{
			mClickPos = Input.mousePosition;
			float y = Input.mousePosition.y - 200;
			if(y > 144)
				y = 144;
			float x = Input.mousePosition.x - 350;
			if(x > 428)
				x = 428;
			_DropDownMenu.SetPosition(x,y);
			_DropDownMenu.SetVisibility(true);
		}

		if(Input.GetMouseButtonUp(1))
			_DropDownMenu.SetVisibility(false);
	}
}
