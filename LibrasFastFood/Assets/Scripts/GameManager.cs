using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject gameoverManager;

    public static int fails;
    public static int score, highScore;
    public static event Action<int> OnScoreChanged;
    public static event Action<int> OnFailuresChanged;
    public static bool gameIsOver;

    void OnEnable()
    {
        gameIsOver = false;
        fails = 0;
        score = 0;
    }

    void OnDisable()
    {
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }
    }

    void Update()
    {
        if (!gameIsOver && fails >= 3)
        {
            gameIsOver = true;
            StartCoroutine(EndGame());
        }
    }

    public static void AddScore(int value)
    {
        if (!gameIsOver)
        {
            score += value;
            OnScoreChanged?.Invoke(score);
        }
    }

    public static void AddFailure(int value)
    {
        fails += value;
        OnFailuresChanged?.Invoke(fails);
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(0.5f);
        gameoverManager.GetComponent<Animator>().SetTrigger("GameOver");
    }
}