﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    protected Vector2 direction;
    [SerializeField] protected float speed, startDashTime, dashTime, dashSpeed;
    protected int arah;
    protected bool pressed;
    public bool delete;
    [SerializeField] private Rigidbody2D rb;

    protected Rigidbody2D enemyRb;
    [SerializeField] protected float enemyForce;
    protected Vector2 distance;

    [SerializeField] protected SpriteRenderer pickup;
    protected bool checkPickup;
    protected SpriteRenderer sampahPickup;
    protected bool checkAngkat;

    public static PlayerBehaviour instance;

    TrailRenderer trail;

    [SerializeField] protected float timeMoveElapsed;
    [SerializeField] protected bool isAccelerating;
    [SerializeField] protected float timeToAccelerate;
    [SerializeField] protected float timeToStop;

    private void Start()
    {
        instance = this;
        Time.timeScale = 1f;
        rb = GetComponent<Rigidbody2D>();
        trail = GetComponent<TrailRenderer>();
    }

    protected void MoveAccelerate()
    {
        if (isAccelerating)
        {
            Accelerate();
            Movement(timeMoveElapsed / timeToAccelerate);
        }
        else
        {
            Stop();
            Movement(timeMoveElapsed / timeToStop);
        }
    }

    private void Accelerate()
    {
        timeMoveElapsed += Time.deltaTime;

        if (timeMoveElapsed >= timeToAccelerate) timeMoveElapsed = timeToAccelerate;
    }

    private void Stop()
    {
        timeMoveElapsed -= Time.deltaTime;

        if (timeMoveElapsed <= 0) timeMoveElapsed = 0;
    }

    public void Movement(float accelerate)
    {
        transform.Translate(direction *speed* Time.deltaTime * accelerate);
    }

    public void Dash()
    {
        if (arah != 0)
        {
            if (dashTime <= 0)
            {
                arah = 0;
                dashTime = startDashTime;
                rb.velocity = Vector2.zero;
                pressed = false;
                delete = false;
            }
            else
            {
                if (pressed)
                {
                    dashTime -= Time.deltaTime;
                    trail.enabled = true;
                    if (arah == 1)
                    {
                        rb.velocity = Vector2.up * dashSpeed;
                    }
                    else if (arah == 2)
                    {
                        rb.velocity = Vector2.down * dashSpeed;
                    }
                    else if (arah == 3)
                    {
                        rb.velocity = Vector2.left * dashSpeed;
                    }
                    else if (arah == 4)
                    {
                        rb.velocity = Vector2.right * dashSpeed;
                    }
                    
                }
                else trail.enabled = false;
            }
        }    
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Push")
        {
            Vector2 target = collision.gameObject.transform.position;
            Vector2 player = this.transform.position;
            distance = (target - player).normalized;
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            enemyRb = rb;
        }

        if (collision.gameObject.tag == "Pickup")
        {
            checkPickup = true;
            sampahPickup = collision.gameObject.GetComponent<SpriteRenderer>();
            if (checkAngkat)
            {
                Destroy(collision.gameObject);
                checkAngkat = !checkAngkat;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Push")
        {
            enemyRb = null;
            distance = Vector2.zero;
        }

        if (collision.gameObject.tag == "Pickup")
        {
            checkPickup = false;
            sampahPickup = null;
        }
    }

}
