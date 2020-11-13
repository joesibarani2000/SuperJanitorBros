using TomWill;
using UnityEngine;

public class PlayerController : PlayerBehaviour
{
    protected Joystick joystick;
    public bool usingJoystick;
    /*private int i;*/
    private void Start()
    {
        joystick = FindObjectOfType<Joystick>();
        TWAudioController.PlayBGM("InGame");
        Init();
        move = true;
    }

    /*public void UsingJoystick()
    {
        i++;
        if (i % 2 == 1) usingJoystick = false;
        else usingJoystick = true;
    }*/

    private void Update()
    {
        Dash();
        if (!isDashed) {
            if (!usingJoystick)
                KeyboardMovement();
            else
            {
                JoystickMovement();
            }
            UpdateAnimationWalk(lastDirection.x, lastDirection.y, direction.sqrMagnitude);
        }
        
        Action();
    }

    public void JoystickMovement()
    {
        direction = joystick.Direction;
       if(joystick.Direction.x > 0.1f)
        {
            CreateDust();
            isAccelerating = true;
            lastDirection = Vector2.right;
        }
        if (joystick.Direction.x < -0.1f)
        {
            CreateDust();
            isAccelerating = true;
            lastDirection = Vector2.left;
        }
        if (joystick.Direction.y < -0.1f)
        {
            CreateDust();
            isAccelerating = true;
            lastDirection = Vector2.down;
        }
        if (joystick.Direction.y > 0.1f)
        {
            CreateDust();
            isAccelerating = true;
            lastDirection = Vector2.up;
        }
        if(joystick.Direction == Vector2.zero)
        {
            if (isAccelerating)
            {
                isAccelerating = false;
                timeMoveElapsed = timeToStop;
            }
        }
        MoveAccelerate();
    }

    public void KeyboardMovement()
    {
        direction = Vector2.zero;
        /*note : 1 : up , 2 : down, 3 : left , 4 : right*/
        if (Input.GetKey(KeyCode.W))
        {
            CreateDust();
            isAccelerating = true;
            direction += Vector2.up;
            lastDirection = Vector2.up;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            if (isAccelerating && lastDirection == Vector2.up)
            {
                isAccelerating = false;
                timeMoveElapsed = timeToStop;
            }
        }

        if (Input.GetKey(KeyCode.S))
        {
            CreateDust();
            isAccelerating = true;
            direction += Vector2.down;
            lastDirection = Vector2.down;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            if (isAccelerating && lastDirection == Vector2.down)
            {
                isAccelerating = false;
                timeMoveElapsed = timeToStop;
            }
        }

        if (Input.GetKey(KeyCode.A))
        {
            CreateDust();
            isAccelerating = true;
            direction += Vector2.left;
            lastDirection = Vector2.left; ;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            if (isAccelerating && lastDirection == Vector2.left)
            {
                isAccelerating = false;
                timeMoveElapsed = timeToStop;
            }
        }

        if (Input.GetKey(KeyCode.D))
        {
            CreateDust();
            isAccelerating = true;
            direction += Vector2.right;
            lastDirection = Vector2.right;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            if (isAccelerating && lastDirection == Vector2.right)
            {
                isAccelerating = false;
                timeMoveElapsed = timeToStop;
            }
        }

        MoveAccelerate();
    }

    public void DashAction()
    {
        if (interactingWithTrash) return;

        if (canDash)
        {
            TWAudioController.PlaySFX("dash");
            dashTime = startDashTime;
            isDashed = true;
            data.IsDashing = true;
            canDash = false;
        }
    }

    public void PushPickAction()
    {
        InteractTrash();
    }

    public void Action()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {

            if (interactingWithTrash) return;

            if (canDash)
            {
                TWAudioController.PlaySFX("dash");
                dashTime = startDashTime;
                isDashed = true;
                data.IsDashing = true;
                canDash = false;
            }

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            InteractTrash();
        }
    }
}
