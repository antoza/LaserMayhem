using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using System;

#nullable enable
public sealed class BoardManager : ScriptableObject
{
    public static BoardManager? Instance { get; private set; }

    private readonly GameObject m_board;
    public int Width { get; private set; }
    public int Height { get; private set; }
    public float ScaleWidth { get; private set; }
    public float ScaleHeight { get; private set; }
    private BoardTile[,] tilesArray;

    public static void SetInstance(GameObject board)
    {
        Instance = new BoardManager(board);
    }

    public static BoardManager GetInstance()
    {
        if (Instance == null)
        {
            Debug.LogError("BoardManager has not been instantiated");
        }

        return Instance!;
    }


    public BoardManager(GameObject board)
    {
        m_board = board;
        DataManager DM = DataManager.Instance;
        Width = DM.Rules.BoardWidth;
        Height = DM.Rules.BoardHeight;
        ScaleWidth = DM.Rules.BoardScaleWidth;
        ScaleHeight = DM.Rules.BoardScaleHeight;
        tilesArray = new BoardTile[Width, Height];
        GenerateAllTiles();
    }

    public Piece? GetPiece(Vector2Int tile)
    {
        return tilesArray[tile[0], tile[1]].m_Piece;
    }
    
    public bool IsOnBoard(Vector2Int tile)
    {
        return tile[0] >= 0 && tile[0] < Width && tile[1] >= 0 && tile[1] < Height;
    }

    private void GenerateAllTiles()
    {
        GameObject prefab = DataManager.Instance.Rules.TilePrefab;
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                tilesArray[x, y] = GenerateTile(x, y, prefab);
            }
        }
    }

    private BoardTile GenerateTile(int x, int y, GameObject prefab)
    {
        GameObject spawnedTile = Instantiate(prefab);
        spawnedTile.transform.SetParent(m_board.transform);
        spawnedTile.name = "Tile_" + x + "_" + y;
        BoardTile boardTile = spawnedTile.GetComponent<BoardTile>();

        boardTile.x = x;
        boardTile.y = y;
        boardTile.positionX = (x - Width/2) * ScaleWidth / Width;
        boardTile.positionY = (y - Height/2) * ScaleHeight / Height;
        boardTile.scaleWidth = ScaleWidth / Width;
        boardTile.scaleHeight = ScaleHeight / Height;

        return boardTile;
    }

    public Vector2Int ConvertBoardCoordinateToWorldCoordinates(Vector2Int coord)
    {
        return new Vector2Int(coord[0] - (Width-1)/2, coord[1] - (Height-1) / 2);
    }
}
