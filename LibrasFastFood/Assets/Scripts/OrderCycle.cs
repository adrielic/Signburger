using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderCycle : MonoBehaviour
{
    Stack<string> orderStack = new Stack<string>();
    string[] ingredientsArray = { "burger", "cheese", "tomato", "lettuce", "ham" };

    float countdownTime = 15f;

    void Start()
    {
        NextOrder();
    }

    void Update()
    {
        if (TrayHandler.WasServed)
        {
            CheckOrder();
            NextOrder();
            TrayHandler.WasServed = false;
        }

        if (countdownTime > 0)
        {
            countdownTime -= Time.deltaTime;
        }
        else
        {
            GameManager.fails++;
            NextOrder();
        }
    }

    void NextOrder()//transformar em coroutine
    {
        if (orderStack.Count > 0) 
            orderStack.Clear();

        int orderSize = Random.Range(2, ingredientsArray.Length + 1);

        for (int i = 0; i < orderSize; i++)
        {
            int rIngredient = Random.Range(0, ingredientsArray.Length);

            if (orderStack.Contains(ingredientsArray[rIngredient]))
                rIngredient = Random.Range(0, ingredientsArray.Length);
            else
                orderStack.Push(ingredientsArray[rIngredient]);
        }

        foreach (string ingredient in orderStack)
        {
            Debug.Log("requested: " + ingredient);
        }

        countdownTime = 15f;
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

    void CheckOrder()
    {
        if (IsSameStack(orderStack, TrayHandler.trayStack))
        {
            GameManager.score += (int)countdownTime;
            Debug.Log("ok");
        }
        else
        {
            GameManager.fails++;
            Debug.Log("missed");
        }
    }
}
