using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicCon : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip gameIntro;
    public AudioClip normalGhost;
    private bool isIntroPlayed = false;

    public void OnButtonClick()
    {
        if (!isIntroPlayed)
        {
            PlayIntro();
            isIntroPlayed = true;
        }
        else
        {
            PlaynormalGhost();
        }
    }

    private void PlayIntro()
    {
        audioSource.clip = gameIntro;
        audioSource.Play();
    }

    private void PlaynormalGhost()
    {
        audioSource.clip = normalGhost;
        audioSource.Play();
    }
    
}
