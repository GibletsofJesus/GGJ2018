using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{

    public Transform jumpTriggerPos;
    public Transform moveTriggerPos;
    public Transform slamTriggerPos;
    public Text instructional;
    
    bool controller = false; //0 for ps4, 1 for xbox
	// Use this for initialization
	void Start ()
    {
        controller = Input.GetJoystickNames()[0].Contains("Xbox") ? true : false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(PromptDistance(moveTriggerPos.position))
        {
            instructional.text = "Left stick to Move";
            if(!instructional.gameObject.activeInHierarchy)
            {
               instructional.transform.position = moveTriggerPos.position;
                instructional.gameObject.SetActive(true);
            }
        }
        else if (PromptDistance(jumpTriggerPos.position))
        {
            instructional.text = "Press " + (controller ? "B " : "Cross ") + "To Jump";
            if (!instructional.gameObject.activeInHierarchy)
            {
                instructional.transform.position = jumpTriggerPos.position;
                instructional.gameObject.SetActive(true);
            }
        }
        else if (PromptDistance(slamTriggerPos.position))
        {
            instructional.text = "Press Down and " + (controller ? "X " : "Square ") + "To Slam";
            if (!instructional.gameObject.activeInHierarchy)
            {
                instructional.transform.position = slamTriggerPos.position;

                instructional.gameObject.SetActive(true);
            }
        }
        else
        {
            instructional.gameObject.SetActive(false);
        }
	}

    bool PromptDistance(Vector2 _pos)
    {
        if (Vector2.Distance(Player.instance.transform.position, _pos)<3)
        {
            return true;
        }
        return false;
    }
}
