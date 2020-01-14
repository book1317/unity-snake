using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    public enum ItemType
    {
        Empty, Food, Wall, Player
    }
    public int x;
    public int y;
    public ItemType itemType = ItemType.Empty;

    public Grid(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public Grid(int x, int y, ItemType itemType)
    {
        this.x = x;
        this.y = y;
        this.itemType = itemType;
    }

    public void SetGridType(ItemType itemType)
    {
        this.itemType = itemType;
    }
}
