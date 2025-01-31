using UnityEngine;

public class IngredientPiles : MonoBehaviour
{
    public GameObject ingredientPrefab;

    void OnMouseDown()
    {
        if (gameObject.CompareTag("Source"))
        {
            GameObject newIngredient = Instantiate(ingredientPrefab, transform.position, Quaternion.identity);
            Ingredients ingredients = newIngredient.GetComponent<Ingredients>();

            if (ingredients != null)
            {
                ingredients.StartFollowingMouse();
            }
        }
    }
}