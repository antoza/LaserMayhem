using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public enum TileName
{
    None = 0,
    NormalBoardTile = 1,
    InvisibleBoardTile = 101,
    LockedBoardTile = 102,
    ConveyorBoardTile = 103,
    SelectionTile = 201,
    PricedSelectionTile = 202,
    InfiniteTile = 203,
    TrashTile = 301,
}

#nullable enable
public class TilePrefabs : MonoBehaviour
{
    public static TilePrefabs Instance { get; private set; }
    [SerializeField]
    private List<Tile> tileList;
    private Dictionary<TileName, Tile> tileDictionary;

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