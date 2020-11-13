using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    private bool isDashing;
    [SerializeField]private float speed, dashSpeed;

    public bool IsDashing { get => isDashing; set => isDashing = value; }
    public float DashSpeed { get => dashSpeed; set => dashSpeed = value; }
    public float Speed { get => speed; set => speed = value; }
}
