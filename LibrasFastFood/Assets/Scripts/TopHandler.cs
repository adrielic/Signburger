using UnityEngine;

public class TopHandler : MonoBehaviour
{
    Transform tray;
    bool isDragging;
    public static bool isReadyToStack;

    void Start()
    {
        tray = GameObject.FindGameObjectWithTag("Target").transform;
        isReadyToStack = false;
    }

    void Update()
    {
        DragAndDrop();
    }

    public void StartFollowingMouse()
    {
        isDragging = true;
    }

    void DragAndDrop()
    {
        if (isDragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            transform.position = mousePosition;
        }

        if (Input.GetMouseButtonDown(0) && !isReadyToStack)
        {
            if (IsMouseOverIngredient())
            {
                isDragging = true;
            }
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            if (IsOverTarget())
            {
                PlaceOnTray();
            }
            else
            {
                Release();
            }

            isDragging = false;
        }
    }

    bool IsMouseOverIngredient()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        return hit.collider != null && hit.collider.gameObject == gameObject;
    }

    bool IsOverTarget()
    {
        Collider2D targetCollider = tray.GetComponent<Collider2D>();
        return targetCollider.OverlapPoint(transform.position);
    }

    void PlaceOnTray()
    {
        IngredientHandler.ingredientsOnTray++;
        transform.position = new Vector3(tray.position.x, tray.position.y + (IngredientHandler.ingredientsOnTray * 0.2f), 0);
        gameObject.transform.SetParent(tray.transform);
        isReadyToStack = true;
        Debug.Log("top placed");
    }

    public void Release()
    {
        Destroy(gameObject);
        isReadyToStack = false;
    }
}
