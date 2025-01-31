using UnityEngine;

public class Ingredients : MonoBehaviour
{
    public string id;

    Transform target;
    bool isDragging;
    bool isPlaced;
    float offset = 0.2f;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Target").transform;
        isPlaced = false;
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

        if (Input.GetMouseButtonDown(0) && !isPlaced)
        {
            if (IsMouseOverIngredient())
            {
                isDragging = true;
            }
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            if (IsOverTarget() && !PlateControl.isReadyToServe)
            {
                PlaceOnTarget();
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
        Collider2D targetCollider = target.GetComponent<Collider2D>();
        return targetCollider.OverlapPoint(transform.position);
    }

    void PlaceOnTarget()
    {
        transform.position = new Vector3(target.position.x, target.position.y + (PlateControl.positionOnPile * offset), 0);
        gameObject.transform.SetParent(target.transform);
        isPlaced = true;
        PlateControl.AddToStack(id);

        Debug.Log(id + " placed");
    }

    public void Release()
    {
        Destroy(gameObject);
        isPlaced = false;
    }
}