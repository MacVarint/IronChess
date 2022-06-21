using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Moves
{
    private static Vector2[] pawn =
    {
        new Vector2(0, 1)
    };
    private static Vector2[] pawnOpponent =
    {
        new Vector2(0, -1)
    };
    private static Vector2[] knight =
    {
        new Vector2(2, 1),
        new Vector2(1, 2),
        new Vector2(2, -1),
        new Vector2(1, -2),
        new Vector2(-2, -1),
        new Vector2(-1, -2),
        new Vector2(-2, 1),
        new Vector2(-1, 2)

    };
    private static Vector2[] bishop =
    {
        new Vector2(1, 1),
        new Vector2(1, -1),
        new Vector2(-1, -1),
        new Vector2(-1, 1)
    };
    private static Vector2[] tower =
    {
        new Vector2(0, 1),
        new Vector2(1, 0),
        new Vector2(0, -1),
        new Vector2(-1, 0)
    };
    private static Vector2[] queen =
    {
        new Vector2(0, 1),
        new Vector2(1, 1),
        new Vector2(1, 0),
        new Vector2(1, -1),
        new Vector2(0, -1),
        new Vector2(-1, -1),
        new Vector2(-1, 0),
        new Vector2(-1, 1)
    };
    private static Vector2[] king =
    {
        new Vector2(0, 1),
        new Vector2(1, 1),
        new Vector2(1, 0),
        new Vector2(1, -1),
        new Vector2(0, -1),
        new Vector2(-1, -1),
        new Vector2(-1, 0),
        new Vector2(-1, 1)
    };
    public static Vector2[] getMoveSet(Tile.chessPiece piece)
    {
        if (piece == Tile.chessPiece.PawnPlayer)
        {
            return pawn;
        }
        if (piece == Tile.chessPiece.KnightPlayer)
        {
            return knight;
        }
        if (piece == Tile.chessPiece.BishopPlayer)
        {
            return bishop;
        }
        if (piece == Tile.chessPiece.TowerPlayer)
        {
            return tower;
        }
        if (piece == Tile.chessPiece.QueenPlayer)
        {
            return queen;
        }
        if (piece == Tile.chessPiece.KingPlayer)
        {
            return king;
        }
        if (piece == Tile.chessPiece.PawnOpponent)
        {
            return pawnOpponent;
        }
        if (piece == Tile.chessPiece.KnightOpponent)
        {
            return knight;
        }
        if (piece == Tile.chessPiece.BishopOpponent)
        {
            return bishop;
        }
        if (piece == Tile.chessPiece.TowerOpponent)
        {
            return tower;
        }
        if (piece == Tile.chessPiece.QueenOpponent)
        {
            return queen;
        }
        if (piece == Tile.chessPiece.KingOpponent)
        {
            return king;
        }
        return null;
    }

    public static bool getRepeat(Tile.chessPiece piece)
    {
        if (piece == Tile.chessPiece.PawnOpponent || piece == Tile.chessPiece.PawnPlayer || piece == Tile.chessPiece.KnightOpponent || piece == Tile.chessPiece.KnightPlayer || piece == Tile.chessPiece.KingOpponent || piece == Tile.chessPiece.KingPlayer)
        {
            return false;
        }
        return true;
    }
}