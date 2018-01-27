using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    private int lives = 3;
    public float speed = 1.2f;
    private bool[] jump = new bool[3] { false,false, false };
    private Rigidbody2D body;
    Vector2 jumpForce = new Vector2(0, 500);
    BoxCollider2D collider;
    Vector2 rayPos;
    bool dj = true;
    SpriteRenderer spRenderer;
    private void Awake()
    {
        instance=this;
        body = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        rayPos = new Vector2(transform.position.x, collider.bounds.min.y);
        spRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        {
            body.velocity = new Vector2(5*Controller.LX(), body.velocity.y);
            if((Controller.Button()&ControllerButton.CIRCLE)!=0&&Controller.LX()!=0)
            {               
                //  body.AddForce(new Vector2(500* Controller.LX(), 0));
            }
        }
        spRenderer.flipX = body.velocity.x < 0;
        Jump();   
    }

    IEnumerator Slide()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 15 * (spRenderer.flipX ? -1 : 1)));
        body.AddForce(new Vector2(30 * (spRenderer.flipX ? -1 : 1), 0));
        yield return new WaitForSeconds(0.2f);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 15 * (spRenderer.flipX ? 1 : -1)));
    }
    #region Jumping Shit
    bool OnGround()
    {
        if (Physics2D.Raycast(rayPos,-Vector2.up, 0.2f))
        {
            return true;
        }
        return false;
    }

    void Jump()
    {
        if (OnGround())
        {
            for (int i = 0; i < jump.Length; i++)
            {
                jump[i] = false;
            }
        }
        if ((Controller.ButtonDown() & ControllerButton.CROSS) != 0)
        {
            if ((Controller.Button() & ControllerButton.DOWN) != 0&&jump[0])
            {
                jump[1] = true;
                dj = false;
            }
            else if ((Controller.Button() & (spRenderer.flipX ? ControllerButton.LEFT : ControllerButton.RIGHT)) != 0)
            {
               
                if (spRenderer.flipX)
                {
                    if (Physics2D.Raycast(new Vector2(collider.bounds.min.x, transform.position.y), -transform.right, 0.1f))
                    {
                        body.AddForce(jumpForce + new Vector2(0, -500));
                    }
                }
                else
                {
                    if (Physics2D.Raycast(new Vector2(collider.bounds.max.x, transform.position.y), transform.right, 0.1f))
                    {
                        body.AddForce(jumpForce + new Vector2(0, 500));
                    }
                }
            }
            if (!jump[0])
            {
                jump[0] = true;
                body.AddForce(jumpForce);
            }
            else
            {
                if (!jump[1])
                {
                    jump[1] = true;
                    body.AddForce(jumpForce);
                }
                else if (!jump[2])
                {
                    jump[2] = true;
                    StartCoroutine(SlamPause(dj));
                    dj = true;
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
        body.AddForce(-jumpForce * 3);
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