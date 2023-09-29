using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class RewindManager : ScriptableObject
{
    private DataManager DM;
    private Stack<RevertableAction> m_actionsList;

    public RewindManager(DataManager dataManager)
    {
        DM = dataManager;
        m_actionsList = new Stack<RevertableAction>();
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
        PlayerData currentPlayer = DM.PlayersManager.GetCurrentPlayer(); // Réfléchir à comment gérer ceci plus logiquement...
        if (m_actionsList.Count == 0) return;
        if (!currentPlayer.PlayerActions.CanPlay()) return;
        DM.GameMessageManager.TryRevertLastActionServerRPC(currentPlayer.m_playerID);
    }

    public void PrepareClearAllActions()
    {
        PlayerData currentPlayer = DM.PlayersManager.GetCurrentPlayer();
        if (m_actionsList.Count == 0) return;
        if (!currentPlayer.PlayerActions.CanPlay()) return;
        DM.GameMessageManager.TryClearAllActionsServerRPC(currentPlayer.m_playerID);
    }
}
