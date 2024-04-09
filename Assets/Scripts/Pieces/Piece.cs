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

    [HideInInspector]
    public Tile? InitialTile;

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
        Piece piece = Instantiate(this, parent?.transform).GetComponent<Piece>();
        if (GameInitialParameters.localPlayerID == 1) piece.RotateSprite180();
        return piece;
    }

    public PieceName GetPieceName()
    {
        return PiecePrefabs.Instance.GetPieceNameFromPiece(this);
    }

    public Sprite GetSprite()
    {
        return GetComponent<SpriteRenderer>().sprite;
    }


    public virtual void RotateSpriteClockwise()
    {
        transform.Rotate(0, 0, -90);
    }

    public virtual void RotateSpriteAnticlockwise()
    {
        for (int i = 0; i < 3; i++) RotateSpriteClockwise();
    }

    public virtual void RotateSprite180()
    {
        for (int i = 0; i < 2; i++) RotateSpriteClockwise();
    }

    public virtual void RotateClockwise()
    {
        RotateSpriteClockwise();
    }

    public void RotateAnticlockwise()
    {
        for (int i = 0; i < 3; i++) RotateClockwise();
    }

    public void Rotate180()
    {
        for (int i = 0; i < 2; i++) RotateClockwise();
    }


    public virtual void ReceiveLaser(Laser? laser, Vector2Int inDirection)
    {
        Assert.IsTrue(ParentTile is BoardTile);
    }
}
