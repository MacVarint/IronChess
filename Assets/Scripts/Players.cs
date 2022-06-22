using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Players : MonoBehaviour
{
    public bool turn = false;


    public Tile.chessPiece[] piecesOpponent =
    {
        Tile.chessPiece.PawnOpponent,
        Tile.chessPiece.KnightOpponent,
        Tile.chessPiece.BishopOpponent,
        Tile.chessPiece.TowerOpponent,
        Tile.chessPiece.QueenOpponent,
        Tile.chessPiece.KingOpponent
    };
    public Tile.chessPiece[] piecesPlayer =
    {
        Tile.chessPiece.PawnPlayer,
        Tile.chessPiece.KnightPlayer,
        Tile.chessPiece.BishopPlayer,
        Tile.chessPiece.TowerPlayer,
        Tile.chessPiece.QueenPlayer,
        Tile.chessPiece.KingPlayer
    };

    //check if attack is allowed.
    public bool AttackController(Tile.chessPiece target)
    {
        //White
        if (!turn)
        {

            for (int i = 0; i < piecesOpponent.Length; i++)
            {
                if (piecesOpponent[i].Equals(target))
                {
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
                    return true;
                }
            }
        }
        return false;
    }   
    public bool CanISelectThis(Tile.chessPiece target)
    {
        if (!turn)
        {
            if(Array.IndexOf(piecesPlayer, target) >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (Array.IndexOf(piecesOpponent, target) >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public bool CanIHitThis(Tile.chessPiece target)
    {
        if (turn)
        {
            if (Array.IndexOf(piecesPlayer, target) >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (Array.IndexOf(piecesOpponent, target) >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
