using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverHandler : MonoBehaviour
{

    [SerializeField] GameObject WinstateUI;
    [SerializeField] GameObject ScoreFrame;
    [SerializeField] GameObject HighScoreFrame;
    [SerializeField] GameObject TimeFrame;
    [SerializeField] GameObject InGameUI;
    [SerializeField] GameObject PlayAgainButton;

    [SerializeField] TextMeshProUGUI Score;
    [SerializeField] TextMeshProUGUI HighScore;
    [SerializeField] TextMeshProUGUI Time;

    [SerializeField] TextMeshProUGUI InGame_Passengers;
    [SerializeField] TextMeshProUGUI InGame_Time;
    [SerializeField] TextMeshProUGUI InGame_Score;

    [HideInInspector] AudioManager audioManager;

    private float lerpSpeed = 1f; // The speed of the interpolation

    private float scaleDuration = 1f; // Duration for scaling up/down
    private float maxScale = 1.5f; // Maximum scale factor
    private Vector3 originalScale; // Original scale of the button

    PlayerController playerController1;

    void Start()
    {
        audioManager = GameObject.Find("Manager").GetComponent<AudioManager>();
        playerController1 = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        InGame_Passengers.text = $"Passengers: {playerController1.Health}";
        InGame_Time.text = $"Time: {(int)playerController1.timeAlive}";
        InGame_Score.text = $"Points: {playerController1.Points}";
    }

public IEnumerator GameOver(PlayerController playerController)
    {
        WinstateUI.SetActive(true);
        InGameUI.SetActive(false);

        originalScale = PlayAgainButton.transform.localScale;

        bool setNewHighScore = false;

        int points = playerController.Points;
        int highscore = playerController.Highscore;
        int timeAlive = Mathf.RoundToInt(playerController.timeAlive);

        if (points > highscore) { setNewHighScore = true; }

        playerController.Highscore = points;

        yield return StartCoroutine(LerpIntText(Score, points));

        yield return StartCoroutine(LerpIntText(Time, timeAlive));
        Time.text = Time.text + "s";

        if (setNewHighScore)
        {
            // Show highscore
            HighScoreFrame.SetActive(true);
            yield return StartCoroutine(LerpIntText(HighScore, points));
            HighScore.text = HighScore.text + "!!";
        }

        PlayAgainButton.SetActive(true);
        StartCoroutine(ScaleButtonLoop());
    }

    // Coroutine to lerp an integer value for a TextMeshProUGUI text
    public IEnumerator LerpIntText(TextMeshProUGUI textField, int targetValue)
    {

        // Initialize the currentValue to the starting value of the text field
        int.TryParse(textField.text, out int currentValue);

        int howManyTimes = 100;
        float waitTime = .001f;

        // Loop until the current value reaches the target value
        for (int i = 0; i < howManyTimes; i++)
        {
            // Smoothly interpolate the current value towards the target value
            currentValue = Mathf.RoundToInt(Mathf.Lerp(currentValue, targetValue, (float)i/howManyTimes));

            // Update the text field with the interpolated value
            textField.text = currentValue.ToString();

            yield return new WaitForSeconds(waitTime);
        }
           
        
    }

    IEnumerator ScaleButtonLoop()
    {
        while (true)
        {
            yield return ScaleButton(Vector3.one * maxScale); // Scale up
            yield return ScaleButton(originalScale); // Scale down
        }
    }

    IEnumerator ScaleButton(Vector3 targetScale)
    {
        float timeElapsed = 0f;
        Vector3 initialScale = PlayAgainButton.transform.localScale;

        while (timeElapsed < scaleDuration)
        {
            PlayAgainButton.transform.localScale = Vector3.Lerp(initialScale, targetScale, timeElapsed / scaleDuration);
            timeElapsed += .01f ;
            yield return new WaitForSeconds(.01f);
        }

        PlayAgainButton.transform.localScale = targetScale;
    }

    public void ResetScene()
    {
        // Reload the current scene by its name
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        audioManager.CheckGameState(AudioManager.GameState.InGame);
    }
}
