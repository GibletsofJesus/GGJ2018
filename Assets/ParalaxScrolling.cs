using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxScrolling : MonoBehaviour {

	public Renderer background_far;
	public Renderer background_mid;
	public Renderer background_near;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		background_far.material.mainTextureOffset=Vector2.right*Player.instance.transform.position.x*0.02f;
		background_mid.material.mainTextureOffset=Vector2.right*Player.instance.transform.position.x*0.05f;
		background_near.material.mainTextureOffset=Vector2.right*Player.instance.transform.position.x*0.08f;
	}
}
