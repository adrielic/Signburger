using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    public Animator animator;
    public TMP_Text highScoreTxt;

    void Start()
    {
        GameManager.highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreTxt.text = "Melhor pontuação: " + GameManager.highScore;
    }

    public void Play()
    {
        animator.SetTrigger("Start");
        StartCoroutine(LoadScene("Game"));
    }

    public void Tutorial()
    {
        animator.SetTrigger("Start");
        StartCoroutine(LoadScene("Tutorial"));
    }

    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator LoadScene(string scene)
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(scene);
    }
}