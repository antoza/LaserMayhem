using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

#nullable enable
public class BoardManager : ScriptableObject
{
    private DataManager m_DataManager;
    public int width { get; private set; }
    public int height { get; private set; }
    public float scaleWidth { get; private set; }
    public float scaleHeight { get; private set; }
    private Piece?[,] piecesArray;
    private BoardTile?[,] tilesArray;

    private void Awake()
    {
        m_DataManager = FindObjectOfType<DataManager>();
        width = m_DataManager.Rules.BoardWidth;
        height = m_DataManager.Rules.BoardHeight;
        scaleWidth = m_DataManager.Rules.BoardScaleWidth;
        scaleHeight = m_DataManager.Rules.BoardScaleHeight;
        piecesArray = new Piece?[width, height];
        tilesArray = new BoardTile?[width, height];
        GenerateAllTiles();
    }

    public Piece? GetPiece((int, int) tile)
    {
        return piecesArray[tile.Item1, tile.Item2];
    }

    public bool IsTileEmpty((int, int) tile)
    {
        return piecesArray[tile.Item1, tile.Item2];
    }

    public bool IsOnBoard((int, int) tile)
    {
        return tile.Item1 >= 0 && tile.Item1 < width && tile.Item2 >= 0 && tile.Item2 < height;
    }

    public bool PlaceOnTile(Piece piece, (int, int) tile)
    {
        if (IsTileEmpty(tile))
        {
            Debug.Log("oui");
            piecesArray[tile.Item1, tile.Item2] = piece;
            return true;
        }
        Debug.Log("non");
        return false;
    }

    public Piece? EmptyTile((int, int) tile)
    {
        Piece? placedPiece = piecesArray[tile.Item1, tile.Item2];
        piecesArray[tile.Item1, tile.Item2] = null;
        return placedPiece;
    }


    private void GenerateAllTiles()
    {
        GameObject prefab = m_DataManager.Rules.TilePrefab;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tilesArray[x, y] = GenerateTile(x, y, prefab);
            }
        }
    }

    private BoardTile GenerateTile(int x, int y, GameObject prefab)
    {
        GameObject spawnedTile = Instantiate(prefab);
        spawnedTile.transform.SetParent(m_DataManager.Board.transform);
        spawnedTile.AddComponent<BoardTile>();
        spawnedTile.name = "Tile_" + x + "_" + y;
        BoardTile boardTile = spawnedTile.GetComponent<BoardTile>();

        boardTile.x = x;
        boardTile.y = y;
        boardTile.positionX = (x - width/2) * scaleWidth / width;
        boardTile.positionY = (y - height/2) * scaleHeight / height;
        boardTile.scaleWidth = scaleWidth / width;
        boardTile.scaleHeight = scaleHeight / height;

        return boardTile;
    }
}
