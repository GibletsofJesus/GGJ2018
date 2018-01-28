using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bun : MonoBehaviour, IGetHit {

	public Sprite[] m_sprites;
	public SpriteRenderer m_spr;
	public Collider2D m_col;	
	int frame=0;
	public bool left;
	public int speed=5,moveSpeed;
	public ParticleSystem m_ps;
	bool dead;
	void OnCollisionEnter2D(Collision2D c)
	{
		if (c.gameObject.tag=="Player")
		{
			
			if (!Player.instance.slamming)
			{
			dead=true;
			Player.instance.gameObject.SetActive(false);
			m_ps.gameObject.SetActive(true);
			Player.instance.DeathSfx();
			}
			else
			{
		Player.instance.m_poof.Emit(8);
        gameObject.SetActive(false);
			}
		}
	}
	public void GotHit()
    {
		Player.instance.m_poof.Emit(8);
        gameObject.SetActive(false);
        //some particle of what
    }
	// Update is called once per frame
	void Update () {
		if (dead)
		{
			if ((Controller.ButtonDown() & ControllerButton.CROSS) != 0)
			{
				dead=false;
				Objective.instance.RestartLevel();
			}
		}
		m_spr.flipX=left;
		Vector3 pos=transform.position+(left ? Vector3.left*0.45f:Vector3.right*0.45f);
		Debug.DrawLine(pos,pos+(left?Vector3.left*.1f:Vector3.right*.1f),Color.red,0.1f);
		if (Physics2D.Raycast(pos,left?Vector3.left:Vector3.right,0.1f))
		{
			left=!left;
		}
		transform.position+=left? Vector3.left*Time.deltaTime*moveSpeed:Vector3.right*Time.deltaTime*moveSpeed;        

		frame++;
		if (frame>(m_sprites.Length-1)*speed)
		{
			frame=0;
		}
		m_spr.sprite=m_sprites[Mathf.RoundToInt(frame/speed)];
	}
}

public interface IGetHit
{
   void GotHit();
}