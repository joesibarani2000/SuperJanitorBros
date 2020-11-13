using System.Collections;
using System.Collections.Generic;
using TomWill;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] protected PlayerData data;
    [SerializeField] protected Vector2 direction, lastDirection;
    [SerializeField] protected float startDashTime, dashTime;

    protected bool isDashed, canDash;
    public bool move;
    [SerializeField] protected float dashDelay;

    [SerializeField] protected Rigidbody2D rb;  

    protected bool bringTrash;
    [SerializeField]protected Animator animation;
    TrailRenderer trail;

    [SerializeField] private Transform pickUpArea;

    [SerializeField] protected float timeMoveElapsed;
    [SerializeField] protected bool isAccelerating;
    [SerializeField] protected float timeToStop;

    public ParticleSystem dust;
    [SerializeField] protected TrashBehaviour interactTrash;
    [SerializeField] protected bool interactingWithTrash;
    public Ghost ghost;

    protected void Init()
    {
        
        canDash = true;
        Time.timeScale = 1f;
        rb = GetComponent<Rigidbody2D>();
        trail = GetComponent<TrailRenderer>();
    }

    public bool PlayerDashing()
    {
        return data.IsDashing;
    }

    protected void MoveAccelerate()
    {
        if (isAccelerating)
        {
            Movement(1);
        }
        else
        {
            Stop();
            Movement(timeMoveElapsed / timeToStop);
        }
    }

    private void Stop()
    {
        timeMoveElapsed -= Time.deltaTime;

        if (timeMoveElapsed <= 0) timeMoveElapsed = 0;
    }



    protected void Movement(float accelerate)
    {
       if(move)transform.Translate(direction * data.Speed * Time.deltaTime*accelerate);
    }

    protected void Dash()
    {
        if (lastDirection != Vector2.zero && isDashed)
        {
            if (dashTime <= 0)
            {
                ghost.makeGhost = false;
                rb.velocity = Vector2.zero;
                animation.SetBool("swipe", false);
                isDashed = false;
                data.IsDashing = false;
                trail.enabled = false;
                StartCoroutine(Delay());
            }
            else
            {
                animation.SetBool("swipe", true);
                ghost.makeGhost = true;
                dashTime -= Time.deltaTime;
                trail.enabled = true;
                rb.velocity = lastDirection * data.DashSpeed;
            }
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(this.dashDelay);
        canDash = true;
    }


    protected void InteractTrash()
    {
        if (!interactTrash) return;

        TrashData.TrashType trashType = interactTrash.GetTrashType();

        if (trashType == TrashData.TrashType.PUSH)
        {
            if (!interactingWithTrash) StartCoroutine(push());
        }
        else if (trashType == TrashData.TrashType.BRING)
        {
            if (!bringTrash)
            {
                TWAudioController.PlaySFX("place");
                animation.SetBool("angkat", true);
                bringTrash = true;
                interactingWithTrash = true;
                interactTrash.PickActionTrash(pickUpArea);
            }
            else
            {
                TWAudioController.PlaySFX("pickup");
                animation.SetBool("angkat", false);
                bringTrash = false;
                interactingWithTrash = false;
                interactTrash.DropActionTrash(transform.position, lastDirection.x > 0);
            }
        }
    }

    IEnumerator push()
    {
        TWAudioController.PlaySFX("push");
        animation.SetBool("push", true);
        interactTrash.PushActionTrash(lastDirection, 0.5f);
        yield return new WaitForSeconds(0.2f);
        animation.SetBool("push", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Trash")
        {
            if (!interactingWithTrash) interactTrash = collision.GetComponent<TrashBehaviour>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Trash")
        {
            if (interactTrash == collision.gameObject.GetComponent<TrashBehaviour>())
            {
                if (!interactingWithTrash) interactTrash = null;
            }
        }
    }

    public void CreateDust()
    {
        dust.Play();
    }

    public void UpdateAnimationWalk(float x, float y, float speed)
    {
        animation.SetFloat("horizontal", x);
        animation.SetFloat("vertical", y);
        animation.SetFloat("speed", speed);
    }

}
