using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using static AudioManager;

public class CollectManager : MonoBehaviour
{
    [Header("Score System")]
    [SerializeField] AudioClip collectClip;
    [HideInInspector] public int score = 0;

    private float scoreIncreaseTimer = 0f;
    private bool canIncreaseScore;

    //[SerializeField] uiScript uiScript;
    void Awake()
    {
        score = 0;

        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            canIncreaseScore = true;
        }
        else
        {
            canIncreaseScore= false;
        }
    }

    void Update()
    {
        if (canIncreaseScore)
        {
            IncreaseScoreOverTime();
        }
        Debug.Log("Score : "  + score);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Collectible"))
        {
            score += 10; // Increment the score
            AudioManager.instance.PlaySFXClip(collectClip);

            Destroy(other.gameObject); // Remove the collectible from the scene
        }
    }
    private void IncreaseScoreOverTime()
    {
        scoreIncreaseTimer += Time.deltaTime;

        if (scoreIncreaseTimer >= 2f) // 2 seconds have passed
        {
            score += 10; // Increase the score by 10
            scoreIncreaseTimer = 0f; // Reset the timer

            // Update UI here, if necessary
            // uiScript.UpdateScore(score);
        }
    }
}
