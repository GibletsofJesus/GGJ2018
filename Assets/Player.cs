﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    public event Action Dead;
    [Header("Sprites")]
    public Sprite[] m_sprites;
    int frame;

    [Header("Audio")]
    public AudioClip[] jump_sfx;
    public AudioClip[] death_sfx;
    public AudioClip thud;
    
    [Header("Everything else")]
    public ParticleSystem m_poof,m_poof2;
    public float maxHorizVelocity = 1.0f;
    private int lives = 3;
    public bool wallJumping = false;
    private float wallJumpTimer = 0.0f;
    public int totalJumps = 2;
    [SerializeField]
    private int jumpsRemaining = 2;
    public Rigidbody2D body;
    Vector2 jumpForce = new Vector2(0, 500);
    BoxCollider2D collider;
    Vector2 rayPos;
    bool dj = true;
    public SpriteRenderer spRenderer;
    float fallMulti = 2.1f;
    float jumpMulti = 2;
    float stompMulti;
    float jumpVel = 5;

    private void Awake()
    {
        Screen.SetResolution(1024,860,false);
        instance = this;
        jumpsRemaining = totalJumps;
        body = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        rayPos = new Vector2(transform.position.x, collider.bounds.min.y);
        spRenderer = GetComponent<SpriteRenderer>();
        stompMulti = fallMulti *5;
    }

    private void Update()
    {
        if (body.velocity.y < 0)
        {
            body.velocity += Vector2.up * Physics2D.gravity.y * ((slamming ?1 : fallMulti) - 1) * Time.deltaTime;
        }
        else if (body.velocity.y > 0 && !Controller.GetButton(ControllerButton.CROSS))
        {
            body.velocity += Vector2.up * Physics2D.gravity.y * (jumpMulti - 1) * Time.deltaTime;
        }

        if (transform.position.y < -5)
        {
            Objective.instance.RestartLevel();
        }
        rayPos = new Vector2(transform.position.x, collider.bounds.min.y);
        if (wallJumping)
        {
            wallJumpTimer += Time.deltaTime;
            if (wallJumpTimer >= 0.1f)
            {
                wallJumping = false;
            }
        }
        else
        {
            if (Controller.LX() != 0 && !slamming)
            {
                body.velocity += 3 * Time.deltaTime * new Vector2(25 * Controller.LX(), 0);
            }
            else if (!slamming)
            {
               body.velocity -= 10 * Time.deltaTime * new Vector2(body.velocity.x, 0);
            }
            if (Math.Abs(body.velocity.x) > maxHorizVelocity)
            {
                body.velocity = new Vector2(Mathf.Clamp(body.velocity.x, -maxHorizVelocity, maxHorizVelocity), body.velocity.y);
            }
            if ((Controller.Button() & ControllerButton.CIRCLE) != 0 && Controller.LX() != 0)
            {
                body.AddForce(new Vector2(800 * Controller.LX(), 0));
            }
            spRenderer.flipX = body.velocity.x < 0;
        }
        if (OnGround())
        {
            if ( spRenderer.sprite == m_sprites[4])
            {
                Screenshake.instance.Shake(.5f, 0.05f);            
                m_poof2.Emit(4);
                SoundManager.instance.PlaySound(thud);
            }
            if (Mathf.Abs(body.velocity.x) > .5f)
            {
                spRenderer.sprite = m_sprites[Mathf.RoundToInt(frame / 10)];
                frame = frame < 40 ? frame + 1 : 0;
            }
            else
            {
                spRenderer.sprite = m_sprites[5 + Mathf.RoundToInt(Time.time % 1)];
            }            
       }
        else if (OnWall() && !slamming)
        {
            if ( spRenderer.sprite == m_sprites[4])
         {       Screenshake.instance.Shake(.5f, 0.05f);
                m_poof2.Emit(4);
        }
            if (Controller.LX() > 0.25f && !spRenderer.flipX)
                spRenderer.sprite = m_sprites[7];
            else if (Controller.LX() < -0.25f && spRenderer.flipX)
                spRenderer.sprite = m_sprites[7];
            else
                spRenderer.sprite = m_sprites[4];
        }
        else
            spRenderer.sprite = m_sprites[4];
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
        Debug.DrawLine(rayPos, rayPos + (Vector2.down * (0.05f + Mathf.Abs(body.velocity.y / 100))), Color.red, 0.1f);
        RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.down, 0.05f + Mathf.Abs(body.velocity.y / 100));
        if (hit)
        {
            if (slamming)
            {
                if (hit.collider.gameObject.GetComponent<IGetHit>() != null)
                {
                    hit.collider.gameObject.GetComponent<IGetHit>().GotHit();
                }
                slamming = false;
            }
            return true;
        }
        //if (Physics2D.Raycast(rayPos, Vector2.down, 0.05f + Mathf.Abs(body.velocity.y / 100)))
        //{
        //    slamming = false;
        //    return true;
        //}
        return false;
    }

    bool OnWall()
    {
        Vector3 vec = new Vector3(spRenderer.flipX ? collider.bounds.min.x : collider.bounds.max.x, transform.position.y, 0);
        Debug.DrawLine(vec, vec + (spRenderer.flipX ? -transform.right * 0.05f : transform.right * 0.05f), Color.green, 0.1f);
        if (Physics2D.Raycast(vec, spRenderer.flipX ? -transform.right : transform.right, 0.05f))
        {
            return true;
        }
        return false;
    }

    //For breaking boxes
    public float lastyVel = 0;
    void LateUpdate()
    {
        lastyVel = body.velocity.y;
    }

    void Jump()
    {
        if (OnGround())
        {
            jumpsRemaining = totalJumps;
        }
        if ((Controller.Button() & ControllerButton.DOWN) != 0 && jumpsRemaining != totalJumps)
        {
            if ((Controller.ButtonDown() & ControllerButton.SQUARE) != 0)
            {
                StartCoroutine(SlamPause(dj));
                dj = true;
            }
        }
        if ((Controller.ButtonDown() & ControllerButton.CROSS) != 0)
        {
            if ((Controller.Button() & (spRenderer.flipX ? ControllerButton.LEFT : ControllerButton.RIGHT)) != 0)
            {
                if (spRenderer.flipX)
                {
                    if (Physics2D.Raycast(new Vector2(collider.bounds.min.x, transform.position.y), -transform.right, 0.1f))
                    {
                        SoundManager.instance.PlaySound(jump_sfx[UnityEngine.Random.Range(0,jump_sfx.Length-1)]);
                        body.velocity = (Vector2.up * jumpVel) + ((Vector2.right) * jumpVel);   
                        wallJumping = true;
                        wallJumpTimer = 0.0f;
                        jumpsRemaining = 1;
                    }
                }
                else
                {
                    if (Physics2D.Raycast(new Vector2(collider.bounds.max.x, transform.position.y), transform.right, 0.1f))
                    {
                        SoundManager.instance.PlaySound(jump_sfx[UnityEngine.Random.Range(0,jump_sfx.Length-1)]);
                        body.velocity = (Vector2.up * jumpVel) + ((-Vector2.right) * jumpVel);
                        wallJumping = true;
                        wallJumpTimer = 0.0f;
                        jumpsRemaining = 1;
                    }
                }
            }
            if (!wallJumping)
            {
                if (jumpsRemaining != 0)
                {                       
                    m_poof2.Emit(4);
                    SoundManager.instance.PlaySound(jump_sfx[UnityEngine.Random.Range(0,jump_sfx.Length-1)]);
                    body.velocity = Vector2.up * jumpVel;
                    jumpsRemaining--;
                }
            }
        }
    }

    public void DeathSfx()
    {
        //Dead();
        SoundManager.instance.PlaySound(death_sfx[UnityEngine.Random.Range(0,death_sfx.Length-1)]);
        GameOver.instance.Died();
    }

    public bool slamming = false;
    IEnumerator SlamPause(bool _dj)
    {
        slamming = true;
        body.velocity = Vector2.zero;
        body.simulated = false;
        float lerpy = 0;
        body.constraints = RigidbodyConstraints2D.None;
        while (lerpy < 1)
        {
            lerpy += Time.deltaTime * 4;
            transform.rotation = Quaternion.Euler(0, 0, lerpy * 360);
            yield return new WaitForEndOfFrame();
        }
        body.constraints = RigidbodyConstraints2D.FreezeRotation;
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

}