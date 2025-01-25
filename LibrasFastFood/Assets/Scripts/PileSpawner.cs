using Mono.Cecil;
using UnityEngine;

public class PileSpawner : MonoBehaviour
{

    public GameObject ingredientPrefab;

    void OnMouseDown()
    {
        if (gameObject.CompareTag("Getter"))
        {
            GameObject newIngredient = Instantiate(ingredientPrefab, transform.position, Quaternion.identity);

            if (ingredientPrefab.name == "Top")
            {
                TopHandler handler = newIngredient.GetComponent<TopHandler>();

                if (handler != null)
                {
                    handler.StartFollowingMouse();
                }
            }
            else
            {
                IngredientHandler handler = newIngredient.GetComponent<IngredientHandler>();

                if (handler != null)
                {
                    handler.StartFollowingMouse();
                }
            }
        }
    }
}
