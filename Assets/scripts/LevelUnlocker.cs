using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUnlocker : MonoBehaviour {

	public static LevelUnlocker instance;
	
	// Use this for initialization
	void Start ()
	{
		instance=this;
		if (!PlayerPrefs.HasKey("PreviousPlay"))
		{
			for (int i=0;i<Overworld.instance.m_levels.Length;i++)
			{
				if (!Overworld.instance.m_levels[i].unlocked && PlayerPrefs.GetInt("Level"+i)>0)
				{
					StartCoroutine(Overworld.instance.RevealArea(i));
				}
			}
		}
		else
		{
			for (int i=0;i<Overworld.instance.m_levels.Length;i++)
			{
				PlayerPrefs.SetInt("Level"+i,0);
			}
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
