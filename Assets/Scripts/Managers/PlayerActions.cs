using Mono.CompilerServices.SymbolWriter;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

#nullable enable
public class PlayerActions : NetworkBehaviour
{
    private DataManager m_DataManager;
    private PlayerData PlayerData;
    private bool m_CanPlay = false;
    public Tile? m_SourceTile;

    public PlayerActions(PlayerData playerData)
    {
        m_DataManager = FindObjectOfType<DataManager>();
        PlayerData = playerData;
    }

    public void StartTurn(int turnNumber)
    {
        PlayerData.PlayerEconomy.AddNewTurnMana(turnNumber);
        m_CanPlay = true;
        m_SourceTile = null;
    }

    public bool EndTurn()
    {
        if (PlayerData.DataManager.TurnManager.TrySkipTurn(false))
        {
            m_CanPlay = false;
            return true;
        }
        return false;
    }

    public bool PlacePiece(Tile sourceTile, BoardTile destinationTile)
    {
        if (!m_CanPlay) return false;
        int pieceCost = -1; // DataManager.SelectablePieces.m_piecesListInfo[pieceNumber].m_cost
        Piece? copiedPiece = sourceTile.m_Piece;
        if (!copiedPiece) return false;
        if (destinationTile.m_Piece) return false;
        if (PlayerData.PlayerEconomy.PayForPlacement(pieceCost))
        {
            destinationTile.UpdatePiece(copiedPiece!);
            m_DataManager.LaserManager.UpdateLaser(true);
            return true;
        }
        return false;
    }

    public bool DeletePiece(BoardTile tile)
    {
        if (!m_CanPlay) return false;
        if (!tile.m_Piece) return false;
        Debug.Log("oui");
        if (PlayerData.PlayerEconomy.PayForDeletion())
        {
            tile.UpdatePiece(null);
            m_DataManager.LaserManager.UpdateLaser(true);
            return true;
        }
        return false;
    }

    public bool MovePiece(BoardTile sourceTile, BoardTile destinationTile)
    {
        if (!m_CanPlay) return false;
        //if (DataManager.Rules.IsMovementAllowed())
        //{
        if (sourceTile == destinationTile) return false;
        Piece? movedPiece = sourceTile.m_Piece;
        if (!movedPiece) return false;
        if (destinationTile.m_Piece) return false;
        if (PlayerData.PlayerEconomy.PayForMovement())
        {
            destinationTile.UpdatePiece(movedPiece);
            sourceTile.UpdatePiece(null);
            m_DataManager.LaserManager.UpdateLaser(true);
            return true;
        }
        //}
        return false;
    }

    public void SetSourceTile(Tile sourceTile)
    {
        if (sourceTile is BoardTile or InfiniteTile)
        m_SourceTile = sourceTile;
    }

    public void MoveToDestinationTile(Tile? sourceTile, Tile destinationTile)
    {
        if (sourceTile == null)
        {
            return;
        }
        Debug.Log(sourceTile);
        Debug.Log(destinationTile);
        switch ((sourceTile, destinationTile))
        {
            case (BoardTile, BoardTile):
                MovePiece((BoardTile)sourceTile!, (BoardTile)destinationTile);
                break;
            case (InfiniteTile, BoardTile):
                PlacePiece(sourceTile, (BoardTile)destinationTile);
                break;
            case (BoardTile, TrashTile):
                DeletePiece((BoardTile)sourceTile);
                break;
            default:
                break;
        }
    }
    /*
    [ClientRpc]
    void RpcDoAction(Tile destinationTile)
    {
        MoveToDestinationTile(m_SourceTile, destinationTile);
    }*/

    //[Command] (pas public)
    public void CmdDoAction(Tile destinationTile)
    {
        MoveToDestinationTile(m_SourceTile, destinationTile);
        m_SourceTile = null;
        /*RpcDoAction(destinationTile);*/
    }

}
