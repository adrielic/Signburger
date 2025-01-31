using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public static Animator animator;
    public TMP_Text scoreTxt;
    public Image timerImg;
    public GameObject[] failObjects;
    public OrderCycle orderCycle;

    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        GameManager.OnScoreChanged += UpdateScoreDisplay;
        GameManager.OnFailuresChanged += UpdateFailureDisplay;

        foreach (GameObject fail in failObjects)
            fail.SetActive(false);
    }

    private void OnDisable()
    {
        GameManager.OnScoreChanged -= UpdateScoreDisplay;
        GameManager.OnFailuresChanged -= UpdateFailureDisplay;
    }

    void Update()
    {
        timerImg.fillAmount = orderCycle.countdownTime / 15f;
    }

    private void UpdateScoreDisplay(int newScore)
    {
        scoreTxt.text = GameManager.score.ToString("D6");
    }

    private void UpdateFailureDisplay(int newFailure)
    {
        failObjects[GameManager.fails - 1].SetActive(true);
        Debug.Log("fails = " + GameManager.fails);
    }

    public static void ShowTimer(bool isThereACustomer)
    {
        if (isThereACustomer)
            animator.SetTrigger("TimerOn");
        else
            animator.SetTrigger("TimerOff");
    }
}