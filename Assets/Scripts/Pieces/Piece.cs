using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

#nullable enable
public abstract class Piece : MonoBehaviour
{
    private Tile? _parentTile;
    public Tile? ParentTile
    {
        get => _parentTile;
        set
        {
#if !DEDICATED_SERVER
            if (_parentTile != null)
            {
                if (IsPlayedThisTurn) { _parentTile.StopPulsating(); }
            }
#endif

            _parentTile = value;
#if !DEDICATED_SERVER
            if (_parentTile != null)
            {
                if (IsPlayedThisTurn) { _parentTile.StartPulsating(); }
            }
#endif
        }
    }

    private bool _isPlayedThisTurn = false;
    public bool IsPlayedThisTurn
    {
        get => _isPlayedThisTurn;
        set
        {
            _isPlayedThisTurn = value;
#if !DEDICATED_SERVER
            if (ParentTile != null)
            {
                if (IsPlayedThisTurn) { ParentTile.StartPulsating(); }
                else { ParentTile.StopPulsating(); };
            }
#endif
            if (IsPlayedThisTurn) TurnManager.Instance.AddPiecePlayedThisTurn(this);
            else TurnManager.Instance.RemovePiecePlayedThisTurn(this);
        }
    }

    public Piece InstantiatePiece(GameObject? parent = null)
    {
        return Instantiate(this, parent?.transform).GetComponent<Piece>();
    }

    public PieceName GetPieceName()
    {
        return PiecePrefabs.Instance.GetPieceNameFromPiece(this);
    }

    public Sprite GetSprite()
    {
        return GetComponent<SpriteRenderer>().sprite;
    }

    public abstract IEnumerable<Vector2Int> ComputeNewDirections(Vector2Int sourceDirection);
}
