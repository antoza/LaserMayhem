using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable
public class BoardManager : ScriptableObject
{
    [SerializeField] public int width { get; private set; } = 7;
    [SerializeField] public int height { get; private set; } = 7;
    private Piece?[,] boardArray;

    void Start()
    {
        boardArray = new Piece?[width, height];
    }

    public Piece? GetPiece((int, int) tile)
    {
        return boardArray[tile.Item1, tile.Item2];
    }

    public bool IsTileEmpty((int, int) tile)
    {
        return boardArray[tile.Item1, tile.Item2];
    }

    public bool IsOnBoard((int, int) tile)
    {
        return tile.Item1 >= 0 && tile.Item1 <= width && tile.Item2 >= 0 && tile.Item2 <= height;
    }

    public bool PlaceOnTile(Piece piece, (int, int) tile)
    {
        if (IsTileEmpty(tile))
        {
            boardArray[tile.Item1, tile.Item2] = piece;
            return true;
        }
        return false;
    }

    public Piece? EmptyTile((int, int) tile)
    {
        Piece? placedPiece = boardArray[tile.Item1, tile.Item2];
        boardArray[tile.Item1, tile.Item2] = null;
        return placedPiece;
    }
}
