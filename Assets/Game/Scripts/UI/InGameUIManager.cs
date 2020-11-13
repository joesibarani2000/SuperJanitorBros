using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TomWill;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;

public class InGameUIManager : MonoBehaviour
{
    public static InGameUIManager instance;
    public GameObject pause, levelComplete;
    private bool isPause;
    [SerializeField]private GameObject [] rateStar;
    [SerializeField]private Sprite emptyStar;
    [SerializeField]private Sprite fullStar;
    [SerializeField] private ParticleSystem [] starPartikel;
    

    
    [SerializeField] private Slider scoreSlider;
    [SerializeField] private Text timeText, scoreText;

    [HideInInspector] public LevelInfo info;
    private bool levelClear;

    private void Start()
    {
        UINavigation.instance.ActiveNavigation(false);
        TWLoading.OnSuccessLoad(() => TWTransition.FadeOut());
        Init();
        levelClear = false;
        instance = this;
        if (GameVariables.ACTIVE_LEVEL != null) info = GameVariables.ACTIVE_LEVEL;
        
    }

    
    private void Init()
    {
        int i = 0;
        while(i< rateStar.Length)
        {
            rateStar[i].GetComponent<Image>().overrideSprite = emptyStar;
            i++;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPause) Resume();
            else Pause();
        }

        if (!levelClear)
        {
            ShowScore();
            ShowTime();
            CheckComplete();
        }
    }

    public void ShowStar()
    {
        StartCoroutine(Star());
    }

    IEnumerator Star()
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < GameData.instance.Star; i++)
        {
            rateStar[i].GetComponent<Image>().overrideSprite = fullStar;
            starPartikel[i].Play();
            yield return new WaitForSeconds(0.5f);
        }

        //if (GameData.instance.Star > 0) {
        //    rateStar[0].GetComponent<Image>().overrideSprite = fullStar;
        //    starPartikel[0].Play();
        //}
        //yield return new WaitForSeconds(0.5f);
        //if (GameData.instance.Star > 1)
        //{
        //    rateStar[1].GetComponent<Image>().overrideSprite = fullStar;
        //    starPartikel[1].Play();
        //}
        //yield return new WaitForSeconds(0.5f);
        //if (GameData.instance.Star > 2)
        //{
        //    rateStar[2].GetComponent<Image>().overrideSprite = fullStar;
        //    starPartikel[2].Play();
        //}

        yield return new WaitForSeconds(1f);
        Time.timeScale = 0f;
    }

    public void ShowScore()
    {
        scoreSlider.value = GameData.instance.Score;
        scoreSlider.maxValue = GameData.instance.MaxScore;
        scoreText.text = GameData.instance.Score.ToString();
    }

    public void ShowTime()
    {
        string second = Mathf.Floor((GameData.instance.TimeGameplay)).ToString("00");
        timeText.text = second;
    }

    public void CheckComplete()
    {
        if (GameData.instance.CheckScore()) ShowLevelComplete();
        if (GameData.instance.TimeGameplay <= 1 && !levelComplete.activeSelf) ShowLevelComplete();
    }

    public void ShowLevelComplete()
    {
        levelClear = true;
        UINavigation.instance.ActiveNavigation(true, "GameOver");
        GameData.instance.GetPlayer().move = false;
        ShowStar();
        levelComplete.SetActive(true);
        timeText.gameObject.SetActive(false);

        if (!GameData.instance.IsNull())
        {
            int collect = GameData.instance.Star;
            //Debug.Log("Game Data : " + GameData.instance.Star);
            //Debug.Log("GameVariables : " + GameVariables.ACTIVE_LEVEL.coinCollect);
            int coinCollect = GameVariables.ACTIVE_LEVEL.coinCollect;
            
            if (collect > GameVariables.ACTIVE_LEVEL.coinCollect)
            {
                GameVariables.ACTIVE_LEVEL.coinCollect = GameData.instance.Star;
                GameData.instance.SaveData();
            }
        }
    }

    public void Pause()
    {
        UINavigation.instance.ActiveNavigation(true, "Pause");
        TWAudioController.PlaySFX("click");
        pause.SetActive(true);
        Time.timeScale = 0f;
        isPause = true;
    }
    public void Resume()
    {
        TWAudioController.PlaySFX("click");
        UINavigation.instance.ActiveNavigation(false);
        pause.SetActive(false);
        Time.timeScale = 1f;
        isPause = false;
    }

    public void Continue()
    {
        TWAudioController.PlaySFX("click");
        TWTransition.FadeIn(() => TWLoading.LoadScene("LevelSelect"));
    }

    public void Retry()
    {
        TWAudioController.PlaySFX("click");
        TWTransition.FadeIn(() => TWLoading.LoadScene(SceneManager.GetActiveScene().name));
    }

    public void MainMenu()
    {
        TWAudioController.PlaySFX("click");
        Time.timeScale = 1f;
        TWTransition.FadeIn(() => TWLoading.LoadScene("LevelSelect"));
    }


}
