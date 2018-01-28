using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour {

	public ParticleSystem m_ps;
	bool dead;
	void OnCollisionEnter2D(Collision2D c)
	{
		if (c.gameObject.tag=="Player")
		{
			dead=true;
			m_ps.transform.position=Player.instance.transform.position;
			Player.instance.gameObject.SetActive(false);
			m_ps.gameObject.SetActive(true);
			Player.instance.DeathSfx();
		}
	}

	void Update()
	{
		if (dead)
		{
			if ((Controller.ButtonDown() & ControllerButton.CROSS) != 0)
			{
				dead=false;
				Objective.instance.RestartLevel();
			}
		}
	}
}
