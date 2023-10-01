using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class EndTurnAction : Action
{
    public EndTurnAction(DataManager dataManager, PlayerData playerData) : base(dataManager, playerData)
    {
    }

    [ServerRpc(RequireOwnership = false)]
    public void TrySkipTurnServerRPC(int playerID, ServerRpcParams serverRpcParams = default)
    {
        PlayerData playerData = DM.PlayersManager.GetPlayer(playerID);
        if (!playerData.PlayerActions.m_CanPlay) return;
        SkipTurnClientRPC(playerID);
        playerData.PlayerActions.EndTurn();
        DM.RewindManager.ClearAllActions();
        DM.TurnManager.StartLaserPhase();
    }

    [ClientRpc]
    private void SkipTurnClientRPC(int playerID)
    {
        PlayerData playerData = DM.PlayersManager.GetPlayer(playerID);
        playerData.PlayerActions.EndTurn();
        DM.RewindManager.ClearAllActions();
        DM.TurnManager.StartLaserPhase();
    }
}
