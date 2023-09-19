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
    public bool m_CanPlay { get; private set; } = false;
    public Tile? m_SourceTile;

    public PlayerActions(PlayerData playerData)
    {
        PlayerData = playerData;
        DM = PlayerData.DM;
    }

    public bool CanPlay()
    {
        if (!m_CanPlay)
        {
            Debug.Log("It is not your turn to play");
            return false;
        }
        return true;
    }

    public void StartTurn(int turnNumber)
    {
        PlayerData.PlayerEconomy.AddNewTurnMana(turnNumber);
        m_CanPlay = true;
        m_SourceTile = null;
    }

    public bool EndTurn()
    {
        if (DM.TurnManager.TrySkipTurn())
        {
            m_CanPlay = false;
            DM.MouseFollower.ChangeFollowingTile(null);
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
        if (!CanPlay()) return;
        m_SourceTile = sourceTile;
        DM.MouseFollower.ChangeFollowingTile(sourceTile);
    }

    public void ResetSourceTile()
    {
        m_SourceTile = null;
        DM.MouseFollower.ChangeFollowingTile(null);
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
        if (!CanPlay()) return;
        DM.GameMode.MoveToDestinationTile(m_SourceTile, destinationTile, PlayerData);
        /*RpcDoAction(destinationTile);*/
    }

#if DEBUG
    public void AddInfiniteMana()
    {
        PlayerData.PlayerEconomy.AddNewTurnMana(500);
    }
#endif

}
