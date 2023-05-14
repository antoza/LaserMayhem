using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable
public class Board : MonoBehaviour
{
    [SerializeField] private int width = 7;
    [SerializeField] private int height = 7;
    [SerializeField] private Piece?[,] boardArray;

    void Start()
    {
        boardArray = new Piece?[width, height];
    }

    public int getWidth() { return width; }
    public int getHeight() { return height; }

    public Piece? getPiece((int, int) tile)
    {
        return boardArray[tile.Item1, tile.Item2];
    }

    public bool isTileEmpty((int, int) tile)
    {
        return boardArray[tile.Item1, tile.Item2];
    }

    public bool isOnBoard((int, int) tile)
    {
        return tile.Item1 >= 0 && tile.Item1 <= width && tile.Item2 >= 0 && tile.Item2 <= height;
    }

    public bool placeOnTile(Piece piece, (int, int) tile)
    {
        if (isTileEmpty(tile))
        {
            boardArray[tile.Item1, tile.Item2] = piece;
            return true;
        }
        return false;
    }

    public Piece? emptyTile((int, int) tile)
    {
        Piece? placedPiece = boardArray[tile.Item1, tile.Item2];
        boardArray[tile.Item1, tile.Item2] = null;
        return placedPiece;
    }
}
