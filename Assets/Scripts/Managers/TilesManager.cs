using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

public sealed class TilesManager
{
    public static TilesManager Instance { get; private set; } // ou private
    private Tile[] _tiles;
    private int _nextTileID = 0;

    private TilesManager() { }

    public static TilesManager GetInstance()
    {
        if (Instance == null)
        {
            Instance = new TilesManager();
        }

        return Instance;
    }

    public void AddTile(Tile tile)
    {
        _tiles.Append(tile);
    }
}
