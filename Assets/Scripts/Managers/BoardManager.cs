using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using System;

#nullable enable
public sealed class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { get; private set; }

    private GameObject m_board;

    // enlever ça, prendre le transform initial de la tile et le modifier pour les coordonnées ajustées à l'écran
    public int Width { get; private set; }
    public int Height { get; private set; }
    public float ScaleWidth { get; private set; }
    public float ScaleHeight { get; private set; }
    private BoardTile[,] tilesArray;

    private void Awake()
    {
        Instance = this;
        m_board = new GameObject("Board");
        Width = DataManager.Instance.Rules.BoardWidth;
        Height = DataManager.Instance.Rules.BoardHeight;
        ScaleWidth = DataManager.Instance.Rules.BoardScaleWidth;
        ScaleHeight = DataManager.Instance.Rules.BoardScaleHeight;
        tilesArray = new BoardTile[Width, Height];
        GenerateAllTiles();
    }

    private void Start()
    {
    }

    public Piece? GetPiece(Vector2Int tile)
    {
        return tilesArray[tile[0], tile[1]].Piece;
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
                if (GameInitialParameters.localPlayerID == 1)
                {
                    int subX = Height - 1 - x;
                    int subY = Width - 1 - y;
                    tilesArray[subX, subY] = GenerateTile(x, y, prefab);
                }
                else
                {
                    tilesArray[x, y] = GenerateTile(x, y, prefab);
                }
                
            }
        }
    }

    private BoardTile GenerateTile(int x, int y, GameObject prefab)
    {
        GameObject spawnedTile = Instantiate(prefab, m_board.transform);
        spawnedTile.name = "Tile_" + x + "_" + y;
        BoardTile boardTile = spawnedTile.GetComponent<BoardTile>();

        boardTile.y = y;
        boardTile.x = x;
        if(GameInitialParameters.localPlayerID == 1)
        {
            x = Height - 1 - x;
            y = Width - 1 - y;
        }
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
