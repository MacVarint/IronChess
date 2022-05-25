using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTiles : MonoBehaviour
{
    public GameObject whiteTile;
    public GameObject blackTile;
    private Transform parent;
    private float scaling = 2f;
    private float startPosition = 3.5f;
    void Start()
    {
        ConstructTiles();
        StartPositions();
    }
    public void ConstructTiles()
    {
        parent = this.transform;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                GameObject temp;
                if ((i + j) % 2 == 0)
                {
                    temp = Instantiate(blackTile, new Vector3((i - startPosition) * scaling, (j - startPosition) * scaling, 0), Quaternion.identity, parent);
                }
                else
                {
                    temp = Instantiate(whiteTile, new Vector3((i - startPosition) * scaling, (j - startPosition) * scaling, 0), Quaternion.identity, parent);
                }
                Tile tempTile = temp.AddComponent<Tile>();
                tempTile.imageHolder = temp.transform.GetChild(0).GetComponent<SpriteRenderer>();
                GridManager.Board[i, j] = tempTile;
                temp.name = "Tile:" + i + "," + j;
            }
        }
    }
    public void StartPositions()
    {
        GridManager.Board[0, 0].current = Tile.chessPiece.TowerWhite;
        GridManager.Board[1, 0].current = Tile.chessPiece.KnightWhite;
        GridManager.Board[2, 0].current = Tile.chessPiece.BishopWhite;
        GridManager.Board[3, 0].current = Tile.chessPiece.QueenWhite;
        GridManager.Board[4, 0].current = Tile.chessPiece.KingWhite;
        GridManager.Board[5, 0].current = Tile.chessPiece.BishopBlack;
        GridManager.Board[6, 0].current = Tile.chessPiece.KnightWhite;
        GridManager.Board[7, 0].current = Tile.chessPiece.TowerWhite;

        for (int i = 0; i < 8; i++)
        {
            GridManager.Board[i, 1].current = Tile.chessPiece.PawnWhite;
        }
    }
}
