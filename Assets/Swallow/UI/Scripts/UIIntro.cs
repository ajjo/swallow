using UnityEngine;
using UUEX.UI;
using UUEX.UI.Effects;
using System.Collections;

public class UIIntro : UI 
{
	protected void Start()
	{
		AddItemClickListener(ItemClicked);

		UIText introText = GetItem<UIText>("LblIntro");
		LerpColor lerpColor = introText.GetComponent<LerpColor>();
		lerpColor.PlayIn();
	}

	void ItemClicked(UIItem item)
	{
		if(item.GetName() == "BtnStartGame")
		{
			SetVisibility(false);
			LevelManager.pInstance.BeginGame();
		}
	}
}
