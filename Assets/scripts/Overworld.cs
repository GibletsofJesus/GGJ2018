using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overworld : MonoBehaviour {

	public static Overworld instance;
	public Renderer m_rend;
	Texture2D m_overlay;
	public Level[] m_levels;

	[System.Serializable]
	public class Level{
		public float r=8;
		public GameObject m_go;
		public bool unlocked;
	}
	void Start ()
	{
		m_overlay=new Texture2D(256,256);
		for (int y=0;y<m_overlay.height;y++)
		{
			for (int x=0;x<m_overlay.width;x++)
			{
				m_overlay.SetPixel(x,y,Color.white);
			}
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		foreach (Level l in m_levels)
		{
			Vector3 pos = Camera.main.WorldToViewportPoint(l.m_go.transform.position);
			pos*=m_overlay.width;
			if (l.unlocked)
			{
				Circle(m_overlay,(int)pos.x,(int)pos.y,(int)l.r,Color.black);
			}
		}
		
		m_rend.material.SetTexture("_SliceGuide",m_overlay);
		m_rend.material.SetFloat("_SliceAmount", 0.5f+(Mathf.Sin(Time.time)/2));
	}

    public IEnumerator RevealArea(int i)
    {
		m_levels[i].unlocked=true;
		while (m_levels[i].r < 32)
		{
			m_levels[i].r+=Time.deltaTime*2;
        	yield return new WaitForEndOfFrame();
		}
	}

  	public void Circle(Texture2D tex, int cx, int cy, int r, Color col)
     {
        int x, y, px, nx, py, ny, d;
    	Color32[] tempArray = tex.GetPixels32();
 
         for (x = 0; x <= r; x++)
         {
             d = (int)Mathf.Ceil(Mathf.Sqrt(r * r - x * x));
             for (y = 0; y <= d; y++)
             {
                 px = cx + x;
                 nx = cx - x;
                 py = cy + y;
                 ny = cy - y;
                 tempArray[Mathf.Clamp(py*tex.width + px,0,tempArray.Length-1)] = col;
                 tempArray[Mathf.Clamp(py*tex.width + nx,0,tempArray.Length-1)] = col;
                 tempArray[Mathf.Clamp(ny*tex.width + px,0,tempArray.Length-1)] = col;
                 tempArray[Mathf.Clamp(ny*tex.width + nx,0,tempArray.Length-1)] = col;
             }
         }    
         tex.SetPixels32(tempArray);
         tex.Apply ();
	 }
}
