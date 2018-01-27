using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    private int lives = 3;
    public float speed = 1.2f;
    public bool[] jump = new bool[3] { false,false, false };
    private Rigidbody2D body;
    BoxCollider2D collider;
    Vector2 rayPos;
    bool dj = true;
    SpriteRenderer spRenderer;

    public float fallMulti = 2.5f;
    private float stompMulti;
    public float lowJumpMulti = 2;
    public float jumpForce = 10;
    private void Awake()
    {
        instance=this;
        body = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        rayPos = new Vector2(transform.position.x, transform.position.y+collider.bounds.min.y);
        spRenderer = GetComponent<SpriteRenderer>();
        stompMulti = fallMulti * 8;
    }

    private void Update()
    {
        if(body.velocity.y<0)
        {
            body.velocity += Vector2.up * Physics2D.gravity.y * ((jump[2]?stompMulti:fallMulti) - 1) * Time.deltaTime;
        }
        else if (body.velocity.y>0 && !Controller.GetButton(ControllerButton.CROSS))
        {
                body.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMulti - 1) * Time.deltaTime;
        }      
            body.velocity = new Vector2(5*Controller.LX(), body.velocity.y); 
        spRenderer.flipX = body.velocity.x < 0;
        Jump();   
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
        if (OnGround())
        {
            //for (int i = 0; i < jump.Length; i++)
            //{
            //    jump[i] = false;
            //}
        }
        if ((Controller.ButtonDown() & ControllerButton.CROSS) != 0)
        {
            if ((Controller.Button() & ControllerButton.DOWN) != 0&&jump[0])
            {
                jump[1] = true;
                dj = false;
            }
           
            if (!jump[0])
            {
                jump[0] = true;
                body.velocity = Vector2.up*jumpForce;
                return;
            }
            else
            {
                if (!jump[1])
                {
                    jump[1] = true;
                    body.velocity = Vector2.up * jumpForce;
                    return;
                }
                else if (!jump[2])
                {
                    jump[2] = true;
                    StartCoroutine(SlamPause(dj));
                    dj = true;
                    return;
                }
            }
        }
    }
    IEnumerator SlamPause(bool _dj)
    {
        body.velocity = Vector2.zero;
        body.simulated = false;
        float lerpy = 0;
        while (lerpy<1)
        {
            lerpy += Time.deltaTime*2;
            //transform.rotation = Quaternion.Euler(0, 0, lerpy* (_dj?720:360));
            yield return new WaitForEndOfFrame();
        }
        body.simulated = true;
    }

    public void Death()
    {
        Debug.Log("You died");
        lives--;
        //reset to point or what;
    }
    #endregion

    private void OnCollisionEnter2D(Collision2D _col)
    {
        if(jump[2])
        {
            for(int i=0;i<jump.Length;i++)
            {
                jump[i] = false;
            }
        }    }

}

enum JumpState
{
    Once = 0,
    Twice = 1,
    Slam = 2,
    Nope = 3,    
}