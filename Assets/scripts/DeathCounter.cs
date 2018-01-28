using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DeathCounter : MonoBehaviour
{
    int deathcount = 0;
    public Text counter;
    private void Awake()
    {
        deathcount = PlayerPrefs.GetInt("deaths");
    }
    private void OnEnable()
    {
       Player.instance.Dead += UpdateDeaths;
    }
 
    // Use this for initialization

    void Start ()
    {
        counter.text = "Death Count : " + deathcount;// + "\nRating : " + rating[ratingNum];	
	}
	
    void UpdateDeaths()
    {
        deathcount++;
        PlayerPrefs.SetInt("deaths", deathcount);
    }
	// Update is called once per frame
	void Update () {
		
	}
}
