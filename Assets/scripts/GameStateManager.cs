using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour {

	public static GameStateManager instance;
	public GameStates m_state;
	public enum GameStates
	{
		STATE_GAMEPLAY = 0,
		STATE_PAUSE,
		STATE_LEVELCOMPLETE,
		STATE_OVERWORLD,
		STATE_DEATH,
		STATE_SPLASH
	}

	// Use this for initialization
	void Start ()
	{
		instance=this;	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ChangeState(GameStates gs)
	{
		m_state=gs;
	}
}
