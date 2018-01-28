using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public static GameOver instance = null;
    public Text gameOver;
    public Text pressContinue;
    bool on = false;
    Color c;
    private void Awake()
    {
        if (instance == null)
        { instance = this; }
        else
            Destroy(this);
        c = pressContinue.color;
    }
    void Update()
    {
        if(on)
        {
            pressContinue.color = Color.Lerp(Color.white, Color.red,Mathf.Sin( Time.time*10));
        }
    }

    public void Died()
    {
        gameOver.gameObject.SetActive(true);
        pressContinue.gameObject.SetActive(true);
        gameOver.text = "YOU DIED";
        on = true;
        gameOver.color = Color.red;
    }

    public void LevelEnd()
    {
        gameOver.gameObject.SetActive(true);
        pressContinue.gameObject.SetActive(true);
        gameOver.text = "...TRANSMISSION COMMENCING...";
        on = true;
        gameOver.color = Color.white;
    }

    public void Deactivate()
    {
        gameOver.gameObject.SetActive(false);
        pressContinue.gameObject.SetActive(false);
    }
}
