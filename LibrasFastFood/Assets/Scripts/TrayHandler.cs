using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrayHandler : MonoBehaviour
{
    public static Stack<string> trayStack = new Stack<string>();

    Vector3 startPosition;
    Transform receiver;
    bool isReadyToServe;
    bool isDragging;
    
    public static bool WasServed;

    void Start()
    {
        startPosition = transform.position;
        receiver = GameObject.FindGameObjectWithTag("Receiver").transform;
        isReadyToServe = false;
        WasServed = false;
    }

    void Update()
    {
        if (TopHandler.isReadyToStack)
        {
            AddToStack();
            isReadyToServe = true;
            TopHandler.isReadyToStack = false;

            foreach (string ingredient in trayStack)
                Debug.Log(ingredient + " on stack");
        }

        if (trayStack.Count > 0 && isReadyToServe == true)
        {
            DragAndDrop();
        }
    }

    void AddToStack()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;

            if (child.tag == "Stackable")
                trayStack.Push(child.GetComponent<IngredientHandler>().id);
        }

        Debug.Log("stack filled");
    }

    public void ClearTray()
    {
        isReadyToServe = false;
        IngredientHandler.ingredientsOnTray = 0;
        List<Transform> children = new List<Transform>();

        foreach (Transform child in transform)
        {
            children.Add(child);
        }

        foreach (Transform child in children)
        {
            if (child.tag == "Stackable")
            {
                child.GetComponent<IngredientHandler>().Release();
                child.SetParent(null);
            }

            if (child.tag == "NonStackable")
            {
                child.GetComponent<TopHandler>().Release();
                child.SetParent(null);
            }
        }

        if (trayStack.Count > 0)
        {
            trayStack.Clear();
        }
    }

    void DragAndDrop()
    {
        if (isDragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            transform.position = mousePosition;
        }

        if (Input.GetMouseButtonDown(0) && !WasServed)
        {
            if (IsMouseOverTray())
            {
                isDragging = true;
            }
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            if (IsOverReceiver())
            {
                StartCoroutine(HandToReceiver());
            }
            else
            {
                ResetPosition();
            }

            isDragging = false;
        }
    }

    bool IsMouseOverTray()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        return hit.collider != null && hit.collider.gameObject == gameObject;
    }

    bool IsOverReceiver()
    {
        Collider2D targetCollider = receiver.GetComponent<Collider2D>();
        return targetCollider.OverlapPoint(transform.position);
    }

    IEnumerator HandToReceiver()
    {
        WasServed = true;
        Debug.Log("order served");

        yield return new WaitForSeconds(0.1f);
        ClearTray();
        ResetPosition();
    }

    void ResetPosition()
    {
        transform.position = startPosition;
    }
}
