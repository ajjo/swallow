using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UUEX.UI;

public class TileToggle : Tile 
{
	private Tile mToggleTile = null;

	public override void UpdateCustomData ()
	{
		base.UpdateCustomData ();
		
		GameObject toggleTileObject = GameObject.Find(_CustomData);
		if(toggleTileObject != null)
		{
			mToggleTile = toggleTileObject.GetComponent<Tile>();
			mToggleTile.gameObject.SetActive(false);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(mToggleTile != null && other.gameObject.name.Contains("PfTileFireball"))
		{
			if(mToggleTile.gameObject.activeSelf)
				mToggleTile.gameObject.SetActive(false);
			else
				mToggleTile.gameObject.SetActive(true);

			mAudioSource.Play();
		}
	}
}
