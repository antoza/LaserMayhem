using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action
{
    protected PlayerData PlayerData { get; private set; }

    public Action(PlayerData playerData)
    {
        PlayerData = playerData;
    }
}
