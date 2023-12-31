using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public enum PieceName
{
    None,
    MirrorSlash,
    MirrorBackSlash,
    Divider,
    MirrorHorizontal,
    MirrorVertical,
    RotateHour,
    RotateTrigo,
    Wall
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
        PieceName pieceName = PieceName.None;
        Assert.IsTrue(Enum.TryParse(piece.GetType().Name, out pieceName));
        return pieceName;
    }

    public Piece? GetPieceOrNull(PieceName pieceName)
    {
        Piece piece;
        if (pieceDictionary.TryGetValue(pieceName, out piece)) return piece;
        return null;
    }

    public Piece GetPiece(PieceName pieceName)
    {
        Piece? piece = GetPieceOrNull(pieceName);
        Assert.IsNotNull(piece);
        return piece!;
    }
}
