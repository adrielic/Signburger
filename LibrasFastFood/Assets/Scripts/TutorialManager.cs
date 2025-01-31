using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] tutorialTxts;
    public GameObject tutorialUi, tutorialElements;
    
    int textCount = 0;

    void Start()
    {
        StartCoroutine(StartText());
    }

    IEnumerator StartText()
    {
        yield return new WaitForSeconds(1);
        NextText();
    }

    IEnumerator LoadScene()
    {
        tutorialUi.GetComponent<Animator>().SetTrigger("TutorialOver");
        tutorialElements.GetComponent<Animator>().SetTrigger("TutorialOver");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Game");
    }

    public void NextText()
    {
        if (textCount >= tutorialTxts.Length)
            StartCoroutine(LoadScene());
        else
        {
            foreach (GameObject txt in tutorialTxts)
            {
                txt.SetActive(false);
            }
            
            tutorialTxts[textCount].SetActive(true);
            textCount++;
        }
    }
}
