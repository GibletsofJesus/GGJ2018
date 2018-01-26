using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int lives = 3;
    public float speed = 10;
    private bool[] jump = new bool[3] { false, false,false };

    

    private void Awake()
    {

    }



    #region Jumping Shit
    bool OnGround()
    {
        if (Physics2D.Raycast(transform.position,-Vector2.up, 2))
        {
            return true;
        }
        return false;
    }

    JumpState Jump()
    {
        for(int i=0;i<jump.Length;i++)
        {
            if(!jump[i])
            {
                jump[i] = true;
                return (JumpState)i;
            }
        }
        return JumpState.Nope;
    }
    #endregion

    private void OnCollisionEnter2D(Collision2D _col)
    {
        
    }
}


enum JumpState
{
    Once = 0,
    Twice = 1,
    Slam = 2,
    Nope = 3,
    
}