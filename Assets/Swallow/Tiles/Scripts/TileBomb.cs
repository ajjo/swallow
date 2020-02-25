using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UUEX.UI;

public class TileBomb : Tile 
{
	public int _SelfDestructDuration = 5;
	public AudioClip _Tick = null;
    public TMPro.TextMeshPro textMeshPro;

	private float mTime = 0;
	private Renderer mRenderer = null;
	private static int mBombCount = 0;
	private static AudioSource mTickingAudioSource = null;
	
	protected override void Start ()
	{
		base.Start ();

		if(mBombCount == 0)
		{
			GameObject obj = new GameObject();
			obj.name = "TickingAudioSource";
			mTickingAudioSource = obj.AddComponent<AudioSource>();

			mTickingAudioSource.playOnAwake = false;
			mTickingAudioSource.clip = _Tick;
			mTickingAudioSource.loop = true;
			mTickingAudioSource.Play();
		}

		mRenderer = GetComponent<Renderer>();
		mBombCount++;
	}

    public override void LevelComplete()
    {
        base.LevelComplete();

        mTickingAudioSource.Stop();
        Debug.Log("Finished Bom " + isDone);
    }

    public override void UpdateCustomData ()
	{
		base.UpdateCustomData ();
		
		if(!string.IsNullOrEmpty(_CustomData))
		{
			if(string.IsNullOrEmpty(_CustomData))
				_SelfDestructDuration = 5;
			else
				_SelfDestructDuration = int.Parse(_CustomData);

			mTime = Time.realtimeSinceStartup;
		}
	}

	void Update()
	{
		if(Application.loadedLevelName == "LevelEditor" || !mRenderer.enabled || isDone)
			return;

		float elapsedTime = (Time.realtimeSinceStartup - mTime);

		if(elapsedTime >= _SelfDestructDuration)
		{
			//gameObject.SetActive(false);
			mRenderer.enabled = false;
            textMeshPro.enabled = false;
			mAudioSource.Play();
			mBombCount--;

			if(mBombCount == 0)
				GameObject.Destroy(mTickingAudioSource.gameObject);
		} else
        {
            float remaining = _SelfDestructDuration - elapsedTime;
            textMeshPro.SetText(remaining.ToString("0"));
        }
	}

	void OnDisable()
	{
		if(mTickingAudioSource != null)
		{
			mBombCount = 0;

			GameObject.Destroy(mTickingAudioSource.gameObject);
			mTickingAudioSource = null;
		}
	}
}
