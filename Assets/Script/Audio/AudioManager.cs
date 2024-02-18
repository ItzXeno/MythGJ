using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public enum GameState { Menu, InGame, Finish };
    private GameState currentState;


    public static AudioManager instance;

    public AudioClip menuBGM;
    public AudioClip gameBGM;
    public AudioClip finishClip;

    public AudioSource bgmSource;
    public AudioSource sfxSource;

    private void Awake()
    {
        #region Manager_Part
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion

        #region BGM_Part
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            CheckGameState(GameState.InGame);
        } 
        else 
        {
            CheckGameState(GameState.Menu);
        }
        #endregion
    }

    private void Start()
    {
        //ChangeBGM();
    }

    private void Update()
    {
        ChangeBGM();
    }

    public void CheckGameState(GameState newState)
    {
        currentState = newState;
        switch (currentState)
        {
            case GameState.Menu:
                break;
            case GameState.InGame:
                break;
            case GameState.Finish:
                break;
        }
    }

    private void ChangeBGM()
    {
        switch (currentState)
        {
            case GameState.Menu:
                PlayBGMClip(menuBGM); 
                break;
            case GameState.InGame:
                PlayBGMClip(gameBGM);
                break;
            case GameState.Finish:
                StopPlaying();
                PlaySFXClip(finishClip);
                break;
        }
    }

    //For BGM only
    public void PlayBGMClip(AudioClip clip)
    {
        if (bgmSource.clip != clip)
        {
            bgmSource.clip = clip;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    //For all OTHER audio clips
    public void PlaySFXClip(AudioClip clip)
    {
        sfxSource.clip = clip;
        sfxSource.loop = false;

        sfxSource.PlayOneShot(clip);
    }

    public void StopPlaying() 
    {
        bgmSource.Stop();     
    }
}
