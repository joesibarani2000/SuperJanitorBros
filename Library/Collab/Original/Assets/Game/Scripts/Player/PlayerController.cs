using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : PlayerBehaviour
{
    public Transform spawnpoint;
    public GameObject prefab;

    private void Update()
    {
        KeyboardMovement();
        Dash();
    }

    public void KeyboardMovement()
    {
        /*note : 1 : up , 2 : down, 3 : left , 4 : right*/
        if (Input.GetKey(KeyCode.W))
        {
            isAccelerating = true;
            if(direction!= Vector2.up)
            {
                timeMoveElapsed = 0;
            }
            direction = Vector2.up;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            if (isAccelerating && direction == Vector2.up)
            {
                isAccelerating = false;
                if (timeMoveElapsed > timeToStop)
                {
                    timeMoveElapsed = timeToStop;
                }
            }
        }

        if (Input.GetKey(KeyCode.S))
        {
            isAccelerating = true;
            if (direction != Vector2.down)
            {
                timeMoveElapsed = 0;
            }
            direction = Vector2.down;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            if (isAccelerating && direction == Vector2.down)
            {
                isAccelerating = false;
                if (timeMoveElapsed > timeToStop)
                {
                    timeMoveElapsed = timeToStop;
                }
            }
        }

        if (Input.GetKey(KeyCode.A))
        {
            isAccelerating = true;
            if (direction != Vector2.left)
            {
                timeMoveElapsed = 0;
            }
            direction = Vector2.left;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            if (isAccelerating && direction == Vector2.left)
            {
                isAccelerating = false;
                if (timeMoveElapsed > timeToStop)
                {
                    timeMoveElapsed = timeToStop;
                }
            }
        }

        if (Input.GetKey(KeyCode.D))
        {
            isAccelerating = true;
            if (direction != Vector2.right)
            {
                timeMoveElapsed = 0;
            }
            direction = Vector2.right;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            if (isAccelerating && direction == Vector2.right)
            {
                isAccelerating = false;
                if (timeMoveElapsed > timeToStop)
                {
                    timeMoveElapsed = timeToStop;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            pressed = true;
            delete = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Push
            if (enemyRb != null)
            {
                enemyRb.bodyType = RigidbodyType2D.Dynamic;
                enemyRb.AddForce(distance * enemyForce);
            }

            // Lift
            if (checkPickup)
            {
                pickup.gameObject.SetActive(true);
                pickup.sprite = sampahPickup.sprite;
                pickup.color = sampahPickup.color;
                checkAngkat = true;
                checkPickup = false;
                Destroy(sampahPickup);
            }

            //Drop
            if (checkPickup == false)
            {
                Instantiate(prefab, spawnpoint.position, spawnpoint.rotation);
                checkAngkat = false;
                checkPickup = true;
            }
        }
    }
}
