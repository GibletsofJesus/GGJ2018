using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DeathCounter : MonoBehaviour
{
    int deathcount = 0;
    public Text counter;
    string[] rating = new string[4] {"Awesome!", "OK", "Amateur", "YOU SUCK"};
    private void Awake()
    {
        deathcount = PlayerPrefs.GetInt("deaths");
    }
    // Use this for initialization
    void Start ()
    {
        int temp = deathcount * PlayerPrefs.GetInt("Level");
        int ratingNum = (temp< 5? 0: temp<20? 1: temp <30? 2 : 3);
        counter.text = "Death Count : " + deathcount + "\nRating : " + rating[ratingNum];	
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
