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
    GridManager gridManager;
    void Start()
    {
        gridManager = this.GetComponent<GridManager>();
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
                tempTile.gridpos = new Vector2Int(i,j);
                GridManager.Board[i, j] = tempTile;
                temp.name = "Tile:" + i + "," + j;
            }
        }
    }
    public void StartPositions()
    {
        //Top
        GridManager.Board[0, 7].current = Tile.chessPiece.TowerOpponent;
        GridManager.Board[1, 7].current = Tile.chessPiece.KnightOpponent;
        GridManager.Board[2, 7].current = Tile.chessPiece.BishopOpponent;

        if (gridManager.startsAsWhite == true)
        {
            GridManager.Board[3, 7].current = Tile.chessPiece.QueenOpponent;
            GridManager.Board[4, 7].current = Tile.chessPiece.KingOpponent;
        }
        else
        {
            GridManager.Board[4, 7].current = Tile.chessPiece.QueenOpponent;
            GridManager.Board[3, 7].current = Tile.chessPiece.KingOpponent;
        }

        GridManager.Board[5, 7].current = Tile.chessPiece.BishopOpponent;
        GridManager.Board[6, 7].current = Tile.chessPiece.KnightOpponent;
        GridManager.Board[7, 7].current = Tile.chessPiece.TowerOpponent;

        for (int i = 0; i < 8; i++)
        {
            GridManager.Board[i, 6].current = Tile.chessPiece.PawnOpponent;
        }

        //Bottom
        GridManager.Board[0, 0].current = Tile.chessPiece.TowerPlayer;
        GridManager.Board[1, 0].current = Tile.chessPiece.KnightPlayer;
        GridManager.Board[2, 0].current = Tile.chessPiece.BishopPlayer;


        if (gridManager.startsAsWhite == true)
        {
            GridManager.Board[3, 0].current = Tile.chessPiece.QueenPlayer;
            GridManager.Board[4, 0].current = Tile.chessPiece.KingPlayer;
        }
        else
        {
            GridManager.Board[4, 0].current = Tile.chessPiece.QueenPlayer;
            GridManager.Board[3, 0].current = Tile.chessPiece.KingPlayer;
        }
        GridManager.Board[5, 0].current = Tile.chessPiece.BishopPlayer;
        GridManager.Board[6, 0].current = Tile.chessPiece.KnightPlayer;
        GridManager.Board[7, 0].current = Tile.chessPiece.TowerPlayer;

        for (int i = 0; i < 8; i++)
        {
            GridManager.Board[i, 1].current = Tile.chessPiece.PawnPlayer;
        }
    }
}
