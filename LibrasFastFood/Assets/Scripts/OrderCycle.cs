using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderCycle : MonoBehaviour
{
    Stack<string> orderStack = new Stack<string>();
    string[] ingredientsArray = { "burger", "cheese", "tomato", "lettuce", "ham" };

    public static bool theresIsAnOrder;
    [HideInInspector] public float countdownTime = 15f;

    public GameObject[] customerPrefabs;
    GameObject currentCustomer;
    CustomerBehaviour customerBehaviour;
    int customerCycle;

    public GameObject signController;
    Animator trayAnimator, signAnimator;

    void Start()
    {
        signAnimator = signController.GetComponent<Animator>();
        trayAnimator = GetComponent<Animator>();
        customerCycle = Random.Range(0, customerPrefabs.Length);
        theresIsAnOrder = false;
        StartCoroutine(NextOrder());
    }

    void Update()
    {
        if (PlateControl.wasServed)
        {
            if (orderStack.Count > 0)
            {
                StartCoroutine(CheckOrder(true));
                PlateControl.wasServed = false;
            }
        }

        if (customerBehaviour != null)
        {
            if (customerBehaviour.isWaiting)
            {
                if (countdownTime > 0)
                {
                    countdownTime -= Time.deltaTime;
                }
                else
                {
                    StartCoroutine(CheckOrder(false));
                }
            }
        }
    }

    IEnumerator NextOrder()
    {
        yield return new WaitForSeconds(1);
        InstantiateCustomer();

        yield return new WaitForSeconds(1);
        List<string> availableIngredients = new List<string>(ingredientsArray);
        int orderSize = Random.Range(2, availableIngredients.Count + 1);

        for (int i = 0; i < orderSize; i++)
        {
            int rIngredient = Random.Range(0, availableIngredients.Count);
            string selectedIngredient = availableIngredients[rIngredient];
            availableIngredients.RemoveAt(rIngredient);
            orderStack.Push(selectedIngredient);
            signAnimator.SetInteger("Index", System.Array.IndexOf(ingredientsArray, selectedIngredient));

            yield return new WaitForSeconds(1.5f);
        }

        yield return new WaitForSeconds(1);
        signAnimator.SetInteger("Index", -1);
        customerBehaviour.isWaiting = true;
        theresIsAnOrder = true;
        HUDManager.ShowTimer(true);
        countdownTime = 15f;

        foreach (string ingredient in orderStack)
        {
            Debug.Log("requested: " + ingredient);
        }
    }

    bool IsSameStack(Stack<string> stack1, Stack<string> stack2)
    {
        bool flag = true;

        if (stack1.Count != stack2.Count)
        {
            flag = false;
        }
        else
        {
            while (stack1.Count != 0)
            {
                if (stack1.Peek() == stack2.Peek())
                {
                    stack1.Pop();
                    stack2.Pop();
                }
                else
                {
                    flag = false;
                    break;
                }
            }
        }

        return flag;
    }

    IEnumerator CheckOrder(bool wasServed)
    {
        trayAnimator.SetTrigger("Serve");
        HUDManager.ShowTimer(false);
        customerBehaviour.isWaiting = false;
        theresIsAnOrder = false;
        float waitTime;

        if (wasServed && IsSameStack(orderStack, PlateControl.plateStack))
        {
            GameManager.AddScore(10 * (int)countdownTime);
            customerBehaviour.RateOrder("Like");
            waitTime = 4.8f;
            Debug.Log("ok");
        }
        else
        {
            GameManager.AddFailure(1);
            customerBehaviour.RateOrder("Dislike");
            waitTime = 2.75f;
            Debug.Log("failed");
        }

        yield return new WaitForSeconds(waitTime);
        customerBehaviour.Leave();
        waitTime = 1.3f;

        yield return new WaitForSeconds(waitTime);
        orderStack.Clear();
        PlateControl.plateStack.Clear();
        StartCoroutine(NextOrder());
    }

    void InstantiateCustomer()
    {
        currentCustomer = Instantiate(customerPrefabs[customerCycle], transform.position, transform.rotation);
        customerBehaviour = currentCustomer.GetComponent<CustomerBehaviour>();
        customerCycle++;

        if (customerCycle >= customerPrefabs.Length)
            customerCycle = 0;
    }
}