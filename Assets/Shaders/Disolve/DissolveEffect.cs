using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveEffect : MonoBehaviour {

    [SerializeField]
    Material Dissolve;
    public float speed;
    public float max;
    private float currentY = 0;
    private float startY;
    private float startTime;
    private Bounds bounds;
    // Use this for initialization
    void Awake ()
    {
        bounds = GetComponent<Collider>().bounds;
        max = transform.position.y + bounds.max.y;
        Dissolve = GetComponent<MeshRenderer>().material;
        startY =  transform.position.y-bounds.min.y;
        currentY = startY;
        Dissolve.SetFloat("_StartY", startY);
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(currentY<max)
        {
            Dissolve.SetFloat("_DissolveY", currentY);
            currentY += Time.deltaTime * speed;
        }
	}

   void ResetValues()
    {
        max = transform.position.y + bounds.max.y;
        startY = transform.position.y - bounds.min.y;
        currentY = startY;
    }
}
