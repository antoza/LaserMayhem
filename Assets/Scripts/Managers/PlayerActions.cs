using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
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

    //TODO : A enlever
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

    public void CreateAndVerifyEndTurnAction()
    {
        EndTurnAction action = new EndTurnAction(PlayerData);
        ((ClientSendActionsManager)SendActionsManager.Instance).VerifyAndSendAction(action);
    }

    public void EndTurn()
    {
        m_CanPlay = false;
        DataManager.Instance.MouseFollower.ChangeFollowingTile(null);
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

    public void CreateAndVerifyMovePieceAction(Tile destinationTile)
    {
        if (m_SourceTile == null) return;
        MovePieceAction action = new MovePieceAction(PlayerData, m_SourceTile, destinationTile);
        ((ClientSendActionsManager)SendActionsManager.Instance).VerifyAndSendAction(action);
    }

    public void CreateAndVerifyRevertLastActionAction()
    {
        RevertLastActionAction action = new RevertLastActionAction(PlayerData);
        ((ClientSendActionsManager)SendActionsManager.Instance).VerifyAndSendAction(action);
    }

    public void CreateAndVerifyRevertAllActionsAction()
    {
        RevertAllActionsAction action = new RevertAllActionsAction(PlayerData);
        ((ClientSendActionsManager)SendActionsManager.Instance).VerifyAndSendAction(action);
    }

#if DEBUG
    public void AddInfiniteMana()
    {
        PlayerData.PlayerEconomy.AddNewTurnMana(500);
    }
#endif
}*/
