using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using System;

#nullable enable
public class SendActionsManager : ScriptableObject
{
    public static SendActionsManager? Instance;

    public static void SetInstance()
    {
        if (GameInitialParameters.localPlayerID == -1)
        {
            Instance = new ServerSendActionsManager();
        }
        else
        {
            Instance = new ClientSendActionsManager();
        }
    }

    public static SendActionsManager GetInstance()
    {
        if (Instance == null)
        {
            Debug.LogError("SendActionsManager has not been instantiated");
        }

        return Instance!;
    }
}
