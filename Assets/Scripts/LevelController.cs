using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public enum gameState
    {
        MainMenu, Playing, Death
    }

    public int score;
    public int hightScore;
    public Text scoreText;
    public Text hightScoreText;

    public gameState currentGameState = gameState.MainMenu;
    Grid[,] allGrid;
    public int GRID_WIDTH = 31;
    public int GRID_HEIGHT = 21;

    public FoodController theFood;
    public PlayerController thePlayer;
    public GameObject GameOverScreen;

    void Start()
    {
        allGrid = new Grid[GRID_WIDTH, GRID_HEIGHT];
        for (int x = 0; x < GRID_WIDTH; x++)
            for (int y = 0; y < GRID_HEIGHT; y++)
            {
                if (x == 0 || x == GRID_WIDTH - 1 || y == 0 || y == GRID_HEIGHT - 1)
                    allGrid[x, y] = new Grid(x, y, Grid.ItemType.Wall);
                else
                    allGrid[x, y] = new Grid(x, y);
            }

        theFood = FindObjectOfType<FoodController>();
        thePlayer = FindObjectOfType<PlayerController>();
        theFood.SpawnFood();
    }

    public Grid GetGrid(int x, int y)
    {
        return allGrid[x, y];
    }

    void UpdateScoreText(Text scoreText, bool isHightScore)
    {
        string prefixScore = "Score";
        if (isHightScore)
            prefixScore = "HScore";

        if (score < 1000)
        {
            if (score >= 100)
                scoreText.text = prefixScore + " : 00" + score;
            else
                scoreText.text = prefixScore + " : 000" + score;
        }
        else
        {
            if (score < 10000)
                scoreText.text = prefixScore + " : 0" + score;
            else
                scoreText.text = prefixScore + " : " + score;
        }
    }

    public void IncreaseScore(int addScore)
    {
        score += addScore;
        UpdateScoreText(scoreText, false);
    }

    public void CheckHightScore()
    {
        if (score > hightScore)
            UpdateScoreText(hightScoreText, true);
    }

    public void GameOver()
    {
        GameOverScreen.SetActive(true);
    }

    public void Restart()
    {
        GameOverScreen.SetActive(false);
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScoreText(scoreText, false);
    }
}
