using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : MonoBehaviour
{
    int x, y;
    public LevelController theLevel;
    public GameObject foodPrefabs;
    public GameObject currentFoodObject;
    void Start()
    {
        theLevel = FindObjectOfType<LevelController>();
    }

    public void GetEat()
    {
        theLevel.GetGrid(x, y).itemType = Grid.ItemType.Empty;
        Destroy(currentFoodObject);
        SpawnFood();
    }

    public void SpawnFood()
    {
        Grid currentGrid;
        do
        {
            x = Random.Range(1, theLevel.GRID_WIDTH - 2);
            y = Random.Range(1, theLevel.GRID_HEIGHT - 2);
            currentGrid = theLevel.GetGrid(x, y);
        } while (currentGrid.itemType != Grid.ItemType.Empty);

        currentGrid.itemType = Grid.ItemType.Food;
        currentFoodObject = Instantiate(foodPrefabs, new Vector3(-7.5f + x * 0.5f, -5 + y * 0.5f, transform.position.z), Quaternion.identity);
        currentFoodObject.transform.SetParent(transform);
    }
}
