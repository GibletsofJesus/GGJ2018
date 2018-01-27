using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fuckyou : MonoBehaviour {

	public Transform spikeWall;
	public Vector3 start,end;
	bool once;
	void OnTriggerEnter2D(Collider2D c)
	{
		if (c.tag=="Player" && !once)
		{
			once=true;
			StartCoroutine(crush());
		}
	}
	

	IEnumerator crush()
	{
		float lerpy=0;
		while (lerpy<1)
		{
			lerpy+=Time.deltaTime*1.5f;
			spikeWall.transform.position =Vector3.Lerp(start,end,lerpy);
       		yield return new WaitForEndOfFrame();
		}
	}

}
