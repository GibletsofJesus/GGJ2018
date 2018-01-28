using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour , IGetHit{

	public ParticleSystem m_ps;
	public Collider2D m_col;
	public SpriteRenderer m_spr;	

	void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.tag=="Player")
		{
			Debug.Log(Player.instance.slamming);
			if (Player.instance.slamming)
			{
                //break;
                //Play a sound
                Screenshake.instance.Shake(1, 0.15f);
                m_spr.enabled = false;
                m_col.enabled = false;
                m_ps.gameObject.SetActive(true);
            }
		}
	}

    public void GotHit()
    {
        Screenshake.instance.Shake(1, 0.15f);
        m_spr.enabled = false;
        m_col.enabled = false;
        m_ps.gameObject.SetActive(true);
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
