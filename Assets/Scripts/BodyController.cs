using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    public int x, y;
    public int lastX;
    public int lastY;
    public float lenght = 0.5f;
    public LevelController theLevel;
    public string currentDirection = "Idle";
    public Vector3 lastPosition;

    void Start()
    {
        theLevel = FindObjectOfType<LevelController>();
    }

    public void Walk(string direction)
    {
        lastX = x;
        lastY = y;
        currentDirection = direction;
        lastPosition = transform.position;
        Grid currentGrid = theLevel.GetGrid(x, y);
        currentGrid.SetGridType(Grid.ItemType.Empty);
        switch (direction)
        {
            case "Left":
                x -= 1;
                transform.position = new Vector3(transform.position.x - lenght, transform.position.y, transform.position.z);
                break;
            case "Right":
                x += 1;
                transform.position = new Vector3(transform.position.x + lenght, transform.position.y, transform.position.z);
                break;
            case "Up":
                y += 1;
                transform.position = new Vector3(transform.position.x, transform.position.y + lenght, transform.position.z);
                break;
            case "Down":
                y -= 1;
                transform.position = new Vector3(transform.position.x, transform.position.y - lenght, transform.position.z);
                break;
        }
        currentGrid = theLevel.GetGrid(x, y);
        currentGrid.SetGridType(Grid.ItemType.Player);
    }
}
