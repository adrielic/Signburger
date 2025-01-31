using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public Animator animator;
    
    public void TryAgain()
    {
        StartCoroutine(LoadScene("Game"));
    }

    public void ReturnToMenu()
    {
        StartCoroutine(LoadScene("MainMenu"));
    }

    IEnumerator LoadScene(string scene)
    {
        animator.SetTrigger("Return");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(scene);
    }
}