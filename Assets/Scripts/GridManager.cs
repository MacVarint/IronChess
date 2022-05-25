using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    static public Tile[,] Board = new Tile[8, 8];
    public Sprite[] sprites;
    void Update()
    {
        AssignSprites();
    }
    public void AssignSprites()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                for (int k = 0; k < Enum.GetValues(typeof(Tile.chessPiece)).Length; k++)
                {
                    if (Board[i, j].current == (Tile.chessPiece)k)
                    {
                        Board[i, j].imageHolder.sprite = sprites[k];
                    }
                }
            }
        }
    }
}
