using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum chessPiece {PawnPlayer, KnightPlayer, BishopPlayer, TowerPlayer, QueenPlayer, KingPlayer, PawnOponent, KnightOponent, BishopOponent, TowerOponent, QueenOponent, KingOponent, None };
    public Vector2Int gridpos;
    public chessPiece current = chessPiece.None;
    public SpriteRenderer imageHolder;

}
