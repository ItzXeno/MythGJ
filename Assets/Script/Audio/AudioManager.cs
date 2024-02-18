using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioClip menuBGM;
    public AudioClip gameBGM;
    private AudioSource bgmSource;
    private enum GameState { Menu, InGame };
    private GameState currentState;

    private void Awake()
    {
        //if (instance == null)
        //{
        //    instance = this;
        //    DontDestroyOnLoad(gameObject);
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}

        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            CheckGameState(GameState.InGame);
        } 
        else 
        {
            CheckGameState(GameState.Menu);
        }
    }

    private void Start()
    {
        bgmSource = GetComponent<AudioSource>();
        //ChangeBGM();
    }

    private void Update()
    {
        ChangeBGM();
    }

    private void CheckGameState(GameState newState)
    {
        currentState = newState;
        switch (currentState)
        {
            case GameState.Menu:
                break;
            case GameState.InGame:
                break;
        }
    }

    private void ChangeBGM()
    {
        switch (currentState)
        {
            case GameState.Menu:
                PlayBGM(menuBGM); 
                break;
            case GameState.InGame:
                PlayBGM(gameBGM);
                break;
        }
    }
    public void PlayBGM(AudioClip clip)
    {
        if (bgmSource.clip != clip)
        {
            bgmSource.clip = clip;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    //private void PlayBGM(AudioClip clip)
    //{
    //    if (audioSource.clip != clip)
    //    {
    //        audioSource.clip = clip;
    //        audioSource.loop = true;
    //        audioSource.Play();
    //    }
    //}

    //void Start()
    //{
    //    SceneManager.sceneLoaded += OnSceneLoaded;
    //}

    //void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    switch (scene.name)
    //    {
    //        case "MainMenu": // Replace with your actual main menu scene name
    //            PlayBGM(mainMenuBGM);
    //            break;
    //        case "Game": // Replace with your actual game scene name
    //            PlayBGM(gameBGM);
    //            break;
    //            // Add more cases as needed for other scenes/BGMs
    //    }
    //}


    //public void PlaySFX(AudioClip clip)
    //{
    //    sfxSource.PlayOneShot(clip); // Plays the SFX without interrupting any currently playing SFX
    //}

    //void OnDestroy()
    //{
    //    SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe to avoid memory leaks
    //}
}
