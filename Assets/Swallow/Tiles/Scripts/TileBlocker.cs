using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UUEX.UI;

public class TileBlocker : Tile 
{
	public override void SetCustomData (string customData)
	{
		base.SetCustomData (customData);

		if(!string.IsNullOrEmpty(customData))
			transform.name = customData;
	}

	protected override void OnCollisionEnter (Collision collision)
	{
		base.OnCollisionEnter (collision);

		if(collision.gameObject.name.Equals("PfTileFireball"))
		{
			Animator mAnimator = transform.GetComponent<Animator>();
			mAnimator.SetInteger("animstate",1);

			mAudioSource.Play();
		}
	}
}
