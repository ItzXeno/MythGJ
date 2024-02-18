using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class CollectManager : MonoBehaviour
{
    [Header("Score System")]
    [SerializeField] int score = 0;
    [SerializeField] AudioClip collectClip;

    //[SerializeField] uiScript uiScript;
    void Awake()
    {

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

    void Update()
    {

    }
}
