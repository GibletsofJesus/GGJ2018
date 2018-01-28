using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowUI : MonoBehaviour
{
    public List<GameObject> stuff = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            for (int i = 0; i < stuff.Count; i++)
                stuff[i].SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.GetComponent<Player>())
        {
            for (int i = 0; i < stuff.Count; i++)
                stuff[i].SetActive(false);
        }
    }
}
