using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using TomWill;
using UnityEngine.UI;


public class LevelSelectManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup startMenu;
    [SerializeField] private Image blurImage;

    private bool startToSelect;
    private void Start()
    {
        blurImage.material.SetFloat("_Size", 3.4f);
        TWAudioController.PlayBGM("InGame");
        TWLoading.OnSuccessLoad(() => TWTransition.FadeOut());
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if(!startToSelect && Input.GetMouseButtonDown(0))
        {
            startToSelect = true;
            GameVariables.WAS_PLAY = true;
            FadeStart();
        }
    }
    private void FadeStart()
    {
        DOTween.Sequence()
            .Append(blurImage.material.DOFloat(0, "_Size", 1f))
            .Join(startMenu.DOFade(0, 1f))
            .OnComplete(() => startMenu.gameObject.SetActive(false));
    }
    public void startLevel(string level)
    {
        //AudioController.PlaySFX("Button_Click");
        //AudioController.PlaySFX("Scene_Transition");
        TWAudioController.StopBGMPlayed();
        TWTransition.FadeIn(() => TWLoading.LoadScene(level));
    }
    public void StarToSelectLevel(bool flag)
    {
        startToSelect = flag;

        if (startToSelect)
        {
            blurImage.material.SetFloat("_Size", 0f);
            startMenu.alpha = 0f;
            startMenu.gameObject.SetActive(false);
        }
        else
        {
            blurImage.material.SetFloat("_Size", 2.2f);
            startMenu.alpha = 1f;
            startMenu.gameObject.SetActive(true);
        }
    }

    public void Click_Sound()
    {
        TWAudioController.PlaySFX("click");
    }

    public void keluar()
    {
        Application.Quit();
    }
}
