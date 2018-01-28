using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour {

	public static Objective instance;
	bool triggered;
	public int level_index;
	public Renderer m_circletransition; 
	public Transform m_background;

	void OnTriggerEnter2D(Collider2D c)
	{
		PlayerPrefs.SetInt("Level"+level_index,1);//1 means complete!
		GameStateManager.instance.ChangeState(GameStateManager.GameStates.STATE_LEVELCOMPLETE);
		StartCoroutine(EndSequence());
        GameOver.instance.LevelEnd();
	}
	void Start()
	{
		instance=this;
		StartCoroutine(CircleTransition(true));
	}
	void Update()
	{
		if (GameStateManager.instance.m_state==GameStateManager.GameStates.STATE_GAMEPLAY)
		{
			Vector3 pos=Camera.main.transform.position;
			pos.z=0;
			m_background.transform.position=pos;
		}
		else if (GameStateManager.instance.m_state==GameStateManager.GameStates.STATE_GAMEPLAY)
		{
			//if jump button
			//Exit to overworld
			//StartCoroutine(CircleTransition(false));
		}
	}

	public void RestartLevel()
	{
		StartCoroutine(CircleTransition(false));
		StartCoroutine(restart());
	}

	IEnumerator restart()
	{
		yield return new WaitForSeconds(1.05f);
		UnityEngine.SceneManagement.SceneManager.LoadScene("Level "+level_index);
	}

	IEnumerator CircleTransition(bool inout)
	{
		if (inout) yield return new WaitForSeconds(.25f);
		float lerpy=0;
		while (lerpy<1)
		{
			lerpy+=Time.deltaTime;
			Vector3 pos = Player.instance.transform.position;
			pos.z=-0.1f;
			m_circletransition.transform.position=pos;
			m_circletransition.material.SetFloat("_SliceAmount",inout ? lerpy : 1-lerpy);
       		yield return new WaitForEndOfFrame();
		}
	}

	//Zoom in on player, maybe change sprite to a thumbs up ting
	IEnumerator EndSequence()
	{
		Player.instance.enabled=false;
		Player.instance.body.simulated=false;
		float lerpy=0;
		Player.instance.spRenderer.sprite=Player.instance.m_sprites[8];
		while (lerpy<1)
		{
			lerpy+=Time.deltaTime;
			Vector3 pos=Vector3.Lerp(Camera.main.transform.position,Player.instance.transform.position,lerpy);
			pos.z=-10;
			Camera.main.transform.position=pos;
			Camera.main.orthographicSize=Mathf.Lerp(5,1.5f,lerpy);
        	yield return new WaitForEndOfFrame();
		}
			while ((Controller.ButtonDown() & ControllerButton.CROSS) == 0)
			{
				yield return null;
			}
        GameOver.instance.Deactivate();
			StartCoroutine(CircleTransition(false));
			level_index++;
			StartCoroutine(restart());
	}

}
