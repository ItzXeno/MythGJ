using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject playBut, credBut, quitBut, gTitle, credText, backBut;

    //new
    [SerializeField] private AudioClip buttonClip;

    [HideInInspector] AudioManager audioManager;

    private void Start()
    {
        credText.SetActive(false);
        backBut.SetActive(false);
        audioManager = GameObject.Find("Manager").GetComponent<AudioManager>();
    }
    public void Play()
    {    
        AudioManager.instance.PlaySFXClip(buttonClip);

        Destroy(AudioManager.instance.gameObject);

        SceneManager.LoadScene(1);
        audioManager.CheckGameState(AudioManager.GameState.InGame);
    }
    public void quitGame()
    {
        AudioManager.instance.PlaySFXClip(buttonClip);

        Application.Quit();
    }

    public void Credits()
    {
        AudioManager.instance.PlaySFXClip(buttonClip);

        playBut.SetActive(false);
        credBut.SetActive(false);
        quitBut.SetActive(false);
        gTitle.SetActive(false);
        credText.SetActive(true);
        backBut.SetActive(true);
    }

    public void Back()
    {
        AudioManager.instance.PlaySFXClip(buttonClip);

        playBut.SetActive(true);
        credBut.SetActive(true);
        quitBut.SetActive(true);
        gTitle.SetActive(true);
        credText.SetActive(false);
        backBut.SetActive(false);
    }
}
