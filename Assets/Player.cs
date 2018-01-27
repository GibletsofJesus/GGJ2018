using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int lives = 3;
    public float maxHorizVelocity = 1.0f;
    public float speed = 1.2f;
    public bool wallJumping = false;
    private float wallJumpTimer = 0.0f;
    public int totalJumps = 3;
    [SerializeField]
    private int jumpsRemaining = 3;
    private Rigidbody2D body;
    Vector2 jumpForce = new Vector2(0, 500);
    BoxCollider2D collider;
    Vector2 rayPos;
    bool dj = true;
    SpriteRenderer spRenderer;
    private void Awake()
    {
        jumpsRemaining = totalJumps;
        body = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        rayPos = new Vector2(transform.position.x, collider.bounds.min.y);
        spRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (wallJumping) {
            wallJumpTimer += Time.deltaTime;
            if (wallJumpTimer >= 0.1f) {
                wallJumping = false;
            }
        }
        else {
            if (Controller.LX() != 0) {
                body.velocity += 3 * Time.deltaTime * new Vector2(25*Controller.LX(), 0);
            }
            else {
                body.velocity -= 5 * Time.deltaTime * new Vector2(body.velocity.x, 0);
            }
            if (Math.Abs(body.velocity.x) > maxHorizVelocity) {
                body.velocity = new Vector2(Mathf.Clamp(body.velocity.x, -maxHorizVelocity, maxHorizVelocity) , body.velocity.y);
            }
            if((Controller.Button()&ControllerButton.CIRCLE)!=0&&Controller.LX()!=0)
            {
                body.AddForce(new Vector2(800* Controller.LX(), 0));
            }
            spRenderer.flipX = body.velocity.x < 0;
        }
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
        if (Physics2D.Raycast(rayPos, Vector2.down, 0.2f))
        {
            return true;
        }
        return false;
    }

    bool OnWall()
    {
        if (Physics2D.Raycast(new Vector2(spRenderer.flipX ? collider.bounds.min.x : collider.bounds.max.x, transform.position.y), spRenderer.flipX ? -transform.right : transform.right, 0.1f)) 
        {
             return true;
        }
        return false;
    }

    void Jump()
    {
        if (OnGround())
        {
            jumpsRemaining = totalJumps;
        }
        if ((Controller.ButtonDown() & ControllerButton.CROSS) != 0)
        {
            if ((Controller.Button() & ControllerButton.DOWN) != 0 && jumpsRemaining != totalJumps)
            {
                StartCoroutine(SlamPause(dj));
                dj = true;
            }
            else if ((Controller.Button() & (spRenderer.flipX ? ControllerButton.LEFT : ControllerButton.RIGHT)) != 0)
            {
                if (spRenderer.flipX)
                {
                    if (Physics2D.Raycast(new Vector2(collider.bounds.min.x, transform.position.y), -transform.right, 0.1f))
                    {
                        body.AddForce(jumpForce + new Vector2(400, 0));
                        wallJumping = true;
                        wallJumpTimer = 0.0f;
                        jumpsRemaining = 1;
                    }
                }
                else
                {
                    if (Physics2D.Raycast(new Vector2(collider.bounds.max.x, transform.position.y), transform.right, 0.1f))
                    {
                        body.AddForce(jumpForce + new Vector2(-400, 0));
                        wallJumping = true;
                        wallJumpTimer = 0.0f;
                        jumpsRemaining = 1;
                    }
                }
            }
            if (!wallJumping) {
                if (jumpsRemaining != 0) {
                    body.AddForce(jumpForce);
                    jumpsRemaining--;
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
            transform.rotation = Quaternion.Euler(0, 0, lerpy* (_dj?720:360));
            yield return new WaitForEndOfFrame();
        }
        transform.rotation = Quaternion.Euler(0, 0, 0);
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

    }

}


enum JumpState
{
    Once = 0,
    Twice = 1,
    Slam = 2,
    Nope = 3,
    
}