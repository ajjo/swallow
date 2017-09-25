using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UUEX.UI;

public class TileTeleport : Tile 
{
	private Tile mTeleportToTile = null;
	private string mSearchForTeleport = "";

	protected override void Start ()
	{
		base.Start ();
	}

	public override void SetCustomData (string customData)
	{
		base.SetCustomData (customData);

		if(!string.IsNullOrEmpty(customData))
		{
			string[] names = customData.Split(',');
			transform.name = names[0];
			mSearchForTeleport = names[1];
		}
	}

	public override void UpdateCustomData ()
	{
		base.UpdateCustomData ();
		
		if(!string.IsNullOrEmpty(_CustomData))
		{
			Transform teleportObject = transform.parent.FindChild(mSearchForTeleport);
			mTeleportToTile = teleportObject.GetComponent<Tile>();
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(mTeleportToTile != null && other.gameObject.name.Contains("PfTileFireball"))
		{
			TileMoveDirection tileMoveDirection = LevelManager.pInstance.GetMoveDirection();

			Rigidbody fireRigidbody = other.gameObject.GetComponent<Rigidbody>();

			fireRigidbody.velocity = Vector3.zero;

			Vector3 otherPos = other.gameObject.transform.position;

			if(tileMoveDirection == TileMoveDirection.DOWN)
			{
				otherPos.y = mTeleportToTile.gameObject.transform.position.y - 1.00001f;
				otherPos.x = mTeleportToTile.gameObject.transform.position.x;
			}
			else if(tileMoveDirection == TileMoveDirection.RIGHT)
			{
				otherPos.x = mTeleportToTile.gameObject.transform.position.x + 1.00001f;
				otherPos.y = mTeleportToTile.gameObject.transform.position.y;
			}
			else if(tileMoveDirection == TileMoveDirection.LEFT)
			{
				otherPos.x = mTeleportToTile.gameObject.transform.position.x - 1.00001f;
				otherPos.y = mTeleportToTile.gameObject.transform.position.y;
			}
			else
			{
				otherPos.y = mTeleportToTile.gameObject.transform.position.y + 1.00001f;
				otherPos.x = mTeleportToTile.gameObject.transform.position.x;
			}

			other.gameObject.transform.position = otherPos;

			mAudioSource.Play();
		}
	}
}
