using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UUEX.UI;

public class TileSpawn : Tile 
{
	private BoxCollider mCollider = null;
	private Transform mSpawnObject = null;
	private Renderer mRenderer = null;

	protected override void Start ()
	{
		base.Start ();

		Vector3 scale = transform.localScale;
		scale.x = 0.3f;
		scale.y = 0.3f;
		transform.localScale = scale;

		mCollider = GetComponent<BoxCollider> ();
		mCollider.isTrigger = true;
		mRenderer = GetComponent<Renderer>();
	}

	public override void UpdateCustomData ()
	{
		base.UpdateCustomData ();

		if(!string.IsNullOrEmpty(_CustomData))
		{
			mSpawnObject = transform.parent.Find(_CustomData);
			mSpawnObject.gameObject.SetActive(false);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.name.Contains("PfTileFireball"))
		{
			mAudioSource.Play();

			mRenderer.enabled = false;
			mSpawnObject.gameObject.SetActive(true);
		}
	}
}
