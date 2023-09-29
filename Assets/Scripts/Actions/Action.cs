using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action
{
    protected DataManager DM { get; private set; }
    protected PlayerData PlayerData { get; private set; }

    public Action(DataManager dataManager, PlayerData playerData)
    {
        DM = dataManager;
        PlayerData = playerData;
    }
}
