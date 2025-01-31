using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateControl : MonoBehaviour
{
    public static Stack<string> plateStack = new Stack<string>();

    static Animator animator;
    public GameObject plate;
    public Transform receiver;
    Vector3 startPosition;
    bool isDragging;

    public static bool isReadyToServe, wasServed;
    public static int positionOnPile;

    void Start()
    {
        animator = GetComponent<Animator>();
        startPosition = plate.transform.position;
        isReadyToServe = false;
        wasServed = false;
    }

    void Update()
    {
        if (plate != null)
            positionOnPile = plate.transform.childCount + 1;

        if (isReadyToServe)
        {
            DragAndDrop();
        }
    }

    public static void AddToStack(string ingredientId)
    {
        if (ingredientId == "top")
        {
            animator.SetTrigger("Ready");
            isReadyToServe = true;
            Debug.Log("isReadyToServe = " + isReadyToServe);
        }
        else
            plateStack.Push(ingredientId);
    }

    public void Clear()
    {
        isReadyToServe = false;
        plateStack.Clear();

        foreach (Transform child in plate.transform)
        {
            if (!child.CompareTag("Ignore"))
                Destroy(child.gameObject);
        }

        ResetPosition();
    }

    void DragAndDrop()
    {
        if (isDragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            plate.transform.position = mousePosition;
        }

        if (Input.GetMouseButtonDown(0) && !wasServed)
        {
            if (IsMouseOverPlate())
            {
                isDragging = true;
            }
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            if (OrderCycle.theresIsAnOrder && IsOverReceiver())
            {
                HandToReceiver();
            }
            else
            {
                ResetPosition();
            }

            isDragging = false;
        }
    }

    bool IsMouseOverPlate()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        return hit.collider != null && hit.collider.gameObject == plate;
    }

    bool IsOverReceiver()
    {
        Collider2D targetCollider = receiver.GetComponent<Collider2D>();
        return targetCollider.OverlapPoint(plate.transform.position);
    }

    void HandToReceiver()
    {
        wasServed = true;
        isReadyToServe = false;

        foreach (Transform child in plate.transform)
        {
            if (!child.CompareTag("Ignore"))
                Destroy(child.gameObject);
        }

        ResetPosition();
        Debug.Log("customer served");
    }

    void ResetPosition()
    {
        plate.transform.position = startPosition;
        animator.SetTrigger("Reset");
    }
}