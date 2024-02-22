using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public enum PieceName
{
    None,
    LaserEmitter = 1,
    Wall = 101,
    MirrorSlash = 201,
    MirrorBackSlash = 202,
    MirrorDivider = 203,
    MirrorHorizontal = 204,
    MirrorVertical = 205,
    MirrorHour = 206,
    MirrorTrigo = 207,
    WeaknessRight = 301,
    WeaknessLeft = 302,
    WeaknessUp = 303,
    WeaknessDown = 304,
    Eye = 310,
}

#nullable enable
public class PiecePrefabs : MonoBehaviour
{
    public static PiecePrefabs Instance { get; private set; }
    [SerializeField]
    private List<Piece> pieceList;
    private Dictionary<PieceName, Piece> pieceDictionary;
    /*
    public static PiecePrefabs GetInstance()
    {
        Assert.IsNotNull(Instance, "PiecePrefabs has not been instantiated");
        return Instance!;
    }*/

    private void Awake()
    {
        Instance = this;
        pieceDictionary = new Dictionary<PieceName, Piece>();
        foreach (Piece piece in pieceList)
        {
            pieceDictionary.Add(GetPieceNameFromPiece(piece), piece);
        }
    }

    public PieceName GetPieceNameFromPiece(Piece piece)
    {
        bool result = Enum.TryParse(piece.GetType().Name, out PieceName pieceName);
        Assert.IsTrue(result);
        return pieceName;
    }

    public Piece? GetPieceOrNull(PieceName pieceName)
    {
        if (pieceDictionary.TryGetValue(pieceName, out Piece piece)) return piece;
        return null;
    }

    public Piece GetPiece(PieceName pieceName)
    {
        Piece? piece = GetPieceOrNull(pieceName);
        Assert.IsNotNull(piece);
        return piece!;
    }
}