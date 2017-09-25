using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UUEX.UI;

public class TileOscillator : Tile 
{
	private int mDeltaX = 0;
	private int mDeltaY = 0;
	private float mDuration = 1.0f;

	private float mStartX = 0;
	private float mEndX = 0;

	private float mStartY = 0;
	private float mEndY = 0;

	private float mTime = 0;
	private bool mReturn = false;
	private bool mDataSet = false;
	
	public override void SetCustomData (string customData)
	{
		base.SetCustomData (customData);

		string [] data = _CustomData.Split(',');
		mDeltaX = int.Parse(data[0]);
		mDeltaY = int.Parse(data[1]);
		mDuration = float.Parse(data[2]);
		
		Vector3 pos = transform.position;
		
		if (mDeltaX != 0)
		{
			mStartX = pos.x - mDeltaX;
			mEndX = pos.x + mDeltaX;
		}
		else
		{
			mStartX = mEndX = pos.x;
		}
		
		if(mDeltaY != 0)
		{
			mStartY = pos.y + mDeltaY;
			mEndY = pos.y - mDeltaY;
		}
		else
		{
			mStartY = mEndY = pos.y;
		}
		
		mDataSet = true;
	}

	void Update()
	{
		if(!mDataSet)
			return;

		float x = Mathf.Lerp (mStartX, mEndX, (mReturn==true) ? (1 - (mTime/mDuration)) : (mTime / mDuration));
		float y = Mathf.Lerp (mStartY, mEndY, (mReturn == true) ? (1 - (mTime / mDuration)) : (mTime / mDuration));

		Vector3 pos = transform.position;
		pos.x = x;
		pos.y = y;
		transform.position = pos;

		if (mTime == mDuration)
		{
			mReturn = !mReturn;
			mTime = 0;
		}

		mTime += Time.deltaTime;
		if (mTime > mDuration)
			mTime = mDuration;
	}
}
