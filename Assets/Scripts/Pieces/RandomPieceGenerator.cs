using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class RandomPieceGenerator : ScriptableObject
{
    [SerializedDictionary("Piece name", "Probability of occurence")]
    public SerializedDictionary<PieceName, float> _piecesPool;

    public PieceName GetRandomButUniquePiece(List<PieceName> excludedPieces)
    {
        PieceName randomPiece;
        do
        {
            randomPiece = GetRandomPiece();
        }
        while (excludedPieces.Contains(randomPiece));
        return randomPiece;
    }

    public PieceName GetRandomPiece()
    {
        float probabilitySum = 0;
        foreach (float probability in _piecesPool.Values)
        {
            probabilitySum += probability;
        }
        // The sum should equal 100f
        float rd = Random.Range(0f, probabilitySum);
        foreach (KeyValuePair<PieceName, float> pair in _piecesPool)
        {
            rd -= pair.Value;
            if (rd < 0f) return pair.Key;
        }

        // Should never happen
        return _piecesPool.First().Key;
    }
}