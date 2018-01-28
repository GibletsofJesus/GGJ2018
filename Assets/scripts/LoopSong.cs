using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LoopSong : MonoBehaviour {

    public  AudioClip clip
        ;
    AudioSource source;
    public static LoopSong instance = null;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        source.clip = clip;
        source.loop = true;
        source.Play();
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
