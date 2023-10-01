using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class Action : NetworkBehaviour
{
    protected DataManager DM { get; private set; }
    protected PlayerData PlayerData { get; private set; }

    public Action(DataManager dataManager, PlayerData playerData)
    {
        DM = dataManager;
        PlayerData = playerData;
    }
}
