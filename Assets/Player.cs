using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int lives = 3;
    public float speed = 1.2f;
    private bool[] jump = new bool[2] { false,false };
    private Rigidbody2D body;
    Vector2 jumpForce = new Vector2(0, 500);
    BoxCollider2D collider;
    Vector2 rayPos;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        rayPos = new Vector2(transform.position.x, collider.bounds.min.y);
    }

    private void Update()
    {
        body.velocity = new Vector2(5  * Controller.LX(),body.velocity.y);
        if(OnGround())
        {
            for(int i=0;i<jump.Length;i++)
            {
                jump[i] = false;
            }
        }
        if((Controller.ButtonDown()&ControllerButton.CROSS)!=0)
        {
            
            //switch(Jump())
            //{
            //    case JumpState.Nope:
            //        break;
            //    case JumpState.Once:
            //        body.AddForce(jumpForce);
            //        break;
            //    case JumpState.Twice:
            //        body.AddForce(jumpForce);
            //        break;
            //    case JumpState.Slam:
            //        body.velocity = Vector2.zero;
            //        body.AddForce(-jumpForce * 2);
            //        break;

                    
            //}
        }
    }

    #region Jumping Shit
    bool OnGround()
    {
        if (Physics2D.Raycast(rayPos,-Vector2.up, 0.1f))
        {
            return true;
        }
        return false;
    }

    void Jump()
    {
        if(!jump[0])
        {
            jump[0] = true;
            body.AddForce(jumpForce);
        }
        else
        {
            if(!jump[1])
            {
                jump[1] = true;
                body.AddForce(jumpForce);
            }
            else
            {
                body.AddForce(-jumpForce * 4);
            }
        }
    }

    //JumpState Jump()
    //{
    //    for(int i=0;i<jump.Length;i++)
    //    {
    //        if(!jump[i])
    //        {
    //            Debug.Log(i);
    //            jump[i] = true;
    //            return (JumpState)i;
    //        }
    //    }
    //    return JumpState.Nope;
    //}
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