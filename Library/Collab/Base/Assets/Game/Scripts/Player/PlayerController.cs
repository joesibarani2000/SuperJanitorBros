using System.Collections;
using System.Collections.Generic;
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
            Movement(1);
            arah = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            Movement(2);
            arah = 2;
        }
        if (Input.GetKey(KeyCode.A))
        {
            Movement(3);
            arah = 3;
        }
        if (Input.GetKey(KeyCode.D))
        {
            Movement(4);
            arah = 4;
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
