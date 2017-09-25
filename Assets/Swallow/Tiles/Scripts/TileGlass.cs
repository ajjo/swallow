using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UUEX.UI;

public class TileGlass : Tile 
{
	private Renderer mRenderer = null;

	protected override void Start ()
	{
		base.Start ();

		mRenderer = GetComponent<Renderer>();
	}

	protected override void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.name.Contains("PfTileFireball"))
		{
			Rigidbody rigidbody = collision.gameObject.GetComponent<Rigidbody>();
			rigidbody.velocity = Vector3.zero;
			rigidbody.angularVelocity = Vector3.zero;

			//gameObject.SetActive(false);
			mRenderer.enabled = false;

			mAudioSource.Play();
		}
	}
}
