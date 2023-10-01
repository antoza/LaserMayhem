using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable
public class PlayerActions : ScriptableObject
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

    public void PrepareEndTurn()
    {
        if (CanPlay()) // TODO : EUUUUH BIZARRE LE JOUEUR 1 PEUT JOUER AU TOUR DU JOUEUR 0
        {
            DM.TurnManager.PrepareSkipTurn(PlayerData.m_playerID);
        }
    }

    public void EndTurn()
    {
        m_CanPlay = false;
        DM.MouseFollower.ChangeFollowingTile(null);
    }

    public bool CopyPiece(Tile sourceTile, Tile destinationTile) // TODO : A déplacer dans un fichier plus adapté
    {
        if (!sourceTile.m_Piece) return false;
        if (destinationTile.m_Piece) return false;

        destinationTile.UpdatePiece(sourceTile.m_Piece!);
        DM.LaserManager.UpdateLaser(true);
        return true;
    }

    public bool DeletePiece(Tile tile) // TODO : A déplacer dans un fichier plus adapté
    {
        if (!tile.m_Piece) return false;

        tile.UpdatePiece(null);
        DM.LaserManager.UpdateLaser(true);
        return true;
    }

    public bool MovePiece(Tile sourceTile, Tile destinationTile) // TODO : A déplacer dans un fichier plus adapté
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

    public void PrepareMoveToDestinationTile(Tile destinationTile)
    {
        if (!CanPlay()) return;
        if (m_SourceTile == null) return; // TODO : Pas nécessaire mais permet d'éviter un envoi de message inutile au serveur
        if (m_SourceTile.m_Piece == null) return; // TODO : Pas nécessaire mais permet d'éviter un envoi de message inutile au serveur
        MovePieceAction action = new MovePieceAction(DM, PlayerData, m_SourceTile, destinationTile, m_SourceTile.m_Piece!);
        action.AskServerRPC();
        //DM.GameMessageManager.TryMoveToDestinationTileServerRPC(m_SourceTile.name, destinationTile.name, PlayerData.m_playerID);
    }

#if DEBUG
    public void AddInfiniteMana()
    {
        Debug.Log("aa");
        PlayerData.PlayerEconomy.AddNewTurnMana(500);
    }
#endif

}
