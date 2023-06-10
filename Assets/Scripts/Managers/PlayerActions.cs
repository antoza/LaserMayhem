using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : ScriptableObject
{
    private PlayerData PlayerData;
    private bool m_CanPlay = false;
    public Piece m_SelectedPiece;

    public PlayerActions(PlayerData playerData)
    {
        PlayerData = playerData;
    }

    public void CallPlayer(int turnNumber)
    {
        PlayerData.PlayerEconomy.AddNewTurnMana(turnNumber);
        m_CanPlay = true;
        m_SelectedPiece = null;
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

    public bool PlacePiece(int pieceNumber, (int, int) selectedSpot)
    {
        if (!m_CanPlay) return false;
        int pieceCost = -1; // DataManager.SelectablePieces.m_piecesListInfo[pieceNumber].m_cost
        if (PlayerData.PlayerEconomy.PayForPlacement(pieceCost))
        {
            return PlayerData.DataManager.BoardManager.PlaceOnTile(m_SelectedPiece, selectedSpot);
        }
        return false;
    }

    public bool DeletePiece((int, int) selectedSpot)
    {
        if (!m_CanPlay) return false;
        if (PlayerData.PlayerEconomy.PayForDeletion())
        {
            PlayerData.DataManager.BoardManager.EmptyTile(selectedSpot);
            return true;
        }
        return false;
    }

    public bool MovePiece((int, int) sourceSpot, (int, int) destinationSpot)
    {
        if (!m_CanPlay) return false;
        if (PlayerData.PlayerEconomy.PayForMovement())
        {
            //if (DataManager.Rules.IsMovementAllowed())
            //{
                Piece movedPiece = PlayerData.DataManager.BoardManager.EmptyTile(sourceSpot);
                PlayerData.DataManager.BoardManager.PlaceOnTile(movedPiece, sourceSpot);
                return true;
            //}
        }
        return false;
    }

    public void ProcessTileClicked((int, int) spot)
    {
        Debug.Log(FindObjectOfType<DataManager>().BoardManager.GetPiece(spot));
        if (m_SelectedPiece == null)
        {
            return;
        }
        if (PlacePiece(1, spot))
        {
            m_SelectedPiece = null;
        }
    }
}
