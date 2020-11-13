using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject tutorMenu;
    [SerializeField] private PlayerBehaviour player;
    private void Start()
    {
        player.move = false;
        StartCoroutine(turnoff());
    }
    // Start is called before the first frame update
    IEnumerator turnoff()
    {
        yield return new WaitForSeconds(10f);
        player.move= true;
        tutorMenu.SetActive(false);
    }
}
