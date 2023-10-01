using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable
public class PlayerActions : ScriptableObject
{
    private PlayerData PlayerData;
    public bool m_CanPlay { get; private set; } = false;
    public Tile? m_SourceTile;

    public PlayerActions(PlayerData playerData)
    {
        PlayerData = playerData;
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
        if (CanPlay()) // EUUUUH BIZARRE LE JOUEUR 1 PEUT JOUER AU TOUR DU JOUEUR 0
        {
            TurnManager.GetInstance().PrepareSkipTurn(PlayerData.m_playerID);
        }
    }

    public void EndTurn()
    {
        m_CanPlay = false;
        DataManager.Instance.MouseFollower.ChangeFollowingTile(null);
    }

    public bool CopyPiece(Tile sourceTile, Tile destinationTile) // A déplacer dans un fichier plus adapté
    {
        if (sourceTile.m_Piece == null) return false;
        if (destinationTile.m_Piece != null) return false;

        destinationTile.UpdatePiece(sourceTile.m_Piece!);
        LaserManager.GetInstance().UpdateLaser(true);
        return true;
    }

    public bool DeletePiece(Tile tile) // A déplacer dans un fichier plus adapté
    {
        if (tile.m_Piece == null) return false;

        tile.UpdatePiece(null);
        LaserManager.GetInstance().UpdateLaser(true);
        return true;
    }

    public bool MovePiece(Tile sourceTile, Tile destinationTile) // A déplacer dans un fichier plus adapté
    {
        if (sourceTile == destinationTile) return false;
        if (sourceTile.m_Piece == null) return false;
        if (destinationTile.m_Piece != null) return false;

        destinationTile.UpdatePiece(sourceTile.m_Piece);
        sourceTile.UpdatePiece(null);
        LaserManager.GetInstance().UpdateLaser(true);
        return true;
    }

    public void SetSourceTile(Tile sourceTile)
    {
        if (!CanPlay()) return;
        m_SourceTile = sourceTile;
        DataManager.Instance.MouseFollower.ChangeFollowingTile(sourceTile);
    }

    public void ResetSourceTile()
    {
        m_SourceTile = null;
        DataManager.Instance.MouseFollower.ChangeFollowingTile(null);
    }

    public void PrepareMoveToDestinationTile(Tile destinationTile)
    {
        if (!CanPlay()) return;
        if (m_SourceTile == null) return; // Pas nécessaire mais permet d'éviter un envoi de message inutile au serveur
         GameMessageManager.Instance!.TryMoveToDestinationTileServerRPC(m_SourceTile.name, destinationTile.name, PlayerData.m_playerID);
    }

#if DEBUG
    public void AddInfiniteMana()
    {
        Debug.Log("aa");
        PlayerData.PlayerEconomy.AddNewTurnMana(500);
    }
#endif

}
