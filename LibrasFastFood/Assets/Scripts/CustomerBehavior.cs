using UnityEngine;

public class CustomerBehaviour : MonoBehaviour
{
    public Animator animator;
    public bool isWaiting;

    public void RateOrder(string rating)
    {
        animator.SetTrigger(rating);
    }

    public void Leave()
    {
        Destroy(gameObject, 1.3f);
    }
}