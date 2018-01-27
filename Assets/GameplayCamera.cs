using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 pos=Player.instance.transform.position;
		pos.z=-10;
		pos.y=0;
		pos.x+=2.5f;

		Screenshake.instance.camPosition=pos;
	}
}
