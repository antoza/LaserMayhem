using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using System;
using Unity.VisualScripting;

#nullable enable
public class ClientSendActionsManager : SendActionsManager
{
    private int currentActionOrder = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void RequestAction()
    {
        GameMessageManager.Instance.RequestActionToServer(currentActionOrder);
    }

    public void ReceiveAndExecuteAction(Action action, int actionOrder)
    {
        if (actionOrder == currentActionOrder)
        {
            DataManager.Instance.GameMode.ExecuteAction(action);
            currentActionOrder++;
        }
        RequestAction();
    }

    public void VerifyAndSendAction(PlayerAction action)
    {
        if (DataManager.Instance.GameMode.VerifyAction(action))
        {
            GameMessageManager.Instance.SendActionToServer(action);
        }
    }
}
