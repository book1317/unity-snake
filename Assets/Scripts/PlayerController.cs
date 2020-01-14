using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public int startX = 15;
    public int startY = 10;

    public int x = 15;
    public int y = 10;
    public float lenght = 0.5f;
    private string currentDirection = "Idle";
    public string lastDirection = "Idle";

    public float walkTime = 0.05f;
    public float walkCounter;

    public LevelController theLevel;
    public GameObject bodyPrefab;

    public List<BodyController> allBody;
    public Transform snakeParentObject;
    private int colorLenght = 0;
    public Slider timeSlider;
    public Text speedText;

    void Start()
    {
        allBody = new List<BodyController>();
        walkCounter = 0;
        theLevel = FindObjectOfType<LevelController>();
        UpdateSpeedText();
    }


    void Update()
    {
        if (Input.anyKey)
        {
            if (Input.GetButtonDown("Left") && !ToggleDirection("Left").Equals(lastDirection))
                currentDirection = "Left";
            else if (Input.GetButtonDown("Right") && !ToggleDirection("Right").Equals(lastDirection))
                currentDirection = "Right";
            else if (Input.GetButtonDown("Up") && !ToggleDirection("Up").Equals(lastDirection))
                currentDirection = "Up";
            else if (Input.GetButtonDown("Down") && !ToggleDirection("Down").Equals(lastDirection))
                currentDirection = "Down";
        }

        if (theLevel.currentGameState == LevelController.gameState.Playing)
        {
            if (walkCounter >= 0)
                walkCounter -= Time.fixedDeltaTime;
            else
                Walk(currentDirection);
        }
        else if (theLevel.currentGameState == LevelController.gameState.MainMenu)
        {
            if (Input.anyKey)
                if (Input.GetButtonDown("Left") || Input.GetButtonDown("Right") || Input.GetButtonDown("Up") || Input.GetButtonDown("Down"))
                    theLevel.currentGameState = LevelController.gameState.Playing;
        }
        else if (theLevel.currentGameState == LevelController.gameState.Death)
        {
            if (Input.anyKey)
                if (Input.GetButtonDown("Left") || Input.GetButtonDown("Right") || Input.GetButtonDown("Up") || Input.GetButtonDown("Down"))
                {
                    Reset();
                    theLevel.currentGameState = LevelController.gameState.MainMenu;
                }
        }

        if (timeSlider.value != walkTime)
        {
            walkTime = timeSlider.value;
            UpdateSpeedText();
        }
    }

    void UpdateSpeedText()
    {
        speedText.text = "Speed : " + (int)((1.0f - walkTime) / 0.99f * 100) + "%";
    }

    void Reset()
    {
        x = startX;
        y = startY;
        transform.position = new Vector3(0, 0, transform.position.z);
        theLevel.score = 0;
        colorLenght = 0;
        RemoveAllBody();
        theLevel.theFood.GetEat();
        theLevel.Restart();
    }

    void RemoveAllBody()
    {
        for (int i = 0; i < allBody.Count; i++)
        {
            theLevel.GetGrid(allBody[i].x, allBody[i].y).itemType = Grid.ItemType.Empty;
            Destroy(allBody[i].gameObject);
        }
        allBody = new List<BodyController>();
    }

    public void Walk(string direction)
    {
        Grid lastGrid = theLevel.GetGrid(x, y);
        int lastX = x;
        int lastY = y;
        lastGrid.SetGridType(Grid.ItemType.Empty);
        Vector3 lastPosition = transform.position;
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
        WalkBody();
        lastDirection = direction;
        Grid currentGrid = theLevel.GetGrid(x, y);
        CheckGrid(currentGrid, lastGrid, lastPosition, lastX, lastY);
        walkCounter = walkTime;
    }

    void WalkBody()
    {
        string prevBodyMove = "Idle";
        string lastMove = lastDirection;
        for (int i = 0; i < allBody.Count; i++)
        {
            if (i == 0)
            {
                prevBodyMove = allBody[i].currentDirection;
                allBody[i].Walk(lastMove);
            }
            else
            {
                lastMove = prevBodyMove;
                prevBodyMove = allBody[i].currentDirection;
                allBody[i].Walk(lastMove);
            }
        }
    }

    void SpawnBody(Grid lastGrid, Vector3 position, int lastX, int lastY)
    {
        BodyController theBody;
        if (allBody.Count <= 0)
            theBody = Instantiate(bodyPrefab, position, Quaternion.identity).GetComponent<BodyController>();
        else
            theBody = Instantiate(bodyPrefab, allBody[allBody.Count - 1].lastPosition, Quaternion.identity).GetComponent<BodyController>();

        if (allBody.Count <= 0)
        {
            theBody.x = lastX;
            theBody.y = lastY;
        }
        else
        {
            theBody.x = allBody[allBody.Count - 1].lastX;
            theBody.y = allBody[allBody.Count - 1].lastY;
        }
        RandomColorBody(theBody);
        allBody.Add(theBody);
        lastGrid.itemType = Grid.ItemType.Player;
        theBody.transform.SetParent(snakeParentObject);
    }

    void RandomColorBody(BodyController theBody)
    {
        SpriteRenderer theBodySprite = theBody.GetComponent<SpriteRenderer>();
        if (colorLenght < 50)
        {
            colorLenght += 5;
        }
        theBodySprite.color = new Color((theBodySprite.color.r * 255 + colorLenght) / 255, (theBodySprite.color.g * 255 + colorLenght - 5) / 255, (theBodySprite.color.b * 255 + colorLenght) / 255);

    }

    void CheckGrid(Grid currentGrid, Grid lastGrid, Vector3 position, int lastX, int lastY)
    {
        if (currentGrid.itemType == Grid.ItemType.Empty)
        {
            currentGrid.SetGridType(Grid.ItemType.Player);
        }
        else if (currentGrid.itemType == Grid.ItemType.Wall || currentGrid.itemType == Grid.ItemType.Player)
        {
            Death();
        }
        else if (currentGrid.itemType == Grid.ItemType.Food)
        {
            theLevel.IncreaseScore(1);
            SpawnBody(lastGrid, position, lastX, lastY);
            theLevel.theFood.GetEat();
        }
    }

    void Death()
    {
        theLevel.currentGameState = LevelController.gameState.Death;
        currentDirection = "Idle";
        theLevel.CheckHightScore();
        theLevel.GameOver();
    }

    public string ToggleDirection(string direction)
    {
        switch (direction)
        {
            case "Left":
                return "Right";
            case "Right":
                return "Left";
            case "Up":
                return "Down";
            case "Down":
                return "Up";
            default:
                return "Idle";
        }
    }

}
