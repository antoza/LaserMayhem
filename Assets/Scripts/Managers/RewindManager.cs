using System.Collections.Generic;
using UnityEngine;
using System;
#nullable enable

public sealed class RewindManager : ScriptableObject
{
    public static RewindManager? Instance { get; private set; }

    private Stack<RevertableAction> m_actionsList;

    public RewindManager()
    {
        m_actionsList = new Stack<RevertableAction>();
    }

    public static void SetInstance()
    {
        Instance = new RewindManager();
    }

    public static RewindManager GetInstance()
    {
        if (Instance == null)
        {
            Debug.LogError("RewindManager has not been instantiated");
        }

        return Instance!;
    }

    public void AddAction(RevertableAction action)
    {
        m_actionsList.Push(action);
    }

    public void RevertLastAction()
    {
        if (m_actionsList.Count == 0)
        {
            Debug.Log("naaan");
            return;
        }
        RevertableAction lastAction = m_actionsList.Pop();
        lastAction.Revert();
    }

    public void ClearAllActions()
    {
        m_actionsList.Clear();
    }

    public void PrepareRevertLastAction()
    {
        PlayerData currentPlayer = PlayersManager.GetInstance().GetCurrentPlayer(); // Réfléchir à comment gérer ceci plus logiquement...
        if (m_actionsList.Count == 0) return;
        if (!currentPlayer.PlayerActions.CanPlay()) return;
        GameMessageManager.Instance!.TryRevertLastActionServerRPC(currentPlayer.m_playerID);
    }

    public void PrepareClearAllActions()
    {
        PlayerData currentPlayer = PlayersManager.GetInstance().GetCurrentPlayer();
        if (m_actionsList.Count == 0) return;
        if (!currentPlayer.PlayerActions.CanPlay()) return;
        GameMessageManager.Instance!.TryClearAllActionsServerRPC(currentPlayer.m_playerID);
    }
}
