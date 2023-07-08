using Mono.CompilerServices.SymbolWriter;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable
public class PlayerActions : ScriptableObject
{
    private PlayerData PlayerData;
    private bool m_CanPlay = false;
    public Tile? m_SourceTile;

    public PlayerActions(PlayerData playerData)
    {
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

    public void MoveToDestinationTile(Tile destinationTile)
    {
        if (m_SourceTile == null)
        {
            return;
        }
        Debug.Log(m_SourceTile);
        Debug.Log(destinationTile);
        switch ((m_SourceTile, destinationTile))
        {
            case (BoardTile, BoardTile):
                MovePiece((BoardTile)m_SourceTile!, (BoardTile)destinationTile);
                break;
            case (InfiniteTile, BoardTile):
                PlacePiece(m_SourceTile, (BoardTile)destinationTile);
                break;
            case (BoardTile, TrashTile):
                DeletePiece((BoardTile)m_SourceTile);
                break;
        }
        // if destinationTile is TrashTile ... on delete la pièce
        {
            /*if (MovePiece((m_SelectedPiece.parentTile.x, m_SelectedPiece.parentTile.y), spot))
            { // CHANGER : faire en sorte que Piece ne soit plus cliquable, mais seulement les BoardTile le sont. Ajouter des tiles à gauche du board pour les pièces hors jeu
                m_SelectedPiece = null;
            }*/
        }

    }
}
