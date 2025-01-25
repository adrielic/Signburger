using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    public TMP_Text scoreTxt, failsTxt;

    void Update()
    {
        scoreTxt.text = GameManager.score.ToString();
        failsTxt.text = GameManager.fails.ToString();
    }
}
