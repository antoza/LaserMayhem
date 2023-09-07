using Mono.CompilerServices.SymbolWriter;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

#nullable enable
public class PlayerActions : NetworkBehaviour
{
    private DataManager DM;
    private PlayerData PlayerData;
    private bool m_CanPlay = false;
    public Tile? m_SourceTile;

    public PlayerActions(PlayerData playerData)
    {
        PlayerData = playerData;
        DM = PlayerData.DM;
    }

    public void StartTurn(int turnNumber)
    {
        PlayerData.PlayerEconomy.AddNewTurnMana(turnNumber);
        m_CanPlay = true;
        m_SourceTile = null;
    }

    public bool EndTurn()
    {
        if (DM.TurnManager.TrySkipTurn(false))
        {
            m_CanPlay = false;
            return true;
        }
        return false;
    }

    public bool CopyPiece(Tile sourceTile, Tile destinationTile) // A déplacer dans un fichier plus adapté
    {
        if (!sourceTile.m_Piece) return false;
        if (destinationTile.m_Piece) return false;

        destinationTile.UpdatePiece(sourceTile.m_Piece!);
        DM.LaserManager.UpdateLaser(true);
        return true;
    }

    public bool DeletePiece(Tile tile) // A déplacer dans un fichier plus adapté
    {
        if (!tile.m_Piece) return false;

        tile.UpdatePiece(null);
        DM.LaserManager.UpdateLaser(true);
        return true;
    }

    public bool MovePiece(Tile sourceTile, Tile destinationTile) // A déplacer dans un fichier plus adapté
    {
        if (sourceTile == destinationTile) return false;
        if (!sourceTile.m_Piece) return false;
        if (destinationTile.m_Piece) return false;

        destinationTile.UpdatePiece(sourceTile.m_Piece);
        sourceTile.UpdatePiece(null);
        DM.LaserManager.UpdateLaser(true);
        return true;
    }

    public void SetSourceTile(Tile sourceTile)
    {
        m_SourceTile = sourceTile;
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
        if (!m_CanPlay)
        {
            Debug.Log("It is not your turn to play");
            return;
        }
        DM.GameMode.MoveToDestinationTile(m_SourceTile, destinationTile, PlayerData);
        m_SourceTile = null;
        /*RpcDoAction(destinationTile);*/
    }

}
