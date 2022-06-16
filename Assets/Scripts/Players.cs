using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Players : MonoBehaviour
{
    public bool turn = false;

    private Tile.chessPiece[] piecesOponent =
    {
        Tile.chessPiece.PawnOponent,
        Tile.chessPiece.KnightOponent,
        Tile.chessPiece.BishopOponent,
        Tile.chessPiece.TowerOponent,
        Tile.chessPiece.QueenOponent,
        Tile.chessPiece.KingOponent,
        Tile.chessPiece.None
    };
    private Tile.chessPiece[] piecesPlayer =
    {
        Tile.chessPiece.PawnPlayer,
        Tile.chessPiece.KnightPlayer,
        Tile.chessPiece.BishopPlayer,
        Tile.chessPiece.TowerPlayer,
        Tile.chessPiece.QueenPlayer,
        Tile.chessPiece.KingPlayer,
        Tile.chessPiece.None
    };

    public bool AttackandMoveController(Tile.chessPiece attacked)
    {
        //White
        if (!turn)
        {
            for (int i = 0; i < piecesOponent.Length; i++)
            {
                if (attacked == piecesOponent[i])
                {
                    Debug.Log("White");
                    return true;
                }
            }
            
        }
        //Black
        else
        {
            for (int i = 0; i < piecesPlayer.Length; i++)
            {
                if (attacked == piecesPlayer[i])
                {
                    Debug.Log("Black");
                    return true;
                }

            }
          
        }
        return false;
    }
}
