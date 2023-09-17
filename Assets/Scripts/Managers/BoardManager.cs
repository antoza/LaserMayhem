using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

#nullable enable
public class BoardManager : ScriptableObject
{
    private DataManager DM;
    private GameObject m_board;
    public int width { get; private set; }
    public int height { get; private set; }
    public float scaleWidth { get; private set; }
    public float scaleHeight { get; private set; }
    private BoardTile[,] tilesArray;

    public BoardManager(DataManager dataManager, GameObject board)
    {
        DM = dataManager;
        m_board = board;
        width = DM.Rules.BoardWidth;
        height = DM.Rules.BoardHeight;
        scaleWidth = DM.Rules.BoardScaleWidth;
        scaleHeight = DM.Rules.BoardScaleHeight;
        tilesArray = new BoardTile[width, height];
        GenerateAllTiles();
    }

    public Piece? GetPiece(Vector2Int tile)
    {
        return tilesArray[tile[0], tile[1]].m_Piece;
    }
    
    public bool IsOnBoard(Vector2Int tile)
    {
        return tile[0] >= 0 && tile[0] < width && tile[1] >= 0 && tile[1] < height;
    }

    private void GenerateAllTiles()
    {
        GameObject prefab = DM.Rules.TilePrefab;
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
        spawnedTile.transform.SetParent(m_board.transform);
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

    public Vector2Int ConvertBoardCoordinateToWorldCoordinates(Vector2Int coord)
    {
        return new Vector2Int(coord[0] - (width-1)/2, coord[1] - (height-1) / 2);
    }
}
