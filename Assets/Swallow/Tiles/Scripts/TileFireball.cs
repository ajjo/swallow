using UnityEngine;
using System.Collections;

public class TileFireball : Tile 
{
	void Update()
	{
		Vector3 position = transform.position;
		Vector3 screenPosition = Camera.main.WorldToScreenPoint(position);

		if(screenPosition.x < 0 || screenPosition.y < 0 || screenPosition.x > Screen.width || screenPosition.y > Screen.height)
			LevelManager.pInstance.ResetGame();
	}

	protected override void OnCollisionEnter (Collision collision)
	{
		base.OnCollisionEnter (collision);

		if(collision.gameObject.name.Contains("PfTileBlockerOscillator"))
		{
			transform.parent = collision.gameObject.transform;

			mAudioSource.Play();
		}
	}
}
