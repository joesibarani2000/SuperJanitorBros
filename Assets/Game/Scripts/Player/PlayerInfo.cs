using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{

    public enum PlayerState
    {
        BRING,
        PUSH,
        NORMAL
    }

    [Header("Player State")]
    [SerializeField] private PlayerState state;

    [Header ("Animator")]
    public Animator anim;

    public PlayerState State { get => state; set => state = value; }

    public void UpdateAnimation(PlayerInfo.PlayerState state)
    {
        if (state == PlayerState.BRING)
        {

        }
        else if (state == PlayerState.NORMAL)
        {

        }
    }

}
