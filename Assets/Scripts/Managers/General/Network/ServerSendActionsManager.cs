using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using System;

#nullable enable
public class ServerSendActionsManager : SendActionsManager
{
    private List<List<Action>> actionsToSend;
    //private List<int> currentActionOrder = new List<int>();
    private List<bool> isWaitingForNewAction;

    private void Awake()
    {
        Instance = this;
        actionsToSend = new List<List<Action>>();
        isWaitingForNewAction = new List<bool>();

        for (int i = 0; i < PlayersManager.Instance.NumberOfPlayers; i++)
        {
            actionsToSend.Add(new List<Action>());
            isWaitingForNewAction.Add(false);
        }
    }

    public void ExecuteActionAndSendItToAllPlayers(Action action)
    {
        GameModeManager.Instance.ExecuteAction(action);
        for (int i = 0; i < PlayersManager.Instance.NumberOfPlayers; i++)
        {
            actionsToSend[i].Add(action);
            if (isWaitingForNewAction[i])
            {
                isWaitingForNewAction[i] = false;
                SendActionToPlayer(i, actionsToSend[i].Count - 1);
            }
        }
    }

    private void SendActionToPlayer(int playerID, int actionOrder)
    {
        Action actionToSend = actionsToSend[playerID][actionOrder];
        GameMessageManager.Instance.SendActionToPlayer(actionToSend, actionOrder, playerID);
    }

    public void ReceivePlayerRequest(int playerID, int actionOrder)
    {
        // TODO : if (actionOrder > actionsToSend[playerID].Count) DIRE QU'IL Y A UN PROBLEME
        if (actionOrder == actionsToSend[playerID].Count)
        {
            isWaitingForNewAction[playerID] = true;
        }
        else
        {
            SendActionToPlayer(playerID, actionOrder);
        }
    }

    public void ReceivePlayerAction(int playerID, Action action)
    {
        // If the player sends an action to the server, but isn't waiting for the latest action, there's a problem
        if (!isWaitingForNewAction[playerID])
        {
            // Thus the server sends them the very first action to engage an exchange
            SendActionToPlayer(playerID, 0);
            return;
        }

        if (action is not PlayerAction || playerID != ((PlayerAction)action).PlayerData.m_playerID)
        {
            return;
        }

        if (GameModeManager.Instance.VerifyAction((PlayerAction)action))
        {
            ExecuteActionAndSendItToAllPlayers(action);
        }
    }
}
