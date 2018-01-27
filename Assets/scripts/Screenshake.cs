using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Screenshake : MonoBehaviour {

	public static Screenshake instance;
	public Vector3 camPosition;
	float duration=0;
	int amount;
	void Start()
	{
		instance=this;
	}
	public void Shake(int _amount, float dur)
	{
		amount=_amount;
		duration+=dur;
	}

	IEnumerator directionalshake()
	{
		yield return new WaitForEndOfFrame();
	}

	void Update()
	{
		if (duration >0)
		{
			int x=1+Random.Range(0,amount);
			int y=1+Random.Range(0,amount);
			if (Random.value>.5f)
				x*=-1;
			if (Random.value>.5f)
				y*=-1;
			Camera.main.transform.position=camPosition+new Vector3(x/8f,y/8f,0);
			duration-=Time.deltaTime;
		}
		else		
			Camera.main.transform.position=camPosition;
	}
}

[CustomEditor(typeof(Screenshake))]
public class ObjectBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        Screenshake myScript = (Screenshake)target;
        if(GUILayout.Button("shake"))
        {
          myScript.Shake(2,0.5f);
        }
    }
}