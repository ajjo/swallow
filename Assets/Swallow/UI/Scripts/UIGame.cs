using UnityEngine;
using UUEX.UI;
using System.Collections;

public class UIGame : UI 
{
	private UIText mLevel = null;
	private UIText mTime = null;
	private UIText mScore = null;

	protected void Start()
	{
		mLevel = GetItem<UIText>("LblLevelNumberValue");
		mTime = GetItem<UIText>("LblLevelTimeValue");
		mScore = GetItem<UIText>("LblLevelScoreValue");
	}

	public void SetLevel(int level)
	{
		mLevel.SetText(level.ToString());
	}

	public void SetTime(float time)
	{
		mTime.SetText(time.ToString("0"));
	}

	public void SetScore(int score)
	{
		mScore.SetText(score.ToString());
	}
}
