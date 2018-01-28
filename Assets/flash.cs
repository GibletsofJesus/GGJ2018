using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flash : MonoBehaviour {

	public UnityEngine.UI.Text txt;


	// Use this for initialization
	void Start () {
		
	}
	int f;
	// Update is called once per frame
	void Update () {
		f++;
		if (f%20==0)
		txt.enabled=!txt.enabled;

		if (Controller.GetButton(ControllerButton.SQUARE))
			UnityEngine.SceneManagement.SceneManager.LoadScene("Level 0");

	}
}
