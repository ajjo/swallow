using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class TileFireball : Tile 
{
    Vector3 startPos;
    Vector3 lerpToPos;
    float t;
    bool animate = false;
    public float lerpDuration = 2.0f;
    public UnityEvent nextEvent;

    public void MoveTo(Vector3 pos)
    {
        t = 0.0f;
        startPos = transform.position;
        lerpToPos = pos;
        animate = true;
        Debug.Log("Moving frrrom " + startPos.ToString() + " : " + lerpToPos.ToString());       
    }

	void Update()
	{
        if(animate)
        {
            t += Time.deltaTime;
            if(t > lerpDuration)
            {
                t = lerpDuration;
                animate = false;

                nextEvent.Invoke();
            }
            Vector3 p = Vector3.Lerp(startPos, lerpToPos, t / lerpDuration);
            transform.position = p;
        }


		//Vector3 position = transform.position;
		//Vector3 screenPosition = Camera.main.WorldToScreenPoint(position);

		//if(screenPosition.x < 0 || screenPosition.y < 0 || screenPosition.x > Screen.width || screenPosition.y > Screen.height)
		//	LevelManager.pInstance.ResetGame();
	}

	//protected override void OnCollisionEnter (Collision collision)
	//{
	//	base.OnCollisionEnter (collision);

	//	if(collision.gameObject.name.Contains("PfTileBlockerOscillator"))
	//	{
	//		transform.parent = collision.gameObject.transform;

	//		mAudioSource.Play();
	//	}
	//}
}
