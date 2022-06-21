using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Players : MonoBehaviour
{
    public bool turn = false;
    public TextMeshPro tmp0;
    public TextMeshPro tmp1;

    public Tile.chessPiece[] piecesOpponent =
    {
        Tile.chessPiece.PawnOpponent,
        Tile.chessPiece.KnightOpponent,
        Tile.chessPiece.BishopOpponent,
        Tile.chessPiece.TowerOpponent,
        Tile.chessPiece.QueenOpponent,
        Tile.chessPiece.KingOpponent,
        Tile.chessPiece.None
    };
    public Tile.chessPiece[] piecesPlayer =
    {
        Tile.chessPiece.PawnPlayer,
        Tile.chessPiece.KnightPlayer,
        Tile.chessPiece.BishopPlayer,
        Tile.chessPiece.TowerPlayer,
        Tile.chessPiece.QueenPlayer,
        Tile.chessPiece.KingPlayer,
        Tile.chessPiece.None
    };

    public bool AttackandMoveController(Tile.chessPiece target)
    {
        //White
        if (!turn)
        {
            for (int i = 0; i < piecesOpponent.Length; i++)
            {
                if (target == piecesOpponent[i])
                {
                    tmp0.text = "White";
                    tmp1.text = "White";
                    return true;
                }
            }
        }
        //Black
        else
        {
            for (int i = 0; i < piecesPlayer.Length; i++)
            {
                if (target == piecesPlayer[i])
                {
                    tmp0.text = "Black";
                    tmp1.text = "Black";
                    return true;
                }
            }
        }
        return false;
    }
}
