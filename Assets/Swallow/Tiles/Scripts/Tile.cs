using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UUEX.UI;

public class Tile : MonoBehaviour 
{
	protected Rigidbody mRigidbody = null;
	protected TextMesh mText = null;
	protected AudioSource mAudioSource = null;

    protected bool isDone = false;

	[System.NonSerialized]
	public int _X;
	[System.NonSerialized]
	public int _Y;
	[System.NonSerialized]
	public string _CustomData = "";
	public TileType _Type;

	protected virtual void Start()
	{
		mRigidbody = GetComponent<Rigidbody> ();
		mText = GetComponentInChildren<TextMesh> ();

		mAudioSource = GetComponent<AudioSource>();
	}

	public virtual void SetCustomData(string customData)
	{
		_CustomData = customData;
	}

	public virtual void UpdateCustomData()
	{
	}

    public virtual void LevelComplete()
    {
        isDone = true;
    }

	public bool IsStationary()
	{
		if(mRigidbody != null)
		{
			if(mRigidbody.velocity.sqrMagnitude < 3.0f)
				return true;
		}

		return false;
	}

	public void AddForce(TileMoveDirection moveDir)
	{
		transform.parent = null;

		if (moveDir == TileMoveDirection.UP)
			mRigidbody.AddForce (transform.up * 3.0f);
		else if (moveDir == TileMoveDirection.RIGHT)
			mRigidbody.AddForce (transform.right * 3.0f);
		else if (moveDir == TileMoveDirection.LEFT)
			mRigidbody.AddForce (-transform.right * 3.0f);
		else if (moveDir == TileMoveDirection.DOWN)
			mRigidbody.AddForce (-transform.up * 3.0f);
	}

	protected virtual void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.name.Contains("PfTileHole"))
		{
			LevelManager.pInstance.NextLevel();
		}
	}
}
