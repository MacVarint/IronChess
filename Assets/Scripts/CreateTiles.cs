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
        //Bottom
        GridManager.Board[0, 0].current = Tile.chessPiece.TowerWhite;
        GridManager.Board[1, 0].current = Tile.chessPiece.KnightWhite;
        GridManager.Board[2, 0].current = Tile.chessPiece.BishopWhite;
        GridManager.Board[3, 0].current = Tile.chessPiece.QueenWhite;
        GridManager.Board[4, 0].current = Tile.chessPiece.KingWhite;
        GridManager.Board[5, 0].current = Tile.chessPiece.BishopWhite;
        GridManager.Board[6, 0].current = Tile.chessPiece.KnightWhite;
        GridManager.Board[7, 0].current = Tile.chessPiece.TowerWhite;

        for (int i = 0; i < 8; i++)
        {
            GridManager.Board[i, 1].current = Tile.chessPiece.PawnWhite;
        }
        //Top
        GridManager.Board[0, 7].current = Tile.chessPiece.TowerBlack;
        GridManager.Board[0, 7].imageHolder.transform.rotation = Quaternion.Euler(0, 0, 180);
        GridManager.Board[1, 7].current = Tile.chessPiece.KnightBlack;
        GridManager.Board[1, 7].imageHolder.transform.rotation = Quaternion.Euler(0, 0, 180);
        GridManager.Board[2, 7].current = Tile.chessPiece.BishopBlack;
        GridManager.Board[2, 7].imageHolder.transform.rotation = Quaternion.Euler(0, 0, 180);
        GridManager.Board[3, 7].current = Tile.chessPiece.QueenBlack;
        GridManager.Board[3, 7].imageHolder.transform.rotation = Quaternion.Euler(0, 0, 180);
        GridManager.Board[4, 7].current = Tile.chessPiece.KingBlack;
        GridManager.Board[4, 7].imageHolder.transform.rotation = Quaternion.Euler(0, 0, 180);
        GridManager.Board[5, 7].current = Tile.chessPiece.BishopBlack;
        GridManager.Board[5, 7].imageHolder.transform.rotation = Quaternion.Euler(0, 0, 180);
        GridManager.Board[6, 7].current = Tile.chessPiece.KnightBlack;
        GridManager.Board[6, 7].imageHolder.transform.rotation = Quaternion.Euler(0, 0, 180);
        GridManager.Board[7, 7].current = Tile.chessPiece.TowerBlack;
        GridManager.Board[7, 7].imageHolder.transform.rotation = Quaternion.Euler(0, 0, 180);

        for (int i = 0; i < 8; i++)
        {
            GridManager.Board[i, 6].current = Tile.chessPiece.PawnBlack;
            GridManager.Board[i, 6].imageHolder.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
    }
}
