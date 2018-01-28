using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshake : MonoBehaviour {

	public static Screenshake instance;
	public Vector3 camPosition;
	float duration=0;
	float amount;
	void Start()
	{
		instance=this;
	}
	public void Shake(float _amount, float dur)
	{
		amount=_amount;
		if (duration < dur)
			duration=dur;
		else
			duration+=dur;
	}

	IEnumerator directionalshake()
	{
		yield return new WaitForEndOfFrame();
	}

	void Update()
	{
		if (GameStateManager.instance.m_state == GameStateManager.GameStates.STATE_GAMEPLAY)
		{
		if (duration >0)
		{
			float x=.25f+Random.Range(0,amount);
			float y=.25f+Random.Range(0,amount);
			if (Random.value>.5f)
				x*=-1;
			if (Random.value>.5f)
				y*=-1;
			Camera.main.transform.position=camPosition+new Vector3(x/8f,y/8f,0);
			duration-=Time.deltaTime;
		}
		else
			Camera.main.transform.position=camPosition;
		}
	}
}