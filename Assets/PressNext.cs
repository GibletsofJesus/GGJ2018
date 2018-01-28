using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PressNext : MonoBehaviour
{
    public Text text;
    public static PressNext instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(this);
    }

    public void Died()
    {
        text.gameObject.SetActive(true);
        text.text = "You Died /n Press Cross to continue";
    }
      public void DoneLevel()
    {
        text.gameObject.SetActive(true);
        text.text = "Transmission valid /n Press Cross to continue";
    }

    private void Update()
    {
        if((Controller.Button()&ControllerButton.CROSS)!=0)
        {
            text.gameObject.SetActive(false);
        }
    }
}
