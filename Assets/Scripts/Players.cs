using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Players : MonoBehaviour
{
    bool turn = false;

    private Tile.chessPiece[] piecesOponent =
    {
        Tile.chessPiece.PawnOponent,
        Tile.chessPiece.KnightOponent,
        Tile.chessPiece.BishopOponent,
        Tile.chessPiece.TowerOponent,
        Tile.chessPiece.QueenOponent,
        Tile.chessPiece.KingOponent
    };
    private Tile.chessPiece[] piecesPlayer =
    {
        Tile.chessPiece.PawnPlayer,
        Tile.chessPiece.KnightPlayer,
        Tile.chessPiece.BishopPlayer,
        Tile.chessPiece.TowerPlayer,
        Tile.chessPiece.QueenPlayer,
        Tile.chessPiece.KingPlayer
    };

    public bool AttackController(Tile.chessPiece attacker, Tile.chessPiece attacked)
    {
        if (!turn)
        {
            //player1
        }
        else
        {
            //player2
        }

        for (int i = 0; i < 6; i++)
        {
            if (attacker == piecesOponent[i])
            {
                //current is opponent
                for (int j = 0; j < 6; j++)
                {
                    if (attacked == piecesOponent[j])
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            else
            {
                //current is player
                for (int j = 0; j < 6; j++)
                {
                    if (attacked == piecesPlayer[j])
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
