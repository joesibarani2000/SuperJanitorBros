using System.Collections;
using System.Collections.Generic;
using TomWill;
using UnityEngine;

public class TestDummy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TWTransition.FadeOut();
        TWAudioController.PlayBGM("Default");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TWTransition.FadeIn(() => TWLoading.LoadScene("MainScene"));
        }
    }

    public void AdjustVolume(float volume)
    {
        TWAudioController.GlobalBGMVolume = volume;
    }
}
