using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public enum TileName
{
    None,
    NormalBoardTile,
    InvisibleBoardTile,
    SelectionTile,
    InfiniteTile,
    TrashTile,
}

#nullable enable
public class TilePrefabs : MonoBehaviour
{
    public static TilePrefabs Instance { get; private set; }
    [SerializeField]
    private List<Tile> tileList;
    private Dictionary<TileName, Tile> tileDictionary;
    /*
    public static PiecePrefabs GetInstance()
    {
        Assert.IsNotNull(Instance, "PiecePrefabs has not been instantiated");
        return Instance!;
    }*/

    private void Awake()
    {
        Instance = this;
        tileDictionary = new Dictionary<TileName, Tile>();
        foreach (Tile tile in tileList)
        {
            tileDictionary.Add(GetTileNameFromTile(tile), tile);
        }
    }

    public TileName GetTileNameFromTile(Tile tile)
    {
        bool result = Enum.TryParse(tile.GetType().Name, out TileName tileName);
        Assert.IsTrue(result);
        return tileName;
    }

    public Tile? GetTileOrNull(TileName tileName)
    {
        if (tileDictionary.TryGetValue(tileName, out Tile tile)) return tile;
        return null;
    }

    public Tile GetTile(TileName tileName)
    {
        Tile? tile = GetTileOrNull(tileName);
        Assert.IsNotNull(tile);
        return tile!;
    }
}