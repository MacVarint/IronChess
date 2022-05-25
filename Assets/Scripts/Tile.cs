using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum chessPiece {PawnWhite, KnightWhite, BishopWhite, TowerWhite, QueenWhite, KingWhite, PawnBlack, KnightBlack, BishopBlack, TowerBlack, QueenBlack, KingBlack, None };

    public chessPiece current = chessPiece.None;
    public SpriteRenderer imageHolder;

}
