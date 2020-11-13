using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TomWill;
public class UIMainMenuManager : MonoBehaviour
{
    private void Start()
    {
        TWTransition.FadeOut();
    }
    public void PlayGame()
    {
        TWTransition.FadeIn(() => TWLoading.LoadScene("MainScene"));
    }

    public void Keluar()
    {
        Application.Quit();
    }
}
